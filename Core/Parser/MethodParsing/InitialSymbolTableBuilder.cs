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
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.MethodParsing
{
  public class InitialSymbolTableBuilder : IInitialSymbolTableBuilder
  {
    private ISymbolTable _result;
    private readonly Method _method;
    private readonly IBlacklistManager _blacklistManager;

    public InitialSymbolTableBuilder (Method method, IBlacklistManager blacklistManager)
    {
      _method = ArgumentUtility.CheckNotNull ("method", method);
      _blacklistManager = ArgumentUtility.CheckNotNull ("blacklistManager", blacklistManager);
      _result = null;
    }

    public ISymbolTable GetResult ()
    {
      Build();
      return _result;
    }

    public void Build ()
    {
      if (_result == null)
      {
        _result = new SymbolTable (_blacklistManager);
        AnalyzeParameters();
        AnalyzeFields();
      }
    }

    private void AnalyzeParameters ()
    {
      foreach (var parameter in _method.Parameters)
      {
        SetSymbolFragmentType (parameter.Name.Name, parameter.Attributes);
      }
    }

    private void AnalyzeFields ()
    {
      AnalyzeFields (_method.DeclaringType);
    }

    private void AnalyzeFields (TypeNode type)
    {
      if (type != null)
      {
        foreach (var member in type.Members)
        {
          if (member is Field)
          {
            var field = (Field) member;
            SetSymbolFragmentType (field.Name.Name, field.Attributes);
          }
        }
        AnalyzeFields (type.BaseType);
      }
    }

    private void SetSymbolFragmentType (string name, AttributeNodeCollection attributes)
    {
      if (!_result.Contains (name))
      {
        if (FragmentUtility.ContainsFragment (attributes))
        {
          string fragmentType = FragmentUtility.GetFragmentType (attributes);
          _result.MakeSafe (name, fragmentType);
        }
        else
        {
          _result.MakeUnsafe (name);
        }
      }
    }
  }
}
