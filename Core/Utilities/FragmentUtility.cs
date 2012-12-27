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
using System.Linq;
using InjectionCop.Fragment;
using InjectionCop.Parser;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Utilities
{
  /// <summary>
  /// Helper methods for handling fragments
  /// </summary>
  public class FragmentUtility
  {
    public static bool IsFragment (AttributeNode attribute)
    {
      ArgumentUtility.CheckNotNull ("attribute", attribute);
      string fragmentFullName = typeof (FragmentAttribute).FullName;
      bool isFragment = attribute.Type.FullName == fragmentFullName;
      bool isFragmentChild = attribute.Type.BaseType.FullName == fragmentFullName;
      return isFragment || isFragmentChild;
    }

    public static bool ContainsFragment (AttributeNodeCollection attributes)
    {
      ArgumentUtility.CheckNotNull ("attributes", attributes);
      return attributes.Any (IsFragment);
    }

    public static string GetFragmentType (AttributeNodeCollection attributes)
    {
      //ArgumentUtility.CheckNotNull ("attributes", attributes);
      string fragmentType = SymbolTable.EMPTY_FRAGMENT;
      if (attributes != null && ContainsFragment (attributes))
      {
        string fragmentFullName = typeof (FragmentAttribute).FullName;

        foreach (AttributeNode attributeNode in attributes)
        {
          if (IsFragment (attributeNode))
          {
            if (attributeNode.Type.FullName == fragmentFullName)
            {
              foreach (Literal literal in attributeNode.Expressions)
              {
                string value = literal.Value as string;
                if (value != null)
                {
                  fragmentType = value;
                }
              }
            }
            else
            {
              fragmentType = attributeNode.Type.Name.Name.Replace ("Attribute", "");
            }
          }
        }
      }
      return fragmentType;
    }

    public static string ReturnFragmentType (Method method)
    {
      ArgumentUtility.CheckNotNull ("method", method);
      string returnFragment = SymbolTable.EMPTY_FRAGMENT;
      Method determiningMethod;

      Method[] interfaceDeclarations = IntrospectionUtility.InterfaceDeclarations (method);
      if (interfaceDeclarations.Any())
      {
        determiningMethod = interfaceDeclarations.First();
      }
      else
      {
        determiningMethod = method;
      }
      
      if (determiningMethod.ReturnAttributes != null && ContainsFragment (determiningMethod.ReturnAttributes))
      {
        returnFragment = GetFragmentType (determiningMethod.ReturnAttributes);
      }
      return returnFragment;
    }
  }
}
