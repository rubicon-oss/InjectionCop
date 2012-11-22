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

namespace InjectionCop.Parser
{
  public class MethodGraph: IMethodGraph
  {
    private static MethodGraph _emptyGraph = new MethodGraph (-1, new Dictionary<int, BasicBlock>());
    
    private readonly int _initialBlockId;
    private readonly Dictionary<int, BasicBlock> _graph;

    public MethodGraph (int initialBlockId, Dictionary<int, BasicBlock> graph)
    {
      _initialBlockId = initialBlockId;
      _graph = graph;
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

    public static MethodGraph Empty
    {
      get { return _emptyGraph; }
    }
  }
}
