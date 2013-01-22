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
using System.Diagnostics;
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Parser.BlockParsing.PreCondition;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;
using System.Linq;

namespace InjectionCop.Parser.MethodParsing
{
  public class MethodGraphAnalyzer : IMethodGraphAnalyzer
  {
    private readonly IProblemPipe _problemPipe;

    private Dictionary<int,int> recursioncalls;

    public MethodGraphAnalyzer (IProblemPipe problemPipe)
    {
      _problemPipe = ArgumentUtility.CheckNotNull ("problemPipe", problemPipe);
    }

    public void Parse (IMethodGraphBuilder methodGraphBuilder, IInitialSymbolTableBuilder initialSymbolTableBuilder)
    {
      ArgumentUtility.CheckNotNull ("methodGraphBuilder", methodGraphBuilder);
      ArgumentUtility.CheckNotNull ("parameterSymbolTableBuilder", initialSymbolTableBuilder);

      var methodGraph = methodGraphBuilder.GetResult();
      var initialSymbolTable = initialSymbolTableBuilder.GetResult();

      //Debugger.Launch();

      if (methodGraph != null && !methodGraph.IsEmpty() && initialSymbolTable != null)
      {
        /*
        var analysisRequired =
            methodGraph.Blocks.Any (
                block =>
                block.PreConditions.Any (
                    preCondition => preCondition.FragmentType != SymbolTable.EMPTY_FRAGMENT && preCondition.FragmentType != SymbolTable.LITERAL));
        */
        var analysisRequired = true;
        if (!analysisRequired)
          return;

        recursioncalls = new Dictionary<int, int>();
        Parse (methodGraph, initialSymbolTable, methodGraph.InitialBlock, new Dictionary<int, int>());
      }
    }

    private void Parse (IMethodGraph methodGraph, ISymbolTable context, BasicBlock currentBlock, Dictionary<int, int> visits)
    {
      if (!recursioncalls.ContainsKey (currentBlock.Id))
      {
        recursioncalls[currentBlock.Id] = 1;
      }
      else
      {
        recursioncalls[currentBlock.Id]++;
      }
      UpdateVisits (currentBlock.Id, visits);
      bool loopIterationsExceeded = visits[currentBlock.Id] > 1;
      if(!loopIterationsExceeded)
      {
        CheckPreCoditions (currentBlock.PreConditions, context);
        ISymbolTable adjustedContext = UpdateContext (context, currentBlock.PostConditionSymbolTable, currentBlock.BlockAssignments);
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

    private void CheckPreCoditions (IPreCondition[] preconditions, ISymbolTable context)
    {
      foreach (IPreCondition precondition in preconditions)
      {
        if(precondition.IsViolated(context))
        {
          _problemPipe.AddProblem(precondition.ProblemMetadata);
        }
      }
    }

    private ISymbolTable UpdateContext (ISymbolTable context, ISymbolTable postConditionSymbolTable, BlockAssignment[] blockAssignments)
    {
      ISymbolTable adjustedContext = context.Copy();
      MergePostConditions (adjustedContext, postConditionSymbolTable);
      MergeBlockAssignments (context, adjustedContext, blockAssignments);
      return adjustedContext;
    }
    
    private void MergePostConditions (ISymbolTable adjustedContext, ISymbolTable postConditionSymbolTable)
    {
      foreach (string symbol in postConditionSymbolTable.Symbols)
      {
        adjustedContext.MakeSafe (symbol, postConditionSymbolTable.GetFragmentType (symbol));
      }
    }

    private void MergeBlockAssignments (ISymbolTable context, ISymbolTable adjustedContext, BlockAssignment[] blockAssignments)
    {
      foreach (var blockAssignment in blockAssignments)
      {
        Fragment propagatedFragmentType = context.GetFragmentType (blockAssignment.SourceSymbol);
        adjustedContext.MakeSafe (blockAssignment.TargetSymbol, propagatedFragmentType);
      }
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
