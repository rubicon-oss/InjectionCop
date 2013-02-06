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
using InjectionCop.Parser.BlockParsing.PreCondition;
using InjectionCop.Parser.ProblemPipe;
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
    public delegate void InspectCallback (Expression expression);

    private readonly IBlacklistManager _blacklistManager;
    private readonly IProblemPipe _problemPipe;
    private readonly Fragment _returnFragmentType;
    private readonly List<ReturnCondition> _returnConditions;
    private readonly Dictionary<Type, IStatementHandler> _statementHandlers;
    private readonly MethodCallAnalyzer _methodCallAnalyzer;

    private ISymbolTable _symbolTableParser;
    private List<IPreCondition> _preConditions;
    private List<int> _successors;
    private List<BlockAssignment> _blockAssignments;
    private List<string> _assignmentTargetVariables;
    private Dictionary<string, bool> _locallyInitializedArrays;
    private Dictionary<string, bool> _stringBuilderFragmentTypesDefined;

    public BlockParser (
        IBlacklistManager blacklistManager, IProblemPipe problemPipe, Fragment returnFragmentType, List<ReturnCondition> returnConditions)
    {
      _blacklistManager = ArgumentUtility.CheckNotNull ("blacklistManager", blacklistManager);
      _problemPipe = ArgumentUtility.CheckNotNull ("typeParser", problemPipe);
      _returnFragmentType = returnFragmentType;
      _returnConditions = returnConditions;
      BlockParserContext blockParserContext = new BlockParserContext (
          _problemPipe, _returnFragmentType, _returnConditions, _blacklistManager, Inspect
          );
      StatementHandlerDictionaryBuilder handlerBuilder = new StatementHandlerDictionaryBuilder (blockParserContext);
      _statementHandlers = handlerBuilder.Build();
      _methodCallAnalyzer = new MethodCallAnalyzer (_problemPipe);
    }

    public BasicBlock Parse (Block block)
    {
      ArgumentUtility.CheckNotNull ("block", block);
      Reset();
      Inspect (block);
      BasicBlock basicBlock = new BasicBlock (
          block.UniqueKey, _preConditions.ToArray(), _symbolTableParser, _successors.ToArray(), _blockAssignments.ToArray());
      return basicBlock;
    }

    public BasicBlock Parse (Block block, int directSuccessorKey)
    {
      ArgumentUtility.CheckNotNull ("block", block);
      Reset();
      _successors.Add (directSuccessorKey);
      Inspect (block);
      BasicBlock basicBlock = new BasicBlock (
          block.UniqueKey, _preConditions.ToArray(), _symbolTableParser, _successors.ToArray(), _blockAssignments.ToArray());
      return basicBlock;
    }

    private void Reset ()
    {
      _symbolTableParser = new SymbolTable (_blacklistManager);
      _preConditions = new List<IPreCondition>();
      _successors = new List<int>();
      _blockAssignments = new List<BlockAssignment>();
      _assignmentTargetVariables = new List<string>();
      _locallyInitializedArrays = new Dictionary<string, bool>();
      _stringBuilderFragmentTypesDefined = new Dictionary<string, bool>();
    }

    private void Inspect (Block methodBodyBlock)
    {
      foreach (Statement statement in methodBodyBlock.Statements)
      {
        if (_statementHandlers.ContainsKey (statement.GetType()))
        {
          HandleContext context = new HandleContext (
              statement,
              _symbolTableParser,
              _preConditions,
              _assignmentTargetVariables,
              _blockAssignments,
              _successors,
              _locallyInitializedArrays,
              _stringBuilderFragmentTypesDefined);

          _statementHandlers[statement.GetType()].Handle (context);
        }
      }
    }

    private void Inspect (Expression expression)
    {
      if (expression is MethodCall)
      {
        var methodCall = (MethodCall) expression;
        Inspect (methodCall);
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

    private void Inspect (MethodCall methodCall)
    {
      _methodCallAnalyzer.Analyze (methodCall, _symbolTableParser, _preConditions);
      foreach (Expression operand in methodCall.Operands)
      {
        Inspect (operand);
      }
    }
  }
}