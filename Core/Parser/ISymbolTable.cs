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
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Parser.ProblemPipe;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  /// <summary>
  /// Keeps state (safe/unsafe) of a set of symbols.
  /// Defines method to infer state of a given expression.
  /// Defines method to check safeness of a method call
  /// </summary>
  public interface ISymbolTable
  {
    IEnumerable<string> Symbols { get; }
    Fragment[] InferParameterFragmentTypes(Method method);
    void InferSafeness (string symbolName, Expression expression);
    void MakeSafe (string symbolName, Fragment fragmentType);
    void MakeUnsafe (string symbolName);
    Fragment GetFragmentType (string symbolName);
    bool IsAssignableTo(string symbolName, Fragment fragmentType);
    Fragment InferFragmentType (Expression expression);
    bool Contains (string symbolName);
    ISymbolTable Copy ();
  }
}
