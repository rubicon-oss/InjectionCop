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
using InjectionCop.Attributes;
using InjectionCop.Config;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  public class MethodParser
  {
    private IBlackTypes _blackTypes;
    private TypeParser _typeParser;
    
    public MethodParser (IBlackTypes blackTypes, TypeParser typeParser)
    {
      _blackTypes = blackTypes;
      _typeParser = typeParser;
    }

    public ProblemCollection Parse (Method method)
    {
      SymbolTable parameterSafeness = ExtractSafeParameters (method);
      MethodGraph methodGraph = new MethodGraph(method.Body, _blackTypes, _typeParser);
      return Parse (methodGraph, parameterSafeness);
    }

    private SymbolTable ExtractSafeParameters (Method method)
    {
      SymbolTable parameterSafeness = new SymbolTable (_blackTypes);
      FragmentAttribute sqlFragment = new FragmentAttribute ("SqlFragment");

      foreach (Parameter parameter in method.Parameters)
      {
        if (FragmentTools.Is (sqlFragment, parameter))
        {
          parameterSafeness.SetSafeness (parameter.Name, sqlFragment.FragmentType, true);
        }
        else
        {
          parameterSafeness.SetSafeness (parameter.Name, sqlFragment.FragmentType, false);
        }
      }
      return parameterSafeness;
    }

    public ProblemCollection Parse(IMethodGraph methodGraph, SymbolTable context)
    {
      ProblemCollection problems = _typeParser.Problems;
      
      if (!methodGraph.IsEmpty())
      {
        Parse (methodGraph, context, methodGraph.InitialBlock, new Dictionary<int, int>());
      }

      return problems;
    }

    private void Parse (IMethodGraph methodGraph, SymbolTable context, BasicBlock currentBlock, Dictionary<int,int> visits)
    {
      UpdateVisits (currentBlock.Id, visits);
      if (visits[currentBlock.Id] < 2)
      {
        CheckPreCoditions (currentBlock.PreConditionSafeSymbols, context);
        SymbolTable adjustedContext = UpdateContext (context, currentBlock.PostConditionSymbolTable);

        foreach (int successorKey in currentBlock.SuccessorKeys)
        {
          Dictionary<int, int> branchVisits = new Dictionary<int, int> (visits);
          BasicBlock successor = methodGraph.GetBasicBlockById (successorKey);
          Parse (methodGraph, adjustedContext, successor, branchVisits);
        }
      }
    }

    private SymbolTable UpdateContext (SymbolTable context, SymbolTable postConditionSymbolTable)
    {
      SymbolTable adjustedContext = context.Clone();
      foreach (string symbol in postConditionSymbolTable.Symbols)
      {
        bool safeness = postConditionSymbolTable.IsSafe (symbol, "SqlFragment");
        adjustedContext.SetSafeness (symbol, "SqlFragment", safeness);
      }
      return adjustedContext;
    }

    private void CheckPreCoditions (List<string> preconditions, SymbolTable context)
    {
      foreach (string precondition in preconditions)
      {
        if (!context.Contains (precondition) || !context.IsSafe (precondition, "SqlFragment"))
        {
          _typeParser.AddProblem();
        }
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
  }
}
