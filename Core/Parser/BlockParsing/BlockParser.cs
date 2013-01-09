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
            if (assignmentStatement.Source.NodeType == NodeType.Construct
                && assignmentStatement.Source.Type.NodeType == NodeType.DelegateNode)
            {
              Construct construct = (Construct)assignmentStatement.Source;
              UnaryExpression methodWrapper = (UnaryExpression) construct.Operands[1];
              MemberBinding methodBinding = (MemberBinding) methodWrapper.Operand;
              Method assignedMethod = (Method) methodBinding.BoundMember;
              
              DelegateNode sourceDelegate = (DelegateNode)assignmentStatement.Source.Type;
              string returnFragment = SymbolTable.EMPTY_FRAGMENT;
              foreach (Member member in sourceDelegate.Members)
              {
                if (member.Name.Name == "Invoke")
                {
                  Method invoke = (Method) member;
                  returnFragment = FragmentUtility.ReturnFragmentType(invoke);
                }
              }

              ISymbolTable environment = new SymbolTable(_blacklistManager);
              foreach (Parameter parameter in sourceDelegate.Parameters)
              {
                if (parameter.Attributes != null)
                {
                  environment.MakeSafe(parameter.Name.Name, FragmentUtility.GetFragmentType(parameter.Attributes));
                }
              }

              IMethodGraphAnalyzer methodParser = new MethodGraphAnalyzer(_problemPipe);
              IMethodGraphBuilder methodGraphBuilder = new MethodGraphBuilder(assignedMethod, _blacklistManager, _problemPipe, returnFragment);
              IInitialSymbolTableBuilder parameterSymbolTableBuilder = new  EmbeddedInitialSymbolTableBuilder(assignedMethod, _blacklistManager, environment);
              methodParser.Parse(methodGraphBuilder, parameterSymbolTableBuilder);
            }
            else
            {
              AssignmentStatementHandler(assignmentStatement);
            }
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
      if (!(assignmentStatement.Target is Indexer))
      {
        string targetSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Target);
        _assignmentTargetVariables.Add (targetSymbol);
        string sourceSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Source);
        bool localSourceVariableNotAssignedInsideCurrentBlock =
            sourceSymbol != null
            && !_assignmentTargetVariables.Contains (sourceSymbol)
            && !IntrospectionUtility.IsField (assignmentStatement.Source);
        bool targetIsField = IntrospectionUtility.IsField (assignmentStatement.Target);

        if (localSourceVariableNotAssignedInsideCurrentBlock)
        {
          if (targetIsField)
          {
            AddAssignmentPreCondition (assignmentStatement);
          }
          else
          {
            AddBlockAssignment (assignmentStatement);
          }
        }
        else
        {
          if (targetIsField)
          {
            ValidateAssignmentOnField (assignmentStatement);
          }
          else
          {
            _symbolTableParser.InferSafeness (targetSymbol, assignmentStatement.Source);
          }
        }
      }
      else
      {
        Indexer targetIndexer = (Indexer) assignmentStatement.Target;
        string targetName = IntrospectionUtility.GetVariableName (targetIndexer.Object);
        string targetFragmentType = _symbolTableParser.GetFragmentType (targetName);
        string sourceFragmentType = _symbolTableParser.InferFragmentType (assignmentStatement.Source);
        if (targetFragmentType != sourceFragmentType )
        {
          _symbolTableParser.MakeUnsafe (targetName);
        }
        else
        {
          if (targetName != null)
          {
            ProblemMetadata problemMetadata = new ProblemMetadata (
                assignmentStatement.UniqueKey,
                assignmentStatement.SourceContext,
                targetFragmentType,
                "??");
            var preCondition = new EqualityPreCondition (targetName, SymbolTable.EMPTY_FRAGMENT, problemMetadata);
            _preConditions.Add (preCondition);
          }
        }
      }
      Inspect (assignmentStatement.Source);
    }
    
    private void AddAssignmentPreCondition (AssignmentStatement assignmentStatement)
    {
      Field targetField = IntrospectionUtility.GetField (assignmentStatement.Target);
      string targetFragmentType = FragmentUtility.GetFragmentType (targetField.Attributes);
      if (targetFragmentType != SymbolTable.EMPTY_FRAGMENT)
      {
        ProblemMetadata problemMetadata = new ProblemMetadata (
            assignmentStatement.UniqueKey,
            assignmentStatement.SourceContext,
            targetFragmentType,
            "??");
        string sourceSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Source);
        if (sourceSymbol != null)
        {
          AssignabilityPreCondition preCondition = new AssignabilityPreCondition (sourceSymbol, targetFragmentType, problemMetadata);
          _preConditions.Add (preCondition);
        }
      }
    }

    private void AddBlockAssignment (AssignmentStatement assignmentStatement)
    {
      string targetSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Target);
      string sourceSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Source);
      if (targetSymbol != null && sourceSymbol != null)
      {
        BlockAssignment blockAssignment = new BlockAssignment (sourceSymbol, targetSymbol);
        _blockAssignments.Add (blockAssignment);
      }
    }

    private void ReturnStatementHandler (ReturnNode returnNode)
    {
      if (returnNode.Expression != null)
      {
        Inspect (returnNode.Expression);
        string returnSymbol = IntrospectionUtility.GetVariableName (returnNode.Expression);
        if (returnSymbol != null)
        {
          ProblemMetadata problemMetadata = new ProblemMetadata (
              returnNode.UniqueKey, returnNode.SourceContext, _returnFragmentType, _symbolTableParser.GetFragmentType (returnSymbol));
          AssignabilityPreCondition returnBlockCondition = new AssignabilityPreCondition (returnSymbol, _returnFragmentType, problemMetadata);
          _preConditions.Add (returnBlockCondition);
          _preConditions.AddRange (_returnConditions);
        }
      }
      else{
        foreach (var returnCondition in _returnConditions)
        {
          string blockInternalFragmentType = _symbolTableParser.GetFragmentType (returnCondition.Symbol);
          if (blockInternalFragmentType != SymbolTable.LITERAL
              && returnCondition.FragmentType != SymbolTable.EMPTY_FRAGMENT
              && returnCondition.FragmentType != blockInternalFragmentType)
          {
            ProblemMetadata problemMetadata = new ProblemMetadata (
            returnNode.UniqueKey,
            returnNode.SourceContext,
            returnCondition.FragmentType,
            blockInternalFragmentType);
            
            if(!_assignmentTargetVariables.Contains (returnCondition.Symbol))
            {
              _preConditions.Add (returnCondition);
            }
            else
            {
              _problemPipe.AddProblem (problemMetadata);
            }
          }
        }
      }
    }

    private void ValidateAssignmentOnField (AssignmentStatement assignmentStatement)
    {
      Field targetField = IntrospectionUtility.GetField (assignmentStatement.Target);
      string targetFragmentType = FragmentUtility.GetFragmentType (targetField.Attributes);
      string givenFragmentType;
      if (IntrospectionUtility.IsField (assignmentStatement.Source))
      {
        Field source = IntrospectionUtility.GetField (assignmentStatement.Source);
        givenFragmentType = FragmentUtility.GetFragmentType (source.Attributes);
      }
      else
      {
        givenFragmentType = _symbolTableParser.InferFragmentType (assignmentStatement.Source);
      }

      if (targetFragmentType != givenFragmentType && givenFragmentType != SymbolTable.LITERAL)
      {
        ProblemMetadata problemMetadata = new ProblemMetadata (
            targetField.UniqueKey,
            targetField.SourceContext,
            targetFragmentType,
            givenFragmentType);
        _problemPipe.AddProblem (problemMetadata);
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
      List<AssignabilityPreCondition> additionalPreConditions;
      List<ProblemMetadata> parameterProblems;
      _symbolTableParser.ParametersSafe (methodCall, out additionalPreConditions, out parameterProblems);
      parameterProblems.ForEach (parameterProblem => _problemPipe.AddProblem (parameterProblem));
      _preConditions.AddRange (additionalPreConditions);
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