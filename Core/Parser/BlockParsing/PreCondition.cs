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
using InjectionCop.Utilities;

namespace InjectionCop.Parser.BlockParsing
{
  /// <summary>
  /// Holds a symbol and the associated fragment type that needs to safe when entering a BasicBlock
  /// </summary>
  public class PreCondition
  {
    private readonly string _symbol;
    private readonly string _fragmentType;

    public PreCondition (string symbol, string fragmentType)
    {
      _symbol = ArgumentUtility.CheckNotNullOrEmpty ("symbol", symbol);
      _fragmentType = ArgumentUtility.CheckNotNullOrEmpty ("fragmentType", fragmentType);
    }

    public string Symbol
    {
      get { return _symbol; }
    }

    public string FragmentType
    {
      get { return _fragmentType; }
    }
  }
}
