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
using InjectionCop.Attributes;
using InjectionCop.Config;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  public class BlockParser : BaseFxCopRule
  {
    private readonly SymbolTable _symbolTable;
    private List<string> _preConditionSafeSymbols;
    private List<int> _successors;
    private readonly FragmentAttribute sqlFragment = new FragmentAttribute("SqlFragment");

    public BlockParser (IBlackTypes blackTypes)
        : base ("TypeParser")
    {
      _symbolTable = new SymbolTable (blackTypes);
      _preConditionSafeSymbols = new List<string>();
      _successors = new List<int>();
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
            _symbolTable.SetSafeness(parameter.Name, true);
          }
          else
          {
            _symbolTable.SetSafeness(parameter.Name, false);
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
            _symbolTable.SetSafeness(symbol, asgn.Source);
            Inspect (asgn.Source);
            break;

          case NodeType.Return:
            ReturnNode returnNode = (ReturnNode) stmt;
            Inspect (returnNode.Expression);
            break;

          case NodeType.Branch:
            Branch branch = (Branch) stmt;
            _successors.Add (branch.Target.UniqueKey);
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
        List<string> additionalPreConditions;
        if (!_symbolTable.ParametersSafe (methodCall, out additionalPreConditions))
        {
          _preConditionSafeSymbols.AddRange (additionalPreConditions);
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
          if (method.Parameters[i].IsOut)
          {
            Identifier symbol = GetVariableIdentifier (methodCall.Operands[i]);
            //bool safeness = FragmentTools.Contains<SqlFragmentAttribute> (method.Parameters[i].Attributes);
            bool safeness = FragmentTools.Contains(sqlFragment, method.Parameters[i].Attributes);
            _symbolTable.SetSafeness(symbol, safeness);
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

    public BasicBlock Parse (Block block, int directSuccessorKey = 0)
    {
      _successors.Add (directSuccessorKey);
      Inspect (block);
      BasicBlock basicBlock = new BasicBlock (_preConditionSafeSymbols.ToArray(), _symbolTable, _successors.ToArray());

      return basicBlock;
    }
  }
}