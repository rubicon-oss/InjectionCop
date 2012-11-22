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
using InjectionCop.Parser._Block;
using InjectionCop.Parser._Type;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser._Method
{
  public class MethodParser : IMethodParser
  {
    private TypeParser _typeParser;
    
    public MethodParser (TypeParser typeParser)
    {
      _typeParser = typeParser;
    }

    public ProblemCollection Parse (IMethodParserDirector director)
    {
      SymbolTable parameterSafeness = director.GetParameterSafeness();
      IMethodGraph methodGraph = director.GetMethodGraph();
      
      if (!methodGraph.IsEmpty())
      {
        Parse (methodGraph, parameterSafeness, methodGraph.InitialBlock, new Dictionary<int, int>());
      }
      
      return _typeParser.Problems;
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
        CheckPreCoditions (currentBlock.PreConditions, context);
        SymbolTable adjustedContext = UpdateContext (context, currentBlock.PostConditionSymbolTable);
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

    private void CheckPreCoditions (PreCondition[] preconditions, SymbolTable context)
    {
      foreach (PreCondition precondition in preconditions)
      {
        if (!context.Contains (precondition.Symbol) || !context.IsSafe (precondition.Symbol, precondition.FragmentType))
        {
          _typeParser.AddProblem();
        }
      }
    }

    private SymbolTable UpdateContext (SymbolTable context, SymbolTable postConditionSymbolTable)
    {
      SymbolTable adjustedContext = context.Clone();
      foreach (string symbol in postConditionSymbolTable.Symbols)
      {
        adjustedContext.SetContextMap (symbol, postConditionSymbolTable.GetContextMap (symbol));
      }
      return adjustedContext;
    }

    private void ParseSuccessors (int[] successorKeys, Dictionary<int, int> visits, IMethodGraph methodGraph, SymbolTable adjustedContext)
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