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
using System.Diagnostics;
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Utilities;

namespace InjectionCop.Parser.MethodParsing
{
  public class MethodGraph : IMethodGraph
  {
    private readonly int _initialBlockId;
    private readonly Dictionary<int, BasicBlock> _blocks;

    public MethodGraph (int initialBlockId, Dictionary<int, BasicBlock> graph)
    {
      _initialBlockId = initialBlockId;
      _blocks = ArgumentUtility.CheckNotNull ("graph", graph);
    }

    public IEnumerable<BasicBlock> Blocks
    {
      get { return _blocks.Values; }
    }

    public BasicBlock GetBasicBlockById (int uniqueKey)
    {
      try
      {
        return _blocks[uniqueKey];
      }
      catch (KeyNotFoundException ex)
      {
        //Debugger.Launch();
        throw new InjectionCopException ("The given key was not present in the MethodGraph", ex);
      }
    }

    public bool IsEmpty ()
    {
      return _blocks.Keys.Count == 0;
    }

    public BasicBlock InitialBlock
    {
      get
      {
        if (!IsEmpty())
        {
          return GetBasicBlockById (_initialBlockId);
        }
        else
        {
          throw new InjectionCopException ("Graph is empty");
        }
      }
    }
  }
}
