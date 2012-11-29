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
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using InjectionCop.Parser;

namespace InjectionCop.Config
{
  public class BlacklistManager : IBlacklistManager
  {
    private static readonly XNamespace _ic = "eu.rubicon.injectioncop";
    private static readonly XName BLACKLIST = _ic + "Blacklist";
    private static readonly XName TYPE = _ic + "Type";
    private static readonly XName METHOD = _ic + "Method";
    private static readonly XName PARAMETER = _ic + "Parameter";
    private static readonly XName NAME = "name";
    private static readonly XName QUALIFIED_NAME = "qualifiedName";
    private static readonly XName QUALIFIED_PARAMETERTYPENAME = "qualifiedParameterTypeName";
    private static readonly XName FRAGMENT_TYPE = "fragmentType";

    private XElement _blacklist;

    public BlacklistManager (XElement blacklist)
    {
      _blacklist = blacklist;
    }

    public bool IsListed (string qualifiedTypeName, string methodName, List<string> qualifiedParameterTypes)
    {
      bool isListed = false;
      if (_blacklist.Name == BLACKLIST)
      {
        IEnumerable<XElement> filteredMethods = FilterMethods (qualifiedTypeName, methodName, qualifiedParameterTypes);
        isListed = filteredMethods.Any();
      }
      return isListed;
    }

    private IEnumerable<XElement> FilterMethods (string qualifiedTypeName, string methodName, List<string> qualifiedParameterTypes)
    {
      return from type in FilterTypes (qualifiedTypeName)
             from method in type.Elements (METHOD)
             where method.Attribute (NAME).Value == methodName
                   && ParametersMatch (method.Elements (PARAMETER), qualifiedParameterTypes)
             select method;
    }

    private IEnumerable<XElement> FilterTypes (string qualifiedTypeName)
    {
      return from type in _blacklist.Elements (TYPE)
             where type.Attribute (QUALIFIED_NAME).Value == qualifiedTypeName
             select type;
    }

    private bool ParametersMatch (IEnumerable<XElement> parameters, List<string> qualifiedParameterTypes)
    {
      bool parametersMatch = true;
      var parametersEnumerator = parameters.GetEnumerator();
      var typesEnumerator = qualifiedParameterTypes.GetEnumerator();
      while (parametersEnumerator.MoveNext())
      {
        if (!typesEnumerator.MoveNext())
        {
          parametersMatch = false;
        }

        var parameter = parametersEnumerator.Current;
        var type = typesEnumerator.Current;

        if (parameter.Attribute (QUALIFIED_PARAMETERTYPENAME).Value != type)
        {
          parametersMatch = false;
        }
      }
      return parametersMatch;
    }

    public List<string> GetFragmentTypes (string qualifiedName, string methodName, List<string> qualifiedParameterTypes)
    {
      List<string> fragmentTypes = new List<string>();
      if (IsListed (qualifiedName, methodName, qualifiedParameterTypes))
      {
        IEnumerable<XElement> filteredMethods = FilterMethods (qualifiedName, methodName, qualifiedParameterTypes);
        var methodEnumerator = filteredMethods.GetEnumerator();
        if (methodEnumerator.MoveNext())
        {
          XElement method = methodEnumerator.Current;
          method.Elements (PARAMETER).ToList().ForEach (parameter => fragmentTypes.Add(GetFragmentType (parameter)));
        }
      }
      return fragmentTypes;
    }

    private string GetFragmentType (XElement parameter)
    {
      string fragmentType = SymbolTable.EMPTY_FRAGMENT;
      if (parameter.Attribute (FRAGMENT_TYPE) != null)
      {
        fragmentType = parameter.Attribute (FRAGMENT_TYPE).Value;
      }
      return fragmentType;
    }
  }
}
