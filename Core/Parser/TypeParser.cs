// Copyright 2012 rubicon informationstechnologie gmbh
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using InjectionCop.Attributes;
using InjectionCop.Config;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  public class TypeParser: BaseFxCopRule
  {
    private IBlackTypes blackTypes;
    private Dictionary<Identifier, bool> safeSymbols;

    public TypeParser (IBlackTypes _blackTypes): base("TypeParser")
    {
      blackTypes = _blackTypes;
      safeSymbols = new Dictionary<Identifier, bool>();
    }

    public override ProblemCollection Check (Member member)
    {
      Method method = member as Method;
      if(method == null)
        return Problems;
 
      foreach (Parameter parameter in method.Parameters)
      {
        if(Is<SqlFragment>(parameter))
          safeSymbols[parameter.Name] = true;
        else
          safeSymbols[parameter.Name] =  false;
      }
      
      foreach (Statement topLevelStatement in method.Body.Statements)
      {
        Block methodBodyBlock = topLevelStatement as Block;
        if (methodBodyBlock == null)
          continue;

        foreach (Statement stmt in methodBodyBlock.Statements)
        {
          switch (stmt.NodeType)
          {
            case NodeType.ExpressionStatement:
              ExpressionStatement exprStmt = stmt as ExpressionStatement;
              if (exprStmt == null)
                continue;

              Check (exprStmt.Expression);
              break;
            
            case NodeType.AssignmentStatement:
              AssignmentStatement asgn = (AssignmentStatement) stmt;
              Identifier symbol = GetIdentifier(asgn.Target);
              safeSymbols[symbol] = IsSafe(asgn.Source);

              Check (asgn.Source);
              break;
              
            case NodeType.Return:
              ReturnNode returnNode = (ReturnNode) stmt;
              Check (returnNode.Expression);
              break;
          }
        }
      }
      return Problems;
    }

    private Identifier GetIdentifier (Expression target)
    {
      if(target is Local)
        return ((Local) target).Name;
              
      if(target is Parameter)
        return ((Parameter) target).Name;

      throw new InjectionCopException("Failed to extract Identifier");
    }

    private void Check (Expression expression)
    {
      if (expression is MethodCall)
      {
        MethodCall mtc = (MethodCall) expression;
        if (!ParametersSafe (mtc))
          AddProblem();

        UpdateSafeOutParameters (mtc);
        return;
      }

      if (expression is UnaryExpression)
      {
        UnaryExpression uExpression = (UnaryExpression) expression;
        Check (uExpression.Operand);
      }
    }

    private void UpdateSafeOutParameters (MethodCall mtc)
    {
      Method method = ExtractMethod (mtc);
      for(int i = 0; i < mtc.Operands.Count; i++)
      {
        if(method.Parameters[i].IsOut)
        {
          Identifier symbol = GetIdentifier(mtc.Operands[i]);
          bool safeness = Contains<SqlFragment> (method.Parameters[i].Attributes);
          safeSymbols[symbol] = safeness;
        }
      }
    }

    private bool IsSafe(Expression expression)
    {
      bool expressionCheck = expression is Literal
                             || Returns<SqlFragment> (expression);
      
      bool tableLookup = false;
      if(expression is Parameter)
      {
        Parameter parameter = (Parameter) expression;
        if(safeSymbols.ContainsKey(parameter.Name))
          tableLookup = safeSymbols[parameter.Name];
        
      }

      if(expression is Local)
      {
        Local local = (Local) expression;
        if(safeSymbols.ContainsKey(local.Name))
          tableLookup = safeSymbols[local.Name];
        
      }

      bool safeMethodCall = false;
      if(expression is MethodCall)
      {
        Method mtd = ExtractMethod((MethodCall) expression);
        safeMethodCall = Contains<SqlFragment> (mtd.ReturnAttributes);
      }

      return expressionCheck || tableLookup || safeMethodCall;
    }

    private bool IsNotSafe (Expression expression)
    {
      return !IsSafe (expression);
    }

    private bool ParametersSafe(MethodCall mtc)
    {
      Method calleeMtd = ExtractMethod (mtc);

      if(blackTypes.IsBlackMethod(calleeMtd.DeclaringType.FullName, calleeMtd.Name.Name))
      {
        foreach (Expression expression in mtc.Operands)
        {
          if (IsNotSafe (expression))
          {
            return false;
          }
        }
      }

      for(int i = 0; i < calleeMtd.Parameters.Count; i++)
      {
        if(Contains<SqlFragment>(calleeMtd.Parameters[i].Attributes)
          && IsNotSafe(mtc.Operands[i]))
        {
          return false;
        }
      }
      return true;
    }

    private void AddProblem()
    {
      Resolution resolution = GetResolution();
      Problem problem = new Problem (resolution, CheckId);
      Problems.Add (problem);
    }

    private Method ExtractMethod(MethodCall mtc)
    {
      MemberBinding mbCallee = mtc.Callee as MemberBinding;
      if (mbCallee == null || !(mbCallee.BoundMember is Method))
        throw new InjectionCopException ("Cannot extract Method from Methodcall");
      
      Method calleeMtd = (Method)mbCallee.BoundMember;
      return calleeMtd;
    }

    private bool Is<F> (Expression expression)
      where F: Fragment
    {
      Parameter parameter = expression as Parameter;
      if(parameter == null)
        return false;

      return Contains<F> (parameter.Attributes);
    }

    private bool Returns<F>(Expression expression)
      where F: Fragment
    {
      MethodCall mtc = expression as MethodCall;
      if (mtc == null)
        return false;

      Method calleeMtd = ExtractMethod (mtc);
      return Contains<F> (calleeMtd.ReturnAttributes);
    }

    private bool Contains<F>(AttributeNodeCollection attributes)
      where F: Fragment
    {
      try
      {
        TypeNode fragmentTypeNode = Helper.TypeNodeFactory<F>();
        return attributes.Any (attribute => 
          attribute.Type.FullName == fragmentTypeNode.FullName
        );
      }
      catch (ArgumentNullException)
      {
        return false;
      }
    }

  }
}