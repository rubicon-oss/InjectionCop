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
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.BlockParsing
{
  public abstract class ABCPreCondition: IPreCondition
  {
    protected readonly string _symbol;
    protected readonly string _fragmentType;
    protected readonly ProblemMetadata _problemMetadata;

    protected ABCPreCondition (string symbol, string fragmentType)
      : this(symbol, fragmentType, new ProblemMetadata (-1, new SourceContext(), "?", "?"))
    {
    }

    protected ABCPreCondition (string symbol, string fragmentType, ProblemMetadata problemMetadata)
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

    public bool IsViolated(ISymbolTable context)
    {
      return CheckStrategy(context);
    }

    protected abstract bool CheckStrategy(ISymbolTable context);
  }
}
