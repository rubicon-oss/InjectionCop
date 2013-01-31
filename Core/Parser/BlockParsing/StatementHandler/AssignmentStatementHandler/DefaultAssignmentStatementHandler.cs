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
using Microsoft.FxCop.Sdk;
using InjectionCop.Utilities;
using InjectionCop.Parser.ProblemPipe;

namespace InjectionCop.Parser.BlockParsing.StatementHandler.AssignmentStatementHandler
{
  public class DefaultAssignmentStatementHandler : StatementHandlerBase<AssignmentStatement>
  {
    public DefaultAssignmentStatementHandler (
        IProblemPipe problemPipe,
        Fragment returnFragmentType,
        List<ReturnCondition> returnConditions,
        IBlacklistManager blacklistManager,
        BlockParser.InspectCallback inspect)
        : base (problemPipe, returnFragmentType, returnConditions, blacklistManager, inspect)
    {
    }

    protected override void HandleStatement (HandleContext context)
    {
      AssignmentStatement assignmentStatement = (AssignmentStatement) context.Statement;

      string targetSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Target);
      context.AssignmentTargetVariables.Add (targetSymbol);
      string sourceSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Source);
      bool localSourceVariableNotAssignedInsideCurrentBlock =
          sourceSymbol != null
          && !context.AssignmentTargetVariables.Contains (sourceSymbol)
          && !IntrospectionUtility.IsField (assignmentStatement.Source);
      bool targetIsField = IntrospectionUtility.IsField (assignmentStatement.Target);

      if (localSourceVariableNotAssignedInsideCurrentBlock)
      {
        if (targetIsField)
        {
          AddAssignmentPreCondition (assignmentStatement, context);
        }
        else
        {
          AddBlockAssignment (assignmentStatement, context);
        }
      }
      else
      {
        if (targetIsField)
        {
          ValidateAssignmentOnField (assignmentStatement, context);
        }
        else
        {
          context.SymbolTable.InferSafeness (targetSymbol, assignmentStatement.Source);
        }
      }
      _inspect (assignmentStatement.Source);
    }

    private void AddAssignmentPreCondition (AssignmentStatement assignmentStatement, HandleContext context)
    {
      Field targetField = IntrospectionUtility.GetField (assignmentStatement.Target);
      Fragment targetFragmentType = FragmentUtility.GetFragmentType (targetField.Attributes);
      if (targetFragmentType != Fragment.CreateEmpty())
      {
        ProblemMetadata problemMetadata = new ProblemMetadata (
            assignmentStatement.UniqueKey,
            assignmentStatement.SourceContext,
            targetFragmentType,
            Fragment.CreateNamed ("??"));
        string sourceSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Source);
        if (sourceSymbol != null)
        {
          AssignabilityPreCondition preCondition = new AssignabilityPreCondition (sourceSymbol, targetFragmentType, problemMetadata);
          context.PreConditions.Add (preCondition);
        }
      }
    }

    private void AddBlockAssignment (AssignmentStatement assignmentStatement, HandleContext context)
    {
      string targetSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Target);
      string sourceSymbol = IntrospectionUtility.GetVariableName (assignmentStatement.Source);
      if (targetSymbol != null && sourceSymbol != null)
      {
        BlockAssignment blockAssignment = new BlockAssignment (sourceSymbol, targetSymbol);
        context.BlockAssignments.Add (blockAssignment);
      }
    }

    private void ValidateAssignmentOnField (AssignmentStatement assignmentStatement, HandleContext context)
    {
      Field targetField = IntrospectionUtility.GetField (assignmentStatement.Target);
      Fragment targetFragmentType = FragmentUtility.GetFragmentType (targetField.Attributes);
      Fragment givenFragmentType = context.SymbolTable.InferFragmentType (assignmentStatement.Source);

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
