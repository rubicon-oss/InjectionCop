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
  public class TypeParser : BaseFxCopRule
  {
    private readonly IBlackTypes _blackTypes;
    private readonly Dictionary<Identifier, bool> _safeSymbols;

    public TypeParser (IBlackTypes _blackTypes)
        : base ("TypeParser")
    {
      this._blackTypes = _blackTypes;
      _safeSymbols = new Dictionary<Identifier, bool>();
    }

    public override ProblemCollection Check (Member member)
    {
      Method method = member as Method;

      if (method != null)
      {
        foreach (Parameter parameter in method.Parameters)
        {
          if (Is<SqlFragmentAttribute> (parameter))
          {
            _safeSymbols[parameter.Name] = true;
          }
          else
          {
            _safeSymbols[parameter.Name] = false;
          }
        }

        foreach (Statement topLevelStatement in method.Body.Statements)
        {
          Block methodBodyBlock = topLevelStatement as Block;
          if (methodBodyBlock != null)
          {
            Inspect (methodBodyBlock);
          }
        }
      }

      return Problems;
    }

    private void Inspect (Block methodBodyBlock)
    {
      foreach (Statement stmt in methodBodyBlock.Statements)
      {
        switch (stmt.NodeType)
        {
          case NodeType.ExpressionStatement:
            ExpressionStatement exprStmt = (ExpressionStatement) stmt;
            Inspect (exprStmt.Expression);
            break;

          case NodeType.AssignmentStatement:
            AssignmentStatement asgn = (AssignmentStatement) stmt;
            Identifier symbol = GetVariableIdentifier (asgn.Target);
            _safeSymbols[symbol] = IsSafe (asgn.Source);
            Inspect (asgn.Source);
            break;

          case NodeType.Return:
            ReturnNode returnNode = (ReturnNode) stmt;
            Inspect (returnNode.Expression);
            break;

          case NodeType.Branch:
            Branch branch = (Branch) stmt;
            Inspect (branch.Target);
            break;
        }
      }
    }

    private Identifier GetVariableIdentifier (Expression target)
    {
      Identifier identifier;

      if (target is Local)
      {
        identifier = ((Local) target).Name;
      }
      else if (target is Parameter)
      {
        identifier = ((Parameter) target).Name;
      }
      else if (target is UnaryExpression)
      {
        identifier = GetVariableIdentifier (((UnaryExpression) target).Operand);
      }
      else
      {
        throw new InjectionCopException ("Failed to extract Identifier");
      }

      return identifier;
    }

    private bool IsVariable (Expression target)
    {
      if (target is UnaryExpression)
      {
        Expression operand = ((UnaryExpression) target).Operand;
        return IsVariable (operand);
      }

      return target is Local || target is Parameter;
    }

    private void Inspect (Expression expression)
    {
      if (expression is MethodCall)
      {
        MethodCall methodCall = (MethodCall) expression;
        if (!ParametersSafe (methodCall))
        {
          AddProblem();
        }
        UpdateSafeOutParameters (methodCall);
      }
      else if (expression is UnaryExpression)
      {
        UnaryExpression unaryExpression = (UnaryExpression) expression;
        Inspect (unaryExpression.Operand);
      }
    }

    private void UpdateSafeOutParameters (MethodCall methodCall)
    {
      Method method = ExtractMethod (methodCall);

      for (int i = 0; i < methodCall.Operands.Count; i++)
      {
        if (IsVariable (methodCall.Operands[i]))
        {
          bool isRef = !method.Parameters[i].IsOut && !method.Parameters[i].IsIn;
          if (method.Parameters[i].IsOut || isRef)
          {
            Identifier symbol = GetVariableIdentifier (methodCall.Operands[i]);
            bool safeness = Contains<SqlFragmentAttribute> (method.Parameters[i].Attributes);
            _safeSymbols[symbol] = safeness;
          }
        }
      }
    }

    private bool IsSafe (Expression expression)
    {
      bool literalOrFragment = expression is Literal || Returns<SqlFragmentAttribute> (expression);
      bool safeVariable = false;
      bool safeUnaryExpression = false;

      if (expression is Parameter)
      {
        Parameter parameter = (Parameter) expression;
        if (_safeSymbols.ContainsKey (parameter.Name))
        {
          safeVariable = _safeSymbols[parameter.Name];
        }
      }
      else if (expression is Local)
      {
        Local local = (Local) expression;
        if (_safeSymbols.ContainsKey (local.Name))
        {
          safeVariable = _safeSymbols[local.Name];
        }
      }
      else if (expression is UnaryExpression)
      {
        safeUnaryExpression = IsSafe (((UnaryExpression) expression).Operand);
      }

      return literalOrFragment || safeVariable || safeUnaryExpression;
    }

    private bool IsNotSafe (Expression expression)
    {
      return !IsSafe (expression);
    }

    private bool ParametersSafe (MethodCall methodCall)
    {
      bool parameterSafe = true;
      Method calleeMethod = ExtractMethod (methodCall);

      if (_blackTypes.IsBlackMethod (calleeMethod.DeclaringType.FullName, calleeMethod.Name.Name))
      {
        foreach (Expression expression in methodCall.Operands)
        {
          if (IsNotSafe (expression))
          {
            parameterSafe = false;
          }
        }
      }

      for (int i = 0; i < calleeMethod.Parameters.Count; i++)
      {
        bool isFragmentParameter = Contains<SqlFragmentAttribute> (calleeMethod.Parameters[i].Attributes);
        if (isFragmentParameter && IsNotSafe (methodCall.Operands[i]))
        {
          parameterSafe = false;
        }
      }

      return parameterSafe;
    }

    private void AddProblem ()
    {
      Resolution resolution = GetResolution();
      Problem problem = new Problem (resolution, CheckId);
      Problems.Add (problem);
    }

    private Method ExtractMethod (MethodCall methodCall)
    {
      MemberBinding callee = methodCall.Callee as MemberBinding;
      if (callee == null || !(callee.BoundMember is Method))
        throw new InjectionCopException ("Cannot extract Method from Methodcall");

      Method boundMember = (Method) callee.BoundMember;
      return boundMember;
    }

    private bool Is<F> (Expression expression)
        where F : FragmentAttribute
    {
      bool isFragment = false;

      Parameter parameter = expression as Parameter;
      if (parameter != null)
      {
        isFragment = Contains<F> (parameter.Attributes);
      }

      return isFragment;
    }

    private bool Returns<F> (Expression expression)
        where F : FragmentAttribute
    {
      bool returnsFragment = false;

      MethodCall methodCall = expression as MethodCall;
      if (methodCall != null)
      {
        Method calleeMethod = ExtractMethod (methodCall);
        returnsFragment = Contains<F> (calleeMethod.ReturnAttributes);
      }

      return returnsFragment;
    }

    private bool Contains<F> (AttributeNodeCollection attributes)
        where F : FragmentAttribute
    {
      bool containsFragment;

      try
      {
        TypeNode fragmentTypeNode = Helper.TypeNodeFactory<F>();
        containsFragment = attributes.Any (
            attribute =>
            attribute.Type.FullName == fragmentTypeNode.FullName
            );
      }
      catch (ArgumentNullException)
      {
        containsFragment = false;
      }

      return containsFragment;
    }
  }
}