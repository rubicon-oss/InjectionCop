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
using InjectionCop.Config;
using InjectionCop.Parser.BlockParsing.PreCondition;
using Microsoft.FxCop.Sdk;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;

namespace InjectionCop.Parser.BlockParsing.StatementHandler
{
  public class ReturnStatementHandler : StatementHandlerBase<ReturnNode>
  {
    public ReturnStatementHandler (
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
      ReturnNode returnNode = (ReturnNode) context.Statement;
      bool memberHasReturnType = returnNode.Expression != null;
      if (memberHasReturnType)
      {
        HandleReturnType (returnNode, context);
      }
      else
      {
        HandleVoidReturn (returnNode, context);
      }
    }

    private void HandleReturnType (ReturnNode returnNode, HandleContext context)
    {
      _inspect (returnNode.Expression);
      string returnSymbol = IntrospectionUtility.GetVariableName (returnNode.Expression);
      if (returnSymbol != null)
      {
        ProblemMetadata problemMetadata = new ProblemMetadata (
            returnNode.UniqueKey, returnNode.SourceContext, _returnFragmentType, context.SymbolTable.GetFragmentType (returnSymbol));
        AssignabilityPreCondition returnBlockCondition = new AssignabilityPreCondition (returnSymbol, _returnFragmentType, problemMetadata);
        context.PreConditions.Add (returnBlockCondition);
        context.PreConditions.AddRange (_returnConditions);
      }
    }

    private void HandleVoidReturn (ReturnNode returnNode, HandleContext context)
    {
      foreach (var returnCondition in _returnConditions)
      {
        Fragment blockInternalFragmentType = context.SymbolTable.GetFragmentType (returnCondition.Symbol);

        if (!FragmentUtility.FragmentTypesAssignable (blockInternalFragmentType, returnCondition.Fragment))
        {
          ProcessBlockInternalPreConditionViolation (
              returnNode, returnCondition, blockInternalFragmentType, context.AssignmentTargetVariables, context.PreConditions);
        }
      }
    }

    private void ProcessBlockInternalPreConditionViolation (
        ReturnNode returnNode,
        ReturnCondition returnCondition,
        Fragment blockInternalFragmentType,
        List<string> assignmentTargetVariables,
        List<IPreCondition> preConditions)
    {
      ProblemMetadata problemMetadata = new ProblemMetadata (
          returnNode.UniqueKey,
          returnNode.SourceContext,
          returnCondition.Fragment,
          blockInternalFragmentType);
      returnCondition.ProblemMetadata = problemMetadata;

      if (!assignmentTargetVariables.Contains (returnCondition.Symbol))
      {
        preConditions.Add (returnCondition);
      }
      else
      {
        _problemPipe.AddProblem (problemMetadata);
      }
    }
  }
}
