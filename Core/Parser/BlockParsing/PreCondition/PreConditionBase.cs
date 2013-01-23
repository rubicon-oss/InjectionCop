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

namespace InjectionCop.Parser.BlockParsing.PreCondition
{
  public abstract class PreConditionBase : IPreCondition
  {
    protected readonly string _symbol;
    protected readonly Fragment _fragment;
    protected ProblemMetadata _problemMetadata;

    protected PreConditionBase (string symbol, Fragment fragment)
    {
      ArgumentUtility.CheckNotNullOrEmpty ("symbol", symbol);
      ArgumentUtility.CheckNotNull ("fragment", fragment);
      
      _symbol = symbol;
      _fragment =  fragment;
    }

    protected PreConditionBase (string symbol, Fragment fragment, ProblemMetadata problemMetadata)
        : this (symbol, fragment)
    {
      _problemMetadata = ArgumentUtility.CheckNotNull ("sourceContext", problemMetadata);
    }

    protected abstract bool ViolationCheckStrategy (ISymbolTable context);

    public string Symbol
    {
      get { return _symbol; }
    }

    public Fragment Fragment
    {
      get { return _fragment; }
    }

    public ProblemMetadata ProblemMetadata
    {
      get { return _problemMetadata; }
      set { _problemMetadata = value; }
    }

    public bool IsViolated (ISymbolTable context)
    {
      bool preConditionViolated = ViolationCheckStrategy (context);
      if (preConditionViolated)
      {
        _problemMetadata.GivenFragment = context.GetFragmentType (_symbol);
      }
      return preConditionViolated;
    }
  }
}
