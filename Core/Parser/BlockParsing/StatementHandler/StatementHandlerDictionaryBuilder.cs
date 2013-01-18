// Copyright 2013 rubicon informationstechnologie gmbh
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
using InjectionCop.Parser.BlockParsing.StatementHandler.AssignmentStatementHandler;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;

namespace InjectionCop.Parser.BlockParsing.StatementHandler
{
  public class StatementHandlerDictionaryBuilder
  {
    private readonly IBlacklistManager _blacklistManager;
    private readonly IProblemPipe _problemPipe;
    private readonly string _returnFragmentType;
    private readonly List<ReturnCondition> _returnConditions;
    private readonly BlockParser.InspectCallback _inspect;

    public StatementHandlerDictionaryBuilder (
        IBlacklistManager blacklistManager,
        IProblemPipe problemPipe,
        string returnFragmentType,
        List<ReturnCondition> returnConditions,
        BlockParser.InspectCallback inspect)
    {
      _blacklistManager = ArgumentUtility.CheckNotNull ("blacklistManager", blacklistManager);
      _problemPipe = ArgumentUtility.CheckNotNull ("typeParser", problemPipe);
      _returnFragmentType = ArgumentUtility.CheckNotNullOrEmpty ("returnFragmentType", returnFragmentType);
      _returnConditions = returnConditions;
      _inspect = inspect;
    }

    public Dictionary<Type, IStatementHandler> Build ()
    {
      Dictionary<Type, IStatementHandler> statementHandlers = new Dictionary<Type, IStatementHandler>();

      ReturnStatementHandler returnStatementHandler = new ReturnStatementHandler (
          _problemPipe, _returnFragmentType, _returnConditions, _blacklistManager, _inspect);
      AssignmentStatementHandlerController assignmentStatementHandler = new AssignmentStatementHandlerController (
          _problemPipe, _returnFragmentType, _returnConditions, _blacklistManager, _inspect);
      ExpressionStatementHandler expressionStatementHandler = new ExpressionStatementHandler (
          _problemPipe, _returnFragmentType, _returnConditions, _blacklistManager, _inspect);
      BranchStatementHandler branchStatementHandler = new BranchStatementHandler (
          _problemPipe, _returnFragmentType, _returnConditions, _blacklistManager, _inspect);
      SwitchStatementHandler switchStatementHandler = new SwitchStatementHandler (
          _problemPipe, _returnFragmentType, _returnConditions, _blacklistManager, _inspect);

      statementHandlers[returnStatementHandler.HandledStatementType] = returnStatementHandler;
      statementHandlers[assignmentStatementHandler.HandledStatementType] = assignmentStatementHandler;
      statementHandlers[expressionStatementHandler.HandledStatementType] = expressionStatementHandler;
      statementHandlers[branchStatementHandler.HandledStatementType] = branchStatementHandler;
      statementHandlers[switchStatementHandler.HandledStatementType] = switchStatementHandler;

      return statementHandlers;
    }
  }
}
