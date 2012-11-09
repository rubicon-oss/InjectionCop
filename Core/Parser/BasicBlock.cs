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

namespace InjectionCop.Parser
{
  public class BasicBlock
  {
    private readonly string[] _preConditionSafeSymbols;
    private readonly SymbolTable _postConditionSymbolTable;
    private readonly int[] _successorKeys;
    
    public BasicBlock(string[] preConditionSafeSymbols, SymbolTable postConditionSymbolTable, int[] successorKeys)
    {
      _preConditionSafeSymbols = preConditionSafeSymbols;
      _postConditionSymbolTable = postConditionSymbolTable;
      _successorKeys = successorKeys;
    }

    /// <summary>
    /// symbols that must be set to a safe value upon entering the basic block
    /// </summary>
    public List<string> PreConditionSafeSymbols
    {
      get { return new List<string> (_preConditionSafeSymbols); }
    }

    /// <summary>
    /// 
    /// </summary>
    public SymbolTable PostConditionSymbolTable
    {
      get { return _postConditionSymbolTable; }
    }

    public int[] SuccessorKeys
    {
      get { return _successorKeys; }
    }
  }
}
