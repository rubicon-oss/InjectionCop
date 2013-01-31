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
using InjectionCop.Parser.BlockParsing.PreCondition;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.BlockParsing.StatementHandler.AssignmentStatementHandler
{
  public class IndexerAssignmentStatementHandler : StatementHandlerBase<AssignmentStatement>
  {
    public IndexerAssignmentStatementHandler (BlockParserContext blockParserContext)
        : base (blockParserContext)
    {
    }

    protected override void HandleStatement (HandleContext context)
    {
      AssignmentStatement assignmentStatement = (AssignmentStatement) context.Statement;
      Indexer targetIndexer = (Indexer) assignmentStatement.Target;
      string targetName = IntrospectionUtility.GetVariableName (targetIndexer.Object);
      
      if (context.ArrayFragmentTypeDefined.ContainsKey (targetName))
      {
        InferArrayFragment(assignmentStatement, targetName, context);
      }
      else
      {
        CheckAssignment(assignmentStatement, targetName, context); 
      }

      _blockParserContext.Inspect (assignmentStatement.Source);
    }

    private void InferArrayFragment (AssignmentStatement assignmentStatement, string targetName, HandleContext context)
    {
      ISymbolTable symbolTable = context.SymbolTable;
      Fragment targetFragmentType = symbolTable.InferFragmentType (assignmentStatement.Source);
      if (context.ArrayFragmentTypeDefined[targetName] == false)
      {
        symbolTable.MakeSafe(targetName, targetFragmentType);
        context.ArrayFragmentTypeDefined[targetName] = true;
      }
      else if (symbolTable.GetFragmentType(targetName) == Fragment.CreateLiteral())
      {
        symbolTable.MakeSafe(targetName, targetFragmentType);
      }
      else if (symbolTable.GetFragmentType(targetName) != targetFragmentType && targetFragmentType != Fragment.CreateLiteral())
      {
        symbolTable.MakeUnsafe(targetName);
      }
    }

    private void CheckAssignment (AssignmentStatement assignmentStatement, string targetName, HandleContext context)
    {
      ISymbolTable symbolTable = context.SymbolTable;
      Fragment targetFragmentType = symbolTable.GetFragmentType (targetName);
      Fragment sourceFragmentType = symbolTable.InferFragmentType (assignmentStatement.Source);

      if (targetFragmentType != sourceFragmentType)
        {
          symbolTable.MakeUnsafe (targetName);
        }
        else
        {
          SetPreConditionForIndexerObject (assignmentStatement, targetName, sourceFragmentType, context);
        }
    }

    private void SetPreConditionForIndexerObject (
        AssignmentStatement assignmentStatement, string targetName, Fragment sourceFragmentType, HandleContext context)
    {
      if (targetName != null)
      {
        ProblemMetadata problemMetadata = new ProblemMetadata (
            assignmentStatement.UniqueKey,
            assignmentStatement.SourceContext,
            sourceFragmentType,
            Fragment.CreateNamed ("??"));

        var preCondition = new EqualityPreCondition (targetName, sourceFragmentType, problemMetadata);
        context.PreConditions.Add (preCondition);
      }
    }
  }
}
