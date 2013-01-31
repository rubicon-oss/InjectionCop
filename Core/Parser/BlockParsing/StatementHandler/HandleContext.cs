// Copyright 2013 rubicon informationstechnologie gmbh
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
using InjectionCop.Parser.BlockParsing.PreCondition;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.BlockParsing.StatementHandler
{
  public class HandleContext
  {
    private readonly Statement _statement;
    private readonly ISymbolTable _symbolTable;
    private readonly List<IPreCondition> _preConditions;
    private readonly List<string> _assignmentTargetVariables;
    private readonly List<BlockAssignment> _blockAssignments;
    private readonly List<int> _successors;
    private readonly Dictionary<string, bool> _arrayFragmentTypeDefined;

    public HandleContext (
        Statement statement,
        ISymbolTable symbolTable,
        List<IPreCondition> preConditions,
        List<string> assignmentTargetVariables,
        List<BlockAssignment> blockAssignments,
        List<int> successors,
        Dictionary<string, bool> arrayFragmentTypeDefined)
    {
      _statement = ArgumentUtility.CheckNotNull ("statement", statement);
      _symbolTable = ArgumentUtility.CheckNotNull ("symbolTable", symbolTable);
      _preConditions = ArgumentUtility.CheckNotNull ("preConditions", preConditions);
      _assignmentTargetVariables = ArgumentUtility.CheckNotNull ("assignmentTargetVariables", assignmentTargetVariables);
      _blockAssignments = ArgumentUtility.CheckNotNull ("blockAssignments", blockAssignments);
      _successors = ArgumentUtility.CheckNotNull ("successors", successors);
      _arrayFragmentTypeDefined = ArgumentUtility.CheckNotNull ("arrayFragmentTypeDefined", arrayFragmentTypeDefined);
    }

    public Statement Statement
    {
      get { return _statement; }
    }

    public ISymbolTable SymbolTable
    {
      get { return _symbolTable; }
    }

    public List<IPreCondition> PreConditions
    {
      get { return _preConditions; }
    }

    public List<BlockAssignment> BlockAssignments
    {
      get { return _blockAssignments; }
    }

    public List<string> AssignmentTargetVariables
    {
      get { return _assignmentTargetVariables; }
    }

    public List<int> Successors
    {
      get { return _successors; }
    }

    public Dictionary<string, bool> ArrayFragmentTypeDefined
    {
      get { return _arrayFragmentTypeDefined; }
    }
  }
}