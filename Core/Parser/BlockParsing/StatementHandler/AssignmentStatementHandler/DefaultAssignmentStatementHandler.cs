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
using InjectionCop.Utilities;
using InjectionCop.Parser.ProblemPipe;

namespace InjectionCop.Parser.BlockParsing.StatementHandler.AssignmentStatementHandler
{
  public class DefaultAssignmentStatementHandler : StatementHandlerBase<AssignmentStatement>
  {
    public DefaultAssignmentStatementHandler (
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

      string targetSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Target);
      assignmentTargetVariables.Add (targetSymbol);
      string sourceSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Source);
      bool localSourceVariableNotAssignedInsideCurrentBlock =
          sourceSymbol != null
          && !assignmentTargetVariables.Contains (sourceSymbol)
          && !IntrospectionUtility.IsField (assignmentStatement.Source);
      bool targetIsField = IntrospectionUtility.IsField (assignmentStatement.Target);

      if (localSourceVariableNotAssignedInsideCurrentBlock)
      {
        if (targetIsField)
        {
          AddAssignmentPreCondition (assignmentStatement, preConditions);
        }
        else
        {
          AddBlockAssignment (assignmentStatement, blockAssignments);
        }
      }
      else
      {
        if (targetIsField)
        {
          ValidateAssignmentOnField (assignmentStatement, symbolTable);
        }
        else
        {
          symbolTable.InferSafeness (targetSymbol, assignmentStatement.Source);
        }
      }
      _inspect (assignmentStatement.Source);
    }

    private void AddAssignmentPreCondition (AssignmentStatement assignmentStatement, List<IPreCondition> preConditions)
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
          preConditions.Add (preCondition);
        }
      }
    }

    private void AddBlockAssignment (AssignmentStatement assignmentStatement, List<BlockAssignment> blockAssignments)
    {
      string targetSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Target);
      string sourceSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Source);
      if (targetSymbol != null && sourceSymbol != null)
      {
        BlockAssignment blockAssignment = new BlockAssignment (sourceSymbol, targetSymbol);
        blockAssignments.Add (blockAssignment);
      }
    }

    private void ValidateAssignmentOnField (AssignmentStatement assignmentStatement, ISymbolTable symbolTable)
    {
      Field targetField = IntrospectionUtility.GetField (assignmentStatement.Target);
      string targetFragmentType = FragmentUtility.GetFragmentType (targetField.Attributes);
      string givenFragmentType = symbolTable.InferFragmentType (assignmentStatement.Source);

      if (!FragmentUtility.FragmentTypesAssignable (givenFragmentType, targetFragmentType))
      {
        ProblemMetadata problemMetadata = new ProblemMetadata (
            targetField.UniqueKey,
            targetField.SourceContext,
            targetFragmentType,
            givenFragmentType);
        _problemPipe.AddProblem (problemMetadata);
      }
    }
  }
}
