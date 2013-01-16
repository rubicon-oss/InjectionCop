﻿// Copyright 2013 rubicon informationstechnologie gmbh
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
using Microsoft.FxCop.Sdk;
using InjectionCop.Utilities;
using InjectionCop.Parser.ProblemPipe;


namespace InjectionCop.Parser.BlockParsing.StatementHandler
{
  public class AssignmentStatementHandler
  {
    public delegate void InspectCallback(Expression expression);

    private IProblemPipe _problemPipe;

    public AssignmentStatementHandler(IProblemPipe problemPipe)
    {
      _problemPipe = problemPipe;
    }

    public void Handle(AssignmentStatement assignmentStatement, ISymbolTable symbolTable, List<IPreCondition> preConditions, List<string> assignmentTargetVariables, InspectCallback inspect, List<BlockAssignment> blockAssignments)
    {
      if (!(assignmentStatement.Target is Indexer))
      {
        string targetSymbol = IntrospectionUtility.GetVariableName(assignmentStatement.Target);
        assignmentTargetVariables.Add(targetSymbol);
        string sourceSymbol = IntrospectionUtility.GetVariableName(assignmentStatement.Source);
        bool localSourceVariableNotAssignedInsideCurrentBlock =
            sourceSymbol != null
            && !assignmentTargetVariables.Contains(sourceSymbol)
            && !IntrospectionUtility.IsField(assignmentStatement.Source);
        bool targetIsField = IntrospectionUtility.IsField(assignmentStatement.Target);

        if (localSourceVariableNotAssignedInsideCurrentBlock)
        {
          if (targetIsField)
          {
            AddAssignmentPreCondition(assignmentStatement, preConditions);
          }
          else
          {
            AddBlockAssignment(assignmentStatement, blockAssignments);
          }
        }
        else
        {
          if (targetIsField)
          {
            ValidateAssignmentOnField(assignmentStatement, symbolTable);
          }
          else
          {
            symbolTable.InferSafeness(targetSymbol, assignmentStatement.Source);
          }
        }
      }
      else
      {
        Indexer targetIndexer = (Indexer)assignmentStatement.Target;
        string targetName = IntrospectionUtility.GetVariableName(targetIndexer.Object);
        string targetFragmentType = symbolTable.GetFragmentType(targetName);
        string sourceFragmentType = symbolTable.InferFragmentType(assignmentStatement.Source);
        if (targetFragmentType != sourceFragmentType)
        {
          symbolTable.MakeUnsafe(targetName);
        }
        else
        {
          if (targetName != null)
          {
            ProblemMetadata problemMetadata = new ProblemMetadata(
                assignmentStatement.UniqueKey,
                assignmentStatement.SourceContext,
                targetFragmentType,
                "??");
            var preCondition = new EqualityPreCondition(targetName, SymbolTable.EMPTY_FRAGMENT, problemMetadata);
            preConditions.Add(preCondition);
          }
        }
      }
      inspect(assignmentStatement.Source);
    }

    private void AddAssignmentPreCondition(AssignmentStatement assignmentStatement, List<IPreCondition> preConditions)
    {
      Field targetField = IntrospectionUtility.GetField(assignmentStatement.Target);
      string targetFragmentType = FragmentUtility.GetFragmentType(targetField.Attributes);
      if (targetFragmentType != SymbolTable.EMPTY_FRAGMENT)
      {
        ProblemMetadata problemMetadata = new ProblemMetadata(
            assignmentStatement.UniqueKey,
            assignmentStatement.SourceContext,
            targetFragmentType,
            "??");
        string sourceSymbol = IntrospectionUtility.GetVariableName(assignmentStatement.Source);
        if (sourceSymbol != null)
        {
          AssignabilityPreCondition preCondition = new AssignabilityPreCondition(sourceSymbol, targetFragmentType, problemMetadata);
          preConditions.Add(preCondition);
        }
      }
    }

    private void AddBlockAssignment(AssignmentStatement assignmentStatement, List<BlockAssignment> blockAssignments)
    {
      string targetSymbol = IntrospectionUtility.GetVariableName(assignmentStatement.Target);
      string sourceSymbol = IntrospectionUtility.GetVariableName(assignmentStatement.Source);
      if (targetSymbol != null && sourceSymbol != null)
      {
        BlockAssignment blockAssignment = new BlockAssignment(sourceSymbol, targetSymbol);
        blockAssignments.Add(blockAssignment);
      }
    }

    private void ValidateAssignmentOnField(AssignmentStatement assignmentStatement, ISymbolTable symbolTable)
    {
      Field targetField = IntrospectionUtility.GetField(assignmentStatement.Target);
      string targetFragmentType = FragmentUtility.GetFragmentType(targetField.Attributes);
      string givenFragmentType;
      if (IntrospectionUtility.IsField(assignmentStatement.Source))
      {
        Field source = IntrospectionUtility.GetField(assignmentStatement.Source);
        givenFragmentType = FragmentUtility.GetFragmentType(source.Attributes);
      }
      else
      {
        givenFragmentType = symbolTable.InferFragmentType(assignmentStatement.Source);
      }

      if (targetFragmentType != givenFragmentType && givenFragmentType != SymbolTable.LITERAL)
      {
        ProblemMetadata problemMetadata = new ProblemMetadata(
            targetField.UniqueKey,
            targetField.SourceContext,
            targetFragmentType,
            givenFragmentType);
        _problemPipe.AddProblem(problemMetadata);
      }
    }
  }
}