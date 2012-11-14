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
using InjectionCop.Config;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  public class MethodGraph: IMethodGraph
  {
    private readonly int _initialBlockId;
    private readonly Dictionary<int, BasicBlock> _graph;

    public MethodGraph (Block methodBody, IBlackTypes blackTypes)
    {
      BlockParser parser = new BlockParser(blackTypes);
      _graph = new Dictionary<int, BasicBlock>();

      List<Block> blockList = new List<Block>();
      foreach (Statement statement in methodBody.Statements)
      {
        Block block = statement as Block;
        if(block != null)
        {
          blockList.Add (block);
        }
      }

      Block[] blocks = blockList.ToArray();
      if (blocks.Length >= 1)
      {
        Block currentBlock = blocks[0];
        _initialBlockId = currentBlock.UniqueKey;
        BasicBlock currentBasicBlock;

        for (int i = 1; i < blocks.Length; i++)
        {
          Block nextBlock = blocks[i];
          if(ContainsUnconditionalBranch(currentBlock))
          {
            currentBasicBlock = parser.Parse (currentBlock);
          }
          else
          {
            currentBasicBlock = parser.Parse (currentBlock, nextBlock.UniqueKey);
          }
          _graph.Add (currentBasicBlock.Id, currentBasicBlock);
          currentBlock = nextBlock;
        }

        currentBasicBlock = parser.Parse (currentBlock);
        _graph.Add (currentBasicBlock.Id, currentBasicBlock);
      }
    }

    private bool ContainsUnconditionalBranch (Block currentBlock)
    {
      bool containsUnconditionalBranch = false;
      foreach (Statement statement in currentBlock.Statements)
      {
        if(statement is Branch && ((Branch)statement).Condition == null)
        {
          containsUnconditionalBranch = true;
        }
      }
      return containsUnconditionalBranch;
    }

    public int InitialBlockId
    {
      get
      {
        if (!IsEmpty())
        {
          return _initialBlockId;
        }
        else
        {
          throw new InjectionCopException ("Graph is empty");
        }
      }
    }

    public BasicBlock GetBasicBlockById (int uniqueKey)
    {
      try
      {
        return _graph[uniqueKey];
      }
      catch (KeyNotFoundException ex)
      {
        throw new InjectionCopException("The given key was not present in the MethodGraph", ex);
      }
    }

    public bool IsEmpty()
    {
      return _graph.Keys.Count == 0;
    }
  }
}
