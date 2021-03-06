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
using InjectionCop.Parser.BlockParsing.PreCondition;
using InjectionCop.Utilities;

namespace InjectionCop.Parser.BlockParsing
{
  /// <summary>
  /// Represents a node in a method graph
  /// </summary>
  public class BasicBlock
  {
    private readonly int _id;
    private readonly IPreCondition[] _preConditions;
    private readonly ISymbolTable _postConditionSymbolTable;
    private readonly int[] _successorKeys;
    private readonly BlockAssignment[] _blockAssignments;

    public BasicBlock (
        int id, IPreCondition[] preConditions, ISymbolTable postConditionSymbolTable, int[] successorKeys, BlockAssignment[] blockAssignments)
    {
      _preConditions = ArgumentUtility.CheckNotNull ("preConditions", preConditions);
      _postConditionSymbolTable = ArgumentUtility.CheckNotNull ("postConditionSymbolTable", postConditionSymbolTable);
      _successorKeys = ArgumentUtility.CheckNotNull ("successorKeys", successorKeys);
      _id = id;
      _blockAssignments = blockAssignments;
    }

    public IPreCondition[] PreConditions
    {
      get { return _preConditions; }
    }

    public ISymbolTable PostConditionSymbolTable
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

    public BlockAssignment[] BlockAssignments
    {
      get { return _blockAssignments; }
    }
  }
}
