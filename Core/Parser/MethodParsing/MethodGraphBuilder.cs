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
using System.Linq;
using InjectionCop.Config;
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Parser.TypeParsing;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.MethodParsing
{
  public class MethodGraphBuilder : IMethodGraphBuilder
  {
    private IMethodGraph _result;
    private readonly Block _methodBody;
    private readonly IBlacklistManager _blacklistManager;
    private readonly TypeParser _typeParser;

    public MethodGraphBuilder (Method method, IBlacklistManager blacklistManager, TypeParser typeParser)
    {
      ArgumentUtility.CheckNotNull ("method", method);
      _methodBody = method.Body;
      _blacklistManager = ArgumentUtility.CheckNotNull ("blacklistManager", blacklistManager);
      _typeParser = ArgumentUtility.CheckNotNull ("typeParser", typeParser);
      _result = null;
    }

    public IMethodGraph GetResult ()
    {
      Build();
      return _result;
    }

    public void Build ()
    {
      if (_result == null)
      {
        _result = MethodGraph.Empty;
        BlockParser parser = new BlockParser (_blacklistManager, _typeParser);
        Dictionary<int, BasicBlock> graph = new Dictionary<int, BasicBlock>();
        int initialBlockId;

        List<Block> blockList = new List<Block> (_methodBody.Statements.OfType<Block>());

        using (var blocksEnumerator = blockList.GetEnumerator())
        {
          if (!blocksEnumerator.MoveNext())
            return;
          var currentBlock = blocksEnumerator.Current;
          initialBlockId = currentBlock.UniqueKey;
          BasicBlock currentBasicBlock;
          while (blocksEnumerator.MoveNext())
          {
            var nextBlock = blocksEnumerator.Current;

            if (ContainsUnconditionalBranch (currentBlock))
            {
              currentBasicBlock = parser.Parse (currentBlock);
            }
            else
            {
              currentBasicBlock = parser.Parse (currentBlock, nextBlock.UniqueKey);
            }
            graph.Add (currentBasicBlock.Id, currentBasicBlock);

            currentBlock = nextBlock;
          }
          currentBasicBlock = parser.Parse (currentBlock);
          graph.Add (currentBasicBlock.Id, currentBasicBlock);
        }

        _result = new MethodGraph (initialBlockId, graph);
      }
    }

    private bool ContainsUnconditionalBranch (Block currentBlock)
    {
      bool containsUnconditionalBranch = false;
      foreach (Statement statement in currentBlock.Statements)
      {
        if (statement is Branch && ((Branch) statement).Condition == null)
        {
          containsUnconditionalBranch = true;
        }
      }
      return containsUnconditionalBranch;
    }
  }
}
