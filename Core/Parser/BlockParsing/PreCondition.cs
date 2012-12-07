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
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.BlockParsing
{
  /// <summary>
  /// Holds a symbol and the associated fragment type that needs to safe when entering a BasicBlock
  /// </summary>
  public class PreCondition
  {
    private readonly string _symbol;
    private readonly string _fragmentType;
    private readonly ProblemMetadata _problemMetadata;
    
    public PreCondition (string symbol, string fragmentType)
      : this(symbol, fragmentType, new ProblemMetadata (-1, new SourceContext(), "?", "?"))
    {
    }

    public PreCondition (string symbol, string fragmentType, ProblemMetadata problemMetadata)
    {
      _symbol = ArgumentUtility.CheckNotNullOrEmpty ("symbol", symbol);
      _fragmentType = ArgumentUtility.CheckNotNullOrEmpty ("fragmentType", fragmentType);
      _problemMetadata = ArgumentUtility.CheckNotNull("sourceContext", problemMetadata);
    }

    public string Symbol
    {
      get { return _symbol; }
    }

    public string FragmentType
    {
      get { return _fragmentType; }
    }

    public ProblemMetadata ProblemMetadata
    {
      get { return _problemMetadata; }
    }
  }
}
