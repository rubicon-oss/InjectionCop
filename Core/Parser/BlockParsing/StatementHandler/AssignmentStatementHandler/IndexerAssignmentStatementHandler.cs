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
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.BlockParsing.StatementHandler.AssignmentStatementHandler
{
  public class IndexerAssignmentStatementHandler : StatementHandlerBase<AssignmentStatement>
  {
    public IndexerAssignmentStatementHandler (
        IProblemPipe problemPipe,
        string returnFragmentType,
        List<ReturnCondition> returnConditions,
        IBlacklistManager blacklistManager,
        BlockParser.InspectCallback inspect)
        : base (problemPipe, returnFragmentType, returnConditions, blacklistManager, inspect)
    {
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
      Indexer targetIndexer = (Indexer) assignmentStatement.Target;
      string targetName = IntrospectionUtility.GetVariableName (targetIndexer.Object);
      string targetFragmentType = symbolTable.GetFragmentType (targetName);
      string sourceFragmentType = symbolTable.InferFragmentType (assignmentStatement.Source);
      if (targetFragmentType != sourceFragmentType)
      {
        symbolTable.MakeUnsafe (targetName);
      }
      else
      {
        SetPreConditionForIndexerObject (assignmentStatement, targetName, sourceFragmentType, preConditions);
      }
      _inspect (assignmentStatement.Source);
    }

    private void SetPreConditionForIndexerObject (
        AssignmentStatement assignmentStatement, string targetName, string sourceFragmentType, List<IPreCondition> preConditions)
    {
      if (targetName != null)
      {
        ProblemMetadata problemMetadata = new ProblemMetadata (
            assignmentStatement.UniqueKey,
            assignmentStatement.SourceContext,
            sourceFragmentType,
            "??");
        
        var preCondition = new EqualityPreCondition (targetName, sourceFragmentType, problemMetadata);
        preConditions.Add (preCondition);
      }
    }
  }
}
