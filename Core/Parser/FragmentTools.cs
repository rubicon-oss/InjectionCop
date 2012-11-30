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
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  /// <summary>
  /// Helper methods for handling fragments
  /// </summary>
  public class FragmentTools
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
      ArgumentUtility.CheckNotNull ("attributes", attributes);
      string fragmentType = SymbolTable.EMPTY_FRAGMENT;
      if (ContainsFragment (attributes))
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
  }
}
