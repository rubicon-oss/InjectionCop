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
using InjectionCop.Parser.MethodParsing;
using InjectionCop.Parser.BlockParsing.StatementHandler;
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
    private List<IPreCondition> _preConditions;
    private List<int> _successors;
    private List<BlockAssignment> _blockAssignments;
    private readonly IBlacklistManager _blacklistManager;
    private readonly IProblemPipe _problemPipe;
    private readonly string _returnFragmentType;
    private List<string> _assignmentTargetVariables;
    private List<ReturnCondition> _returnConditions;

    public BlockParser (IBlacklistManager blacklistManager, IProblemPipe problemPipe, string returnFragmentType, List<ReturnCondition> returnConditions)
    {
      _blacklistManager = ArgumentUtility.CheckNotNull ("blacklistManager", blacklistManager);
      _problemPipe = ArgumentUtility.CheckNotNull ("typeParser", problemPipe);
      _symbolTableParser = new SymbolTable (blacklistManager);
      _preConditions = new List<IPreCondition>();
      _successors = new List<int>();
      _blockAssignments = new List<BlockAssignment>();
      _returnFragmentType = ArgumentUtility.CheckNotNullOrEmpty("returnFragmentType", returnFragmentType);
      _assignmentTargetVariables = new List<string>();
      _returnConditions = returnConditions;
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
      _preConditions = new List<IPreCondition>();
      _successors = new List<int>();
      _blockAssignments = new List<BlockAssignment>();
      _assignmentTargetVariables = new List<string>();
    }

    private void Inspect (Block methodBodyBlock)
    {
      ReturnStatementHandler returnStatementHandler = new ReturnStatementHandler(_problemPipe, _returnFragmentType, _returnConditions, _blacklistManager, Inspect);
      AssignmentStatementHandler assignmentStatementHandler = new AssignmentStatementHandler(_problemPipe, _returnFragmentType, _returnConditions, _blacklistManager, Inspect);
      ExpressionStatementHandler expressionStatementHandler = new ExpressionStatementHandler(_problemPipe, _returnFragmentType, _returnConditions, _blacklistManager, Inspect);
      BranchStatementHandler branchStatementHandler = new BranchStatementHandler(_problemPipe, _returnFragmentType, _returnConditions, _blacklistManager, Inspect);
      SwitchStatementHandler switchStatementHandler = new SwitchStatementHandler(_problemPipe, _returnFragmentType, _returnConditions, _blacklistManager, Inspect);

      foreach (Statement statement in methodBodyBlock.Statements)
      {
        switch (statement.NodeType)
        {
          case NodeType.ExpressionStatement:
            expressionStatementHandler.Handle (statement, _symbolTableParser, _preConditions, _assignmentTargetVariables, _blockAssignments, _successors);
            break;

          case NodeType.AssignmentStatement:
            assignmentStatementHandler.Handle  (statement, _symbolTableParser, _preConditions, _assignmentTargetVariables, _blockAssignments, _successors);
            break;

          case NodeType.Return:
            returnStatementHandler.Handle(statement, _symbolTableParser, _preConditions, _assignmentTargetVariables, _blockAssignments, _successors);
            break;

          case NodeType.Branch:
            branchStatementHandler.Handle(statement, _symbolTableParser, _preConditions, _assignmentTargetVariables, _blockAssignments, _successors);
            break;

          case NodeType.SwitchInstruction:
            switchStatementHandler.Handle(statement, _symbolTableParser, _preConditions, _assignmentTargetVariables, _blockAssignments, _successors);
            break;
        }
      }
    }

    private void Inspect (Expression expression)
    {
      if (expression is MethodCall)
      {
        MethodCall methodCall = (MethodCall) expression;
        CheckParameters (methodCall);
        UpdateOutAndRefSymbols (methodCall);
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
      List<IPreCondition> additionalPreConditions;
      List<ProblemMetadata> parameterProblems;
      ParametersSafe (methodCall, out additionalPreConditions, out parameterProblems);
      parameterProblems.ForEach (parameterProblem => _problemPipe.AddProblem (parameterProblem));
      _preConditions.AddRange (additionalPreConditions);
    }

    private void ParametersSafe(MethodCall methodCall, out List<IPreCondition> requireSafenessParameters, out List<ProblemMetadata> parameterProblems)
    {
      ArgumentUtility.CheckNotNull("methodCall", methodCall);

      requireSafenessParameters = new List<IPreCondition>();
      parameterProblems = new List<ProblemMetadata>();
      Method calleeMethod = IntrospectionUtility.ExtractMethod(methodCall);
      string[] parameterFragmentTypes = _symbolTableParser.InferParameterFragmentTypes(calleeMethod);

      for (int i = 0; i < parameterFragmentTypes.Length; i++)
      {
        Expression operand = methodCall.Operands[i];
        string operandFragmentType = _symbolTableParser.InferFragmentType(operand);
        string parameterFragmentType = parameterFragmentTypes[i];

        if (operandFragmentType != SymbolTable.LITERAL
            && parameterFragmentType != SymbolTable.EMPTY_FRAGMENT
            && operandFragmentType != parameterFragmentType)
        {
          string variableName;
          ProblemMetadata problemMetadata = new ProblemMetadata(operand.UniqueKey, operand.SourceContext, parameterFragmentType, operandFragmentType);
          if (IntrospectionUtility.IsVariable(operand, out variableName)
              && !_symbolTableParser.Contains(variableName))
          {
            requireSafenessParameters.Add(new AssignabilityPreCondition(variableName, parameterFragmentType, problemMetadata));
          }
          else
          {
            parameterProblems.Add(problemMetadata);
          }
        }
      }
    }

    private void UpdateOutAndRefSymbols (MethodCall methodCall)
    {
      Method method = IntrospectionUtility.ExtractMethod (methodCall);
      for (int i = 0; i < methodCall.Operands.Count; i++)
      {
        if (IntrospectionUtility.IsVariable (methodCall.Operands[i])
            && (method.Parameters[i].IsOut || method.Parameters[i].Type is Reference))
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