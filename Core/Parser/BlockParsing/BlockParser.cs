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
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.BlockParsing
{
  /// <summary>
  /// Takes a block (from a method body) and creates the corresponding BasicBlock
  /// </summary>
  public class BlockParser
  {
    private ISymbolTable _symbolTableParser;
    private List<PreCondition> _preConditions;
    private List<int> _successors;
    private readonly IBlacklistManager _blacklistManager;
    private readonly IProblemPipe _problemPipe;

    public BlockParser (IBlacklistManager blacklistManager, IProblemPipe problemPipe)
    {
      _blacklistManager = ArgumentUtility.CheckNotNull ("blacklistManager", blacklistManager);
      _problemPipe = ArgumentUtility.CheckNotNull ("typeParser", problemPipe);
      _symbolTableParser = new SymbolTable (blacklistManager);
      _preConditions = new List<PreCondition>();
      _successors = new List<int>();
    }

    public BasicBlock Parse (Block block)
    {
      ArgumentUtility.CheckNotNull ("block", block);
      Reset();
      Inspect (block);
      BasicBlock basicBlock = new BasicBlock (block.UniqueKey, _preConditions.ToArray(), _symbolTableParser, _successors.ToArray());
      return basicBlock;
    }

    public BasicBlock Parse (Block block, int directSuccessorKey)
    {
      ArgumentUtility.CheckNotNull ("block", block);
      Reset();
      _successors.Add (directSuccessorKey);
      Inspect (block);
      BasicBlock basicBlock = new BasicBlock (block.UniqueKey, _preConditions.ToArray(), _symbolTableParser, _successors.ToArray());
      return basicBlock;
    }

    private void Reset ()
    {
      _symbolTableParser = new SymbolTable (_blacklistManager);
      _preConditions = new List<PreCondition>();
      _successors = new List<int>();
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
            string symbol = IntrospectionUtility.GetVariableName (asgn.Target);
            _symbolTableParser.InferSafeness (symbol, asgn.Source);
            InspectIfFieldAssignment (asgn.Target);
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

    private void InspectIfFieldAssignment (Expression targetExpression)
    {
      if (IntrospectionUtility.IsField (targetExpression))
      {
        string symbol = IntrospectionUtility.GetVariableName (targetExpression);
        Field target = IntrospectionUtility.GetField (targetExpression);
        if (FragmentUtilities.ContainsFragment (target.Attributes))
        {
          string targetFragmentType = FragmentUtilities.GetFragmentType (target.Attributes);
          string givenFragmentType = _symbolTableParser.GetFragmentType (symbol);
          if (targetFragmentType != givenFragmentType)
          {
            ProblemMetadata problemMetadata = new ProblemMetadata (
                targetExpression.UniqueKey,
                targetExpression.SourceContext,
                targetFragmentType,
                givenFragmentType);
            _problemPipe.AddProblem(problemMetadata);
          }
        }
      }
    }

    private void Inspect (Expression expression)
    {
      if (expression is MethodCall)
      {
        MethodCall methodCall = (MethodCall) expression;
        CheckParameters (methodCall);
        UpdateSafeOutParameters (methodCall);
        foreach (Expression operand in methodCall.Operands)
        {
          Inspect (operand);
        }
      }
      else if (expression is UnaryExpression)
      {
        var unaryExpression = (UnaryExpression) expression;
        Inspect (unaryExpression.Operand);
      }
      else if (expression is BinaryExpression)
      {
        var binaryExpression = (BinaryExpression) expression;
        Inspect (binaryExpression.Operand1);
        Inspect (binaryExpression.Operand2);
      }
    }

    private void CheckParameters (MethodCall methodCall)
    {
      List<PreCondition> additionalPreConditions;
      List<ProblemMetadata> parameterProblems;
      _symbolTableParser.ParametersSafe (methodCall, out additionalPreConditions, out parameterProblems);
      parameterProblems.ForEach (parameterProblem => _problemPipe.AddProblem (parameterProblem));
      _preConditions.AddRange (additionalPreConditions);
    }

    private void UpdateSafeOutParameters (MethodCall methodCall)
    {
      Method method = IntrospectionUtility.ExtractMethod (methodCall);
      
      for (int i = 0; i < methodCall.Operands.Count; i++)
      {
        if (IntrospectionUtility.IsVariable (methodCall.Operands[i]))
        {
          if (method.Parameters[i].IsOut)
          {
            string symbol = IntrospectionUtility.GetVariableName (methodCall.Operands[i]);
            if (FragmentUtilities.ContainsFragment (method.Parameters[i].Attributes))
            {
              string fragmentType = FragmentUtilities.GetFragmentType (method.Parameters[i].Attributes);
              _symbolTableParser.MakeSafe (symbol, fragmentType);
            }
            else
            {
              _symbolTableParser.MakeUnsafe (symbol);
            }
          }
        }
      }
    }
  }
}