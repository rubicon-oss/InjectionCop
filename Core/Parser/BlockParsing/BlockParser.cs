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
    private List<BlockAssignment> _blockAssignments;
    private readonly IBlacklistManager _blacklistManager;
    private readonly IProblemPipe _problemPipe;
    private readonly string _returnFragmentType;
    private List<string> _assignmentTargetVariables;

    public BlockParser (IBlacklistManager blacklistManager, IProblemPipe problemPipe, string returnFragmentType)
    {
      _blacklistManager = ArgumentUtility.CheckNotNull ("blacklistManager", blacklistManager);
      _problemPipe = ArgumentUtility.CheckNotNull ("typeParser", problemPipe);
      _symbolTableParser = new SymbolTable (blacklistManager);
      _preConditions = new List<PreCondition>();
      _successors = new List<int>();
      _blockAssignments = new List<BlockAssignment>();
      _returnFragmentType = ArgumentUtility.CheckNotNullOrEmpty("returnFragmentType", returnFragmentType);
      _assignmentTargetVariables = new List<string>();
    }

    public BasicBlock Parse (Block block)
    {
      ArgumentUtility.CheckNotNull ("block", block);
      Reset();
      Inspect (block);
      BasicBlock basicBlock = new BasicBlock (block.UniqueKey, _preConditions.ToArray(), _symbolTableParser, _successors.ToArray(), _blockAssignments.ToArray());
      return basicBlock;
    }

    public BasicBlock Parse (Block block, int directSuccessorKey)
    {
      ArgumentUtility.CheckNotNull ("block", block);
      Reset();
      _successors.Add (directSuccessorKey);
      Inspect (block);
      BasicBlock basicBlock = new BasicBlock (block.UniqueKey, _preConditions.ToArray(), _symbolTableParser, _successors.ToArray(), _blockAssignments.ToArray());
      return basicBlock;
    }

    private void Reset ()
    {
      _symbolTableParser = new SymbolTable (_blacklistManager);
      _preConditions = new List<PreCondition>();
      _successors = new List<int>();
      _blockAssignments = new List<BlockAssignment>();
      _assignmentTargetVariables = new List<string>();
    }

    private void Inspect (Block methodBodyBlock)
    {
      foreach (Statement statement in methodBodyBlock.Statements)
      {
        switch (statement.NodeType)
        {
          case NodeType.ExpressionStatement:
            ExpressionStatement expressionStatement = (ExpressionStatement) statement;
            Inspect (expressionStatement.Expression);
            break;

          case NodeType.AssignmentStatement:
            AssignmentStatement assignmentStatement = (AssignmentStatement) statement;
            AssignmentStatementHandler (assignmentStatement);
            break;

          case NodeType.Return:
            ReturnNode returnNode = (ReturnNode) statement;
            ReturnStatementHandler (returnNode);
            break;

          case NodeType.Branch:
            Branch branch = (Branch) statement;
            _successors.Add (branch.Target.UniqueKey);
            break;

          case NodeType.SwitchInstruction:
            SwitchInstruction switchInstruction = (SwitchInstruction) statement;
            foreach (Block caseBlock in switchInstruction.Targets)
            {
              _successors.Add (caseBlock.UniqueKey);
            }
            break;
          //default:
            // exception werfen + logging
        }
      }
    }

    private void AssignmentStatementHandler (AssignmentStatement assignmentStatement)
    {
      string targetSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Target);
      _assignmentTargetVariables.Add (targetSymbol);

      string sourceSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Source);
      bool methodVariableNotAssignedInsideCurrentBlock =
          sourceSymbol != null
          && !_assignmentTargetVariables.Contains (sourceSymbol)
          && !IntrospectionUtility.IsField (assignmentStatement.Source);
      if (methodVariableNotAssignedInsideCurrentBlock)
      {
        BlockAssignment blockAssignment = new BlockAssignment (sourceSymbol, targetSymbol);
        _blockAssignments.Add (blockAssignment);
      }
      else
      {
        _symbolTableParser.InferSafeness (targetSymbol, assignmentStatement.Source);
        VerifyAssignmentTargetFragmentType (assignmentStatement.Target);
      }
      Inspect (assignmentStatement.Source);
    }
    
    private void VerifyAssignmentTargetFragmentType (Expression targetExpression)
    {
      if (IntrospectionUtility.IsField (targetExpression))
      {
        Field target = IntrospectionUtility.GetField (targetExpression);
        string symbol = target.Name.Name;

        if (FragmentUtility.ContainsFragment (target.Attributes))
        {
          string targetFragmentType = FragmentUtility.GetFragmentType (target.Attributes);
          string givenFragmentType = _symbolTableParser.GetFragmentType (symbol);
          if (targetFragmentType != givenFragmentType && givenFragmentType != SymbolTable.LITERAL)
          {
            ProblemMetadata problemMetadata = new ProblemMetadata (
                targetExpression.UniqueKey,
                targetExpression.SourceContext,
                targetFragmentType,
                givenFragmentType);
            _problemPipe.AddProblem (problemMetadata);
          }
        }
      }
    }

    private void ReturnStatementHandler (ReturnNode returnNode)
    {
      if (returnNode.Expression != null)
      {
        Inspect (returnNode.Expression);
        string returnSymbol = IntrospectionUtility.GetVariableName (returnNode.Expression);
        ProblemMetadata problemMetadata = new ProblemMetadata (
            returnNode.UniqueKey, returnNode.SourceContext, _returnFragmentType, _symbolTableParser.GetFragmentType (returnSymbol));
        PreCondition returnBlockCondition = new PreCondition (returnSymbol, _returnFragmentType, problemMetadata);
        _preConditions.Add (returnBlockCondition);
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
            if (FragmentUtility.ContainsFragment (method.Parameters[i].Attributes))
            {
              string fragmentType = FragmentUtility.GetFragmentType (method.Parameters[i].Attributes);
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