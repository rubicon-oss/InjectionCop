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
using InjectionCop.Config;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  public class BlockParser : BaseFxCopRule
  {
    private SymbolTable _symbolTableParser;
    private List<PreCondition> _preConditions;
    private List<int> _successors;
    private readonly IBlackTypes _blackTypes;
    private TypeParser _typeParser;

    public BlockParser (IBlackTypes blackTypes, TypeParser typeParser)
        : base ("TypeParser")
    {
      _symbolTableParser = new SymbolTable (blackTypes);
      _preConditions = new List<PreCondition>();
      _successors = new List<int>();
      _blackTypes = blackTypes;
      _typeParser = typeParser;
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
            _symbolTableParser.InferSafeness(symbol, asgn.Source);
            Inspect (asgn.Source);
            break;

          case NodeType.Return:
            ReturnNode returnNode = (ReturnNode) stmt;
            Inspect (returnNode.Expression);
            break;

          case NodeType.Branch:
            Branch branch = (Branch) stmt;
            _successors.Add (branch.Target.UniqueKey);
            break;

          case NodeType.SwitchInstruction:
            SwitchInstruction switchInstruction = (SwitchInstruction) stmt;
            foreach (Block caseBlock in switchInstruction.Targets)
            {
              _successors.Add (caseBlock.UniqueKey);
            }
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
        List<PreCondition> additionalPreConditions;
        if (!_symbolTableParser.ParametersSafe (methodCall, out additionalPreConditions))
        {
          _typeParser.AddProblem();
        }
        _preConditions.AddRange (additionalPreConditions);
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
            if (FragmentTools.ContainsFragment (method.Parameters[i].Attributes))
            {
              string fragmentType = FragmentTools.GetFragmentType (method.Parameters[i].Attributes);
              _symbolTableParser.SetSafeness (symbol, fragmentType, true);
            }
            else
            {
              _symbolTableParser.MakeUnsafe (symbol);
            }
          }
        }
      }
    }

    public BasicBlock Parse (Block block)
    {
      Reset();
      Inspect (block);
      BasicBlock basicBlock = new BasicBlock (block.UniqueKey, _preConditions.ToArray(), _symbolTableParser, _successors.ToArray());
      return basicBlock;
    }

    public BasicBlock Parse (Block block, int directSuccessorKey)
    {
      Reset();
      _successors.Add (directSuccessorKey);
      Inspect (block);
      BasicBlock basicBlock = new BasicBlock (block.UniqueKey, _preConditions.ToArray(), _symbolTableParser, _successors.ToArray());
      return basicBlock;
    }

    private void Reset()
    {
      _symbolTableParser = new SymbolTable (_blackTypes);
      _preConditions = new List<PreCondition>();
      _successors = new List<int>();
    }
  }
}