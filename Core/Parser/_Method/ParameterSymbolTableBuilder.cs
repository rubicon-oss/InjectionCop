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

namespace InjectionCop.Parser._Method
{
  public class ParameterSymbolTableBuilder : IParameterSymbolTableBuilder
  {
    private SymbolTable _result;
    private Method _method;
    private IBlackTypes _blackTypes;

    public ParameterSymbolTableBuilder (Method method, IBlackTypes blackTypes)
    {
      _method = method;
      _blackTypes = blackTypes;
    }

    public void Build ()
    {
      SymbolTable parameterSafeness = new SymbolTable (_blackTypes);
      foreach (Parameter parameter in _method.Parameters)
      {
        if (FragmentTools.ContainsFragment (parameter.Attributes))
        {
          string fragmentType = FragmentTools.GetFragmentType (parameter.Attributes);
          parameterSafeness.SetSafeness (parameter.Name.Name, fragmentType, true);
        }
        else
        {
          parameterSafeness.MakeUnsafe (parameter.Name.Name);
        }
      }
      _result = parameterSafeness;
    }

    public SymbolTable GetResult ()
    {
      return _result;
    }
  }
}
