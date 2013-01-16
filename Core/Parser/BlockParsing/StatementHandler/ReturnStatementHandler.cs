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
using Microsoft.FxCop.Sdk;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;

namespace InjectionCop.Parser.BlockParsing.StatementHandler
{
  public class ReturnStatementHandler: StatementHandlerBase<ReturnNode>
  {
    public ReturnStatementHandler (
        IProblemPipe problemPipe,
        string returnFragmentType,
        List<ReturnCondition> returnConditions,
        IBlacklistManager blacklistManager,
        InspectCallback inspect)
        : base (problemPipe, returnFragmentType, returnConditions, blacklistManager, inspect)
    {
    }

    protected override void HandleStatement(Statement statement, ISymbolTable symbolTable, List<IPreCondition> preConditions, List<string> assignmentTargetVariables, List<BlockAssignment> blockAssignments, List<int> successors)
    {
      ReturnNode returnNode = (ReturnNode) statement;
      if (returnNode.Expression != null)
      {
        _inspect(returnNode.Expression);
        string returnSymbol = IntrospectionUtility.GetVariableName(returnNode.Expression);
        if (returnSymbol != null)
        {
          ProblemMetadata problemMetadata = new ProblemMetadata(
              returnNode.UniqueKey, returnNode.SourceContext, _returnFragmentType, symbolTable.GetFragmentType(returnSymbol));
          AssignabilityPreCondition returnBlockCondition = new AssignabilityPreCondition(returnSymbol, _returnFragmentType, problemMetadata);
          preConditions.Add(returnBlockCondition);
          preConditions.AddRange(_returnConditions);
        }
      }
      else
      {
        foreach (var returnCondition in _returnConditions)
        {
          string blockInternalFragmentType = symbolTable.GetFragmentType(returnCondition.Symbol);
          if (blockInternalFragmentType != SymbolTable.LITERAL
              && returnCondition.FragmentType != SymbolTable.EMPTY_FRAGMENT
              && returnCondition.FragmentType != blockInternalFragmentType)
          {
            ProblemMetadata problemMetadata = new ProblemMetadata(
            returnNode.UniqueKey,
            returnNode.SourceContext,
            returnCondition.FragmentType,
            blockInternalFragmentType);
            returnCondition.ProblemMetadata = problemMetadata;

            if (!assignmentTargetVariables.Contains(returnCondition.Symbol))
            {
              preConditions.Add(returnCondition);
            }
            else
            {
              _problemPipe.AddProblem(problemMetadata);
            }
          }
        }
      }
    }
  }
}
