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
using InjectionCop.Attributes;
using InjectionCop.Config;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  public class BlockParser : BaseFxCopRule
  {
    private SymbolTable _safenessManager;
    private FragmentAttribute sqlFragment = new FragmentAttribute("SqlFragment");

    public BlockParser (IBlackTypes blackTypes)
        : base ("TypeParser")
    {
      _safenessManager = new SymbolTable (blackTypes);
    }

    public override ProblemCollection Check (Member member)
    {
      Method method = member as Method;

      if (method != null)
      {
        foreach (Parameter parameter in method.Parameters)
        {
          if (FragmentTools.Is(sqlFragment, parameter))
          {
            _safenessManager.SetSafeness(parameter.Name, true);
          }
          else
          {
            _safenessManager.SetSafeness(parameter.Name, false);
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
            _safenessManager.SetSafeness(symbol, asgn.Source);
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
        if (!_safenessManager.ParametersSafe (methodCall))
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
      Method method = IntrospectionTools.ExtractMethod(methodCall);

      for (int i = 0; i < methodCall.Operands.Count; i++)
      {
        if (IsVariable (methodCall.Operands[i]))
        {
          bool isRef = !method.Parameters[i].IsOut && !method.Parameters[i].IsIn;
          if (method.Parameters[i].IsOut || isRef)
          {
            Identifier symbol = GetVariableIdentifier (methodCall.Operands[i]);
            //bool safeness = FragmentTools.Contains<SqlFragmentAttribute> (method.Parameters[i].Attributes);
            bool safeness = FragmentTools.Contains(sqlFragment, method.Parameters[i].Attributes);
            _safenessManager.SetSafeness(symbol, safeness);
          }
        }
      }
    }

    private void AddProblem ()
    {
      Resolution resolution = GetResolution();
      Problem problem = new Problem (resolution, CheckId);
      Problems.Add (problem);
    }
  }
}