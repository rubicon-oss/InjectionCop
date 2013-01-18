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
using InjectionCop.Parser.ProblemPipe;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.BlockParsing.StatementHandler.AssignmentStatementHandler
{
  public class AssignmentStatementHandlerController : StatementHandlerBase<AssignmentStatement>
  {
    private readonly DefaultAssignmentStatementHandler _defaultAssignmentStatementHandler;
    private readonly DelegateAssignmentStatementHandler _delegateAssignmentStatementHandler;
    private readonly IndexerAssignmentStatementHandler _indexerAssignmentStatementHandler;

    public AssignmentStatementHandlerController (
        IProblemPipe problemPipe,
        string returnFragmentType,
        List<ReturnCondition> returnConditions,
        IBlacklistManager blacklistManager,
        BlockParser.InspectCallback inspect)
        : base (problemPipe, returnFragmentType, returnConditions, blacklistManager, inspect)
    {
      _defaultAssignmentStatementHandler = new DefaultAssignmentStatementHandler (
          _problemPipe, _returnFragmentType, _returnConditions, _blacklistManager, inspect);
      _delegateAssignmentStatementHandler = new DelegateAssignmentStatementHandler (
          _problemPipe, _returnFragmentType, _returnConditions, _blacklistManager, inspect);
      _indexerAssignmentStatementHandler = new IndexerAssignmentStatementHandler (
          _problemPipe, _returnFragmentType, _returnConditions, _blacklistManager, inspect);
    }

    protected override void HandleStatement (
        Statement statement,
        ISymbolTable symbolTable,
        List<IPreCondition> preConditions,
        List<string> assignmentTargetVariables,
        List<BlockAssignment> blockAssignments,
        List<int> successors)
    {
      AssignmentStatement assignmentStatement = (AssignmentStatement) statement;
      bool sourceIsDelegate = assignmentStatement.Source.NodeType == NodeType.Construct
                              && assignmentStatement.Source.Type.NodeType == NodeType.DelegateNode;

      if (sourceIsDelegate)
      {
        _delegateAssignmentStatementHandler.Handle (statement, symbolTable, preConditions, assignmentTargetVariables, blockAssignments, successors);
      }
      else if (assignmentStatement.Target is Indexer)
      {
        _indexerAssignmentStatementHandler.Handle (statement, symbolTable, preConditions, assignmentTargetVariables, blockAssignments, successors);
      }
      else
      {
        _defaultAssignmentStatementHandler.Handle (statement, symbolTable, preConditions, assignmentTargetVariables, blockAssignments, successors);
      }
    }
  }
}
