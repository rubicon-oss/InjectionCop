﻿// Copyright 2013 rubicon informationstechnologie gmbh
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
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.BlockParsing
{
  class EqualityPreCondition: ABCPreCondition
  {
    public EqualityPreCondition(string symbol, string fragmentType)
      : this(symbol, fragmentType, new ProblemMetadata(-1, new SourceContext(), "?", "?"))
    {
    }

    public EqualityPreCondition(string symbol, string fragmentType, ProblemMetadata problemMetadata)
      : base (symbol, fragmentType, problemMetadata)
    {
    }

    protected override bool CheckStrategy(ISymbolTable context)
    {
      return !context.Contains(_symbol)
              || context.GetFragmentType(_symbol) != _fragmentType;
    }
  }
}