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
using InjectionCop.Utilities;

namespace InjectionCop.Config
{
  /// <summary>
  /// Checks if methods are blacklisted, returns corresponding fragment types
  /// </summary>
  public class BlacklistManager : IBlacklistManager
  {
    private static readonly XNamespace c_ic = "eu.rubicon.injectioncop";
    private static readonly XName c_blacklist = c_ic + "Blacklist";
    private static readonly XName c_type = c_ic + "Type";
    private static readonly XName c_method = c_ic + "Method";
    private static readonly XName c_parameter = c_ic + "Parameter";
    private static readonly XName c_name = "name";
    private static readonly XName c_qualified_name = "qualifiedName";
    private static readonly XName c_qualified_parametertypename = "qualifiedParameterTypeName";
    private static readonly XName c_fragment_typte = "fragmentType";

    private readonly XElement _blacklist;

    public BlacklistManager (XElement blacklist)
    {
      _blacklist = ArgumentUtility.CheckNotNull ("blacklist", blacklist);
    }

    public bool IsListed (string qualifiedTypeName, string methodName, IList<string> qualifiedParameterTypes)
    {
      ArgumentUtility.CheckNotNullOrEmpty ("qualifiedTypeName", qualifiedTypeName);
      ArgumentUtility.CheckNotNullOrEmpty ("methodName", methodName);
      ArgumentUtility.CheckNotNull ("qualifiedParameterTypes", qualifiedParameterTypes);

      bool isListed = false;
      if (_blacklist.Name == c_blacklist)
      {
        IEnumerable<XElement> filteredMethods = FilterMethods (qualifiedTypeName, methodName, qualifiedParameterTypes);
        isListed = filteredMethods.Any();
      }
      return isListed;
    }

    public string[] GetFragmentTypes (string qualifiedTypeName, string methodName, IList<string> qualifiedParameterTypes)
    {
      ArgumentUtility.CheckNotNullOrEmpty ("qualifiedTypeName", qualifiedTypeName);
      ArgumentUtility.CheckNotNullOrEmpty ("methodName", methodName);
      ArgumentUtility.CheckNotNull ("qualifiedParameterTypes", qualifiedParameterTypes);

      IList<string> fragmentTypes = new List<string>();
      if (IsListed (qualifiedTypeName, methodName, qualifiedParameterTypes))
      {
        IEnumerable<XElement> filteredMethods = FilterMethods (qualifiedTypeName, methodName, qualifiedParameterTypes);
        var methodEnumerator = filteredMethods.GetEnumerator();
        if (methodEnumerator.MoveNext())
        {
          XElement method = methodEnumerator.Current;
          method.Elements (c_parameter).ToList().ForEach (parameter => fragmentTypes.Add (GetFragmentType (parameter)));
        }
      }
      return fragmentTypes.ToArray();
    }

    private IEnumerable<XElement> FilterMethods (string qualifiedTypeName, string methodName, IList<string> qualifiedParameterTypes)
    {
      return from type in FilterTypes (qualifiedTypeName)
             from method in type.Elements (c_method)
             where method.Attribute (c_name).Value == methodName
                   && ParametersMatch (method.Elements (c_parameter), qualifiedParameterTypes)
             select method;
    }

    private IEnumerable<XElement> FilterTypes (string qualifiedTypeName)
    {
      return from type in _blacklist.Elements (c_type)
             where type.Attribute (c_qualified_name).Value == qualifiedTypeName
             select type;
    }

    private bool ParametersMatch (IEnumerable<XElement> parameters, IList<string> qualifiedParameterTypes)
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

        if (parameter.Attribute (c_qualified_parametertypename).Value != type)
        {
          parametersMatch = false;
        }
      }
      return parametersMatch;
    }

    private string GetFragmentType (XElement parameter)
    {
      string fragmentType = SymbolTable.EMPTY_FRAGMENT;
      if (parameter.Attribute (c_fragment_typte) != null)
      {
        fragmentType = parameter.Attribute (c_fragment_typte).Value;
      }
      return fragmentType;
    }
  }
}
