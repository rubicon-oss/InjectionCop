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
using InjectionCop.Config;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.MethodParsing
{
  public class ParameterSymbolTableBuilder : IParameterSymbolTableBuilder
  {
    private ISymbolTable _result;
    private readonly Method _method;
    private readonly IBlacklistManager _blacklistManager;

    public ParameterSymbolTableBuilder (Method method, IBlacklistManager blacklistManager)
    {
      _method = method;
      _blacklistManager = blacklistManager;
      _result = null;
    }

    public void Build ()
    {
      if (_result == null)
      {
        ISymbolTable parameterSafeness = new SymbolTable (_blacklistManager);
        foreach (Parameter parameter in _method.Parameters)
        {
          if (FragmentTools.ContainsFragment (parameter.Attributes))
          {
            string fragmentType = FragmentTools.GetFragmentType (parameter.Attributes);
            parameterSafeness.MakeSafe (parameter.Name.Name, fragmentType);
          }
          else
          {
            parameterSafeness.MakeUnsafe (parameter.Name.Name);
          }
        }
        _result = parameterSafeness;
      }
    }

    public ISymbolTable GetResult ()
    {
      Build();
      return _result;
    }
  }
}
