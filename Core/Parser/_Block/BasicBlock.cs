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

namespace InjectionCop.Parser._Block
{
  /// <summary>
  /// Represents a node in a method graph
  /// </summary>
  public class BasicBlock
  {
    private readonly int _id;
    private readonly PreCondition[] _preConditions;
    private readonly SymbolTable _postConditionSymbolTable;
    private readonly int[] _successorKeys;
    
    public BasicBlock(int id, PreCondition[] preConditions, SymbolTable postConditionSymbolTable, int[] successorKeys)
    {
      _preConditions = preConditions;
      _postConditionSymbolTable = postConditionSymbolTable;
      _successorKeys = successorKeys;
      _id = id;
    }

    public PreCondition[] PreConditions
    {
      get { return _preConditions; }
    }

    public SymbolTable PostConditionSymbolTable
    {
      get { return _postConditionSymbolTable; }
    }

    public int[] SuccessorKeys
    {
      get { return _successorKeys; }
    }

    public int Id
    {
      get { return _id; }
    }
  }
}
