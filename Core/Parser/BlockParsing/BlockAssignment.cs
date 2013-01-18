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
  /// Models the special case when a local variable is copied into another local variable. Such assignments need to be tracked because Fragment types need to be passed in that scenario.
  /// </summary>
  public class BlockAssignment
  {
    private readonly string _sourceSymbol;
    private readonly string _targetSymbol;

    public BlockAssignment (string sourceSymbol, string targetSymbol)
    {
      _sourceSymbol = ArgumentUtility.CheckNotNullOrEmpty ("sourceSymbol", sourceSymbol);
      _targetSymbol = ArgumentUtility.CheckNotNullOrEmpty ("targetSymbol", targetSymbol);
    }

    public string SourceSymbol
    {
      get { return _sourceSymbol; }
    }

    public string TargetSymbol
    {
      get { return _targetSymbol; }
    }
  }
}
