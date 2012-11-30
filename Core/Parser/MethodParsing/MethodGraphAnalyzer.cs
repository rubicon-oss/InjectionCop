// Copyright 2012 rubicon informationstechnologie gmbh
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
using InjectionCop.Parser.TypeParsing;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.MethodParsing
{
  public class MethodGraphAnalyzer : IMethodGraphAnalyzer
  {
    private readonly TypeParser _typeParser;

    public MethodGraphAnalyzer (TypeParser typeParser)
    {
      _typeParser = ArgumentUtility.CheckNotNull ("typeParser", typeParser);
    }

    public ProblemCollection Parse (IMethodGraphBuilder methodGraphBuilder, IParameterSymbolTableBuilder parameterSymbolTableBuilder)
    {
      ArgumentUtility.CheckNotNull ("methodGraphBuilder", methodGraphBuilder);
      ArgumentUtility.CheckNotNull ("parameterSymbolTableBuilder", parameterSymbolTableBuilder);

      IMethodGraph methodGraph = methodGraphBuilder.GetResult();
      ISymbolTable parameterSafeness = parameterSymbolTableBuilder.GetResult();

      if (!methodGraph.IsEmpty() && parameterSafeness != null)
      {
        Parse (methodGraph, parameterSafeness, methodGraph.InitialBlock, new Dictionary<int, int>());
      }

      return _typeParser.Problems;
    }

    private void Parse (IMethodGraph methodGraph, ISymbolTable context, BasicBlock currentBlock, Dictionary<int, int> visits)
    {
      UpdateVisits (currentBlock.Id, visits);
      if (visits[currentBlock.Id] < 2)
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
          _typeParser.AddProblem();
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
