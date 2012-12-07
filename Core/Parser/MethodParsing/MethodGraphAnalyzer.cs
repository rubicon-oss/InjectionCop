﻿// Copyright 2012 rubicon informationstechnologie gmbh
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
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;

namespace InjectionCop.Parser.MethodParsing
{
  public class MethodGraphAnalyzer : IMethodGraphAnalyzer
  {
    private readonly IProblemPipe _problemPipe;

    public MethodGraphAnalyzer (IProblemPipe problemPipe)
    {
      _problemPipe = ArgumentUtility.CheckNotNull ("problemPipe", problemPipe);
    }

    public void Parse (IMethodGraphBuilder methodGraphBuilder, IInitialSymbolTableBuilder initialSymbolTableBuilder)
    {
      ArgumentUtility.CheckNotNull ("methodGraphBuilder", methodGraphBuilder);
      ArgumentUtility.CheckNotNull ("parameterSymbolTableBuilder", initialSymbolTableBuilder);

      IMethodGraph methodGraph = methodGraphBuilder.GetResult();
      ISymbolTable initialSymbolTable = initialSymbolTableBuilder.GetResult();

      if (!methodGraph.IsEmpty() && initialSymbolTable != null)
      {
        Parse (methodGraph, initialSymbolTable, methodGraph.InitialBlock, new Dictionary<int, int>());
      }
    }

    private void Parse (IMethodGraph methodGraph, ISymbolTable context, BasicBlock currentBlock, Dictionary<int, int> visits)
    {
      UpdateVisits (currentBlock.Id, visits);
      bool loopIterationsExceeded = visits[currentBlock.Id] > 1;
      if(!loopIterationsExceeded)
      {
        CheckPreCoditions (currentBlock.PreConditions, context);
        ISymbolTable adjustedContext = UpdateContext (context, currentBlock.PostConditionSymbolTable);
        ParseSuccessors (currentBlock.SuccessorKeys, visits, methodGraph, adjustedContext);
      }
    }

    private void UpdateVisits (int id, Dictionary<int, int> visits)
    {
      if (!visits.ContainsKey (id))
      {
        visits[id] = 0;
      }
      else
      {
        visits[id] = visits[id] + 1;
      }
    }

    private void CheckPreCoditions (PreCondition[] preconditions, ISymbolTable context)
    {
      foreach (PreCondition precondition in preconditions)
      {
        if (!context.Contains (precondition.Symbol) || !context.IsFragment (precondition.Symbol, precondition.FragmentType))
        {
          _problemPipe.AddProblem(precondition.ProblemMetadata);
        }
      }
    }

    private ISymbolTable UpdateContext (ISymbolTable context, ISymbolTable postConditionSymbolTable)
    {
      ISymbolTable adjustedContext = context.Copy();
      foreach (string symbol in postConditionSymbolTable.Symbols)
      {
        adjustedContext.MakeSafe (symbol, postConditionSymbolTable.GetFragmentType (symbol));
      }
      return adjustedContext;
    }

    private void ParseSuccessors (int[] successorKeys, Dictionary<int, int> visits, IMethodGraph methodGraph, ISymbolTable adjustedContext)
    {
      foreach (int successorKey in successorKeys)
      {
        Dictionary<int, int> branchVisits = new Dictionary<int, int> (visits);
        BasicBlock successor = methodGraph.GetBasicBlockById (successorKey);
        Parse (methodGraph, adjustedContext, successor, branchVisits);
      }
    }
  }
}
