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
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Parser.BlockParsing.PreCondition;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.MethodParsing
{
  public class MethodGraphBuilder : IMethodGraphBuilder
  {
    private IMethodGraph _result;
    private readonly Block _methodBody;
    private readonly IBlacklistManager _blacklistManager;
    private readonly IProblemPipe _problemPipe;
    private readonly string _returnFragmentType;
    private readonly List<ReturnCondition> _referenceAndOutConditions;

    public MethodGraphBuilder (Method method, IBlacklistManager blacklistManager, IProblemPipe problemPipe)
    {
      ArgumentUtility.CheckNotNull ("method", method);
      _blacklistManager = ArgumentUtility.CheckNotNull ("blacklistManager", blacklistManager);
      _problemPipe = ArgumentUtility.CheckNotNull ("problemPipe", problemPipe);
      bool isInterfaceMethod = method.Body.Statements == null;
      if (!isInterfaceMethod)
      {
        _methodBody = method.Body;
        _referenceAndOutConditions = ReferenceAndOutConditions (method);
        _result = null;
      }
      else
      {
        _result = new MethodGraph (-1, new Dictionary<int, BasicBlock>());
      }
      _returnFragmentType = FragmentUtility.ReturnFragmentType (method);
    }

    public MethodGraphBuilder(Method method, IBlacklistManager blacklistManager, IProblemPipe problemPipe, string returnFragmentType)
      : this(method, blacklistManager, problemPipe)
    {
      _returnFragmentType = returnFragmentType;
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
        BlockParser parser = new BlockParser (_blacklistManager, _problemPipe, _returnFragmentType, _referenceAndOutConditions);
        Dictionary<int, BasicBlock> graph = new Dictionary<int, BasicBlock>();
        int initialBlockId;
        List<Block> blockList = GetBlocks(_methodBody.Statements);

        using (var blocksEnumerator = blockList.GetEnumerator())
        {
          if (!blocksEnumerator.MoveNext())
            return;
          var currentBlock = blocksEnumerator.Current;
// ReSharper disable PossibleNullReferenceException
          initialBlockId = currentBlock.UniqueKey;
// ReSharper restore PossibleNullReferenceException
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
// ReSharper disable PossibleNullReferenceException
              currentBasicBlock = parser.Parse (currentBlock, nextBlock.UniqueKey);
// ReSharper restore PossibleNullReferenceException
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

    private List<Block> GetBlocks (StatementCollection statements)
    {
      List<Block> blocks = new List<Block>();
      foreach (var statement in statements)
      {
        if (statement is Block)
        {
          blocks.Add ((Block) statement);
        }
        else if (statement is TryNode)
        {
          TryNode tryNode = (TryNode) statement;
          blocks.AddRange(GetTryNodeBlocks (tryNode));
          blocks.AddRange (GetCatcherBlocks (tryNode.Catchers));
          if (tryNode.Finally != null)
          {
            blocks.AddRange (GetFinallyBlocks (tryNode.Finally));
          }
        }
      }
      return blocks;
    }
    
    private List<ReturnCondition> ReferenceAndOutConditions (Method method)
    {
      List<ReturnCondition> referenceAndOutConditions = new List<ReturnCondition>();
      foreach (var parameter in method.Parameters)
      {
        if (parameter.Type is Reference && parameter.Attributes != null)
        {
          string parameterFragmentType = FragmentUtility.GetFragmentType (parameter.Attributes);
          if (parameterFragmentType != SymbolTable.EMPTY_FRAGMENT)
          {
            string parameterName = IntrospectionUtility.GetVariableName (parameter);
            ReturnCondition returnCondition = new ReturnCondition (parameterName, parameterFragmentType);
            referenceAndOutConditions.Add (returnCondition);
          }
        }
      }
      return referenceAndOutConditions;
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

    private IEnumerable<Block> GetTryNodeBlocks (TryNode tryNode)
    {
      List<Block> blocks = new List<Block>();
      blocks.Add (tryNode.Block);
      List<Block> tryNodeBlocks = GetBlocks (tryNode.Block.Statements);
      blocks.AddRange (tryNodeBlocks);
      return blocks;
    }

    private IEnumerable<Block> GetCatcherBlocks (CatchNodeCollection catchers)
    {
      List<Block> blocks = new List<Block>();
      foreach (var catcher in catchers)
      {
        blocks.Add (catcher.Block);
        List<Block> catchNodeBlocks = GetBlocks (catcher.Block.Statements);
        blocks.AddRange (catchNodeBlocks);
      }
      return blocks;
    }

    private IEnumerable<Block> GetFinallyBlocks (FinallyNode finallyNode)
    {
      List<Block> blocks = new List<Block>();
      blocks.Add (finallyNode.Block);
      blocks.AddRange (GetBlocks (finallyNode.Block.Statements));
      return blocks;
    }
  }
}
