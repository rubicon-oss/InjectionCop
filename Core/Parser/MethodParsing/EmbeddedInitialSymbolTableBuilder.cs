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
using Microsoft.FxCop.Sdk;
using InjectionCop.Config;
using InjectionCop.Utilities;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace InjectionCop.Parser.MethodParsing
{
  public class EmbeddedInitialSymbolTableBuilder: InitialSymbolTableBuilder
  {
    private bool _symbolTableBuilt;

    public EmbeddedInitialSymbolTableBuilder(Method method, IBlacklistManager blacklistManager, ISymbolTable environment)
      : base(method, blacklistManager)
    {
      ArgumentUtility.CheckNotNull("environment", environment);
      _result = environment;
      _symbolTableBuilt = false;
    }

    public override void Build()
    {
      if (!_symbolTableBuilt)
      {
        AnalyzeParameters();
        AnalyzeFields();
        _symbolTableBuilt = true;
      }
    }

    private void AnalyzeFields()
    {
      IEnumerable<Field> fields = IntrospectionUtility.FilterFields(_method.DeclaringType);
      foreach (Field field in fields)
      {
        if (field.Attributes != null && FragmentUtility.ContainsFragment(field.Attributes))
        {
          _result.MakeUnsafe(field.Name.Name);
        }
      }
    }


  }
}
