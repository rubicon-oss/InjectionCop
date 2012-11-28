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
  public class BlacklistManager
  {
    private static readonly XNamespace _ic = "eu.rubicon.injectioncop";
    private XElement _blacklist;

    public static XNamespace IC
    {
      get { return _ic; }
    }

    public BlacklistManager (XElement blacklist)
    {
      _blacklist = blacklist;
    }
    
    public bool IsListed (string qualifiedName, string methodName, List<string> qualifiedParameterTypes)
    {
      IEnumerable<XElement> filteredMethods = FilterMethods (qualifiedName, methodName, qualifiedParameterTypes);
      return filteredMethods.Any();
    }

    private IEnumerable<XElement> FilterMethods (string filteredTypes, string methodName, List<string> qualifiedParameterTypes)
    {
      return from type in FilterTypes(filteredTypes)
             from method in type.Elements (IC + "Method")
             where method.Attribute ("name").Value == methodName
                   && ParametersMatch (method.Elements (IC + "Parameter"), qualifiedParameterTypes)
             select method;
    }

    private IEnumerable<XElement> FilterTypes (string qualifiedName)
    {
      return from type in _blacklist.Elements (IC + "Type") 
             where type.Attribute ("qualifiedName").Value == qualifiedName 
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

        if (parameter.Attribute ("qualifiedParameterTypeName").Value != type)
        {
          parametersMatch = false;
        }
      }
      return parametersMatch;
    }

    public Dictionary<string, string> GetFragmentTypes (string qualifiedName, string methodName, List<string> qualifiedParameterTypes)
    {
      Dictionary<string, string> fragmentTypes = new Dictionary<string, string>();
      if (IsListed (qualifiedName, methodName, qualifiedParameterTypes))
      {
        IEnumerable<XElement> filteredMethods = FilterMethods (qualifiedName, methodName, qualifiedParameterTypes);
        var methodEnumerator = filteredMethods.GetEnumerator();
        if (methodEnumerator.MoveNext())
        {
          XElement method = methodEnumerator.Current;
          method.Elements (IC + "Parameter").ToList().ForEach (parameter => fragmentTypes[parameter.Value] = GetFragmentType (parameter));
        }
      }
      return fragmentTypes;
    }

    private string GetFragmentType (XElement parameter)
    {
      string fragmentType = SymbolTable.EmptyFragment;
      if (parameter.Attribute ("fragmentType") != null)
      {
        fragmentType = parameter.Attribute ("fragmentType").Value;
      }
      return fragmentType;
    }
  }
}
