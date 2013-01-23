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
using System.Collections.Generic;
using InjectionCop.Attributes;
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

    public static Fragment GetFragmentType (AttributeNodeCollection attributes)
    {
      var fragmentType = Fragment.CreateEmpty();
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
                  fragmentType = Fragment.CreateNamed(value);
                }
              }
            }
            else
            {
              fragmentType = Fragment.CreateNamed (attributeNode.Type.Name.Name.Replace ("Attribute", ""));
            }
          }
        }
      }
      return fragmentType;
    }

    public static Fragment ReturnFragmentType (Method method)
    {
      ArgumentUtility.CheckNotNull ("method", method);
      var returnFragment = Fragment.CreateEmpty();
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

    public static Fragment[] GetAnnotatedParameterFragmentTypes(Method method)
    {
      var buffer = new List<Fragment>();
      if (!IsAnnotatedPropertySetter(method))
      {
        buffer.AddRange(method.Parameters.Select(parameter => FragmentUtility.GetFragmentType(parameter.Attributes)));
      }
      else
      {
        buffer.Add(FragmentUtility.GetFragmentType(method.DeclaringMember.Attributes));
      }
      return buffer.ToArray();
    }

    public static bool IsAnnotatedPropertySetter(Method method)
    {
      bool isAnnotatedPropertySetter = false;
      if (IntrospectionUtility.IsPropertySetter(method))
      {
        isAnnotatedPropertySetter = FragmentUtility.ContainsFragment(method.DeclaringMember.Attributes);
      }
      return isAnnotatedPropertySetter;
    }

    public static bool IsAnnotatedPropertyGetter(Method method)
    {
      bool isAnnotatedPropertyGetter = false;
      if (IntrospectionUtility.IsPropertyGetter(method))
      {
        isAnnotatedPropertyGetter = FragmentUtility.ContainsFragment(method.DeclaringMember.Attributes);
      }
      return isAnnotatedPropertyGetter;
    }

    public static Fragment InferReturnFragmentType(Method method)
    {
      Fragment fragmentType;
      AttributeNodeCollection definingAttributes = GetDefiningAttributes(method);

      if (definingAttributes != null)
      {
        fragmentType = FragmentUtility.GetFragmentType(definingAttributes);
      }
      else
      {
        fragmentType = Fragment.CreateEmpty();
      }

      return fragmentType;
    }

    public static Fragment GetMemberBindingFragmentType(MemberBinding memberBinding)
    {
      var fragmentType = Fragment.CreateEmpty();

      if (memberBinding.BoundMember is Field)
      {
        Field field = (Field)memberBinding.BoundMember;
        fragmentType = FragmentUtility.GetFragmentType(field.Attributes);
      }
      return fragmentType;
    }

    private static AttributeNodeCollection GetDefiningAttributes(Method method)
    {
      AttributeNodeCollection definingAttributes;
      Method[] interfaceDeclarations = IntrospectionUtility.InterfaceDeclarations(method);

      if (interfaceDeclarations.Any())
      {
        definingAttributes = interfaceDeclarations.First().ReturnAttributes;
      }
      else if (FragmentUtility.IsAnnotatedPropertyGetter(method))
      {
        definingAttributes = method.DeclaringMember.Attributes;
      }
      else
      {
        definingAttributes = method.ReturnAttributes;
      }

      return definingAttributes;
    }

    public static bool IsFragmentGenerator(Method method)
    {
      ArgumentUtility.CheckNotNull("method", method);
      bool isFragmentGenerator = ContainsFragmentGeneratorAttribute(method.Attributes);
      return isFragmentGenerator;
    }

    public static bool FragmentTypesAssignable (Fragment givenFragmentType, Fragment targetFragmentType)
    {
      ArgumentUtility.CheckNotNull ("givenFragmentType", givenFragmentType);
      ArgumentUtility.CheckNotNull ("targetFragmentType", targetFragmentType);

      return targetFragmentType == givenFragmentType
             || targetFragmentType == Fragment.CreateEmpty()
             || givenFragmentType == Fragment.CreateLiteral();
    }

    private static bool ContainsFragmentGeneratorAttribute(AttributeNodeCollection attributes)
    {
      bool containsFragmentGeneratorAttribute = false;
      if (attributes != null)
      {
        containsFragmentGeneratorAttribute = attributes.Any(IsFragmentGeneratorAttribute);
      }
      return containsFragmentGeneratorAttribute;
    }

    private static bool IsFragmentGeneratorAttribute(AttributeNode attribute)
    {
      return attribute.Type.FullName == typeof(FragmentGeneratorAttribute).FullName
              || attribute.Type.BaseType.FullName == typeof(FragmentGeneratorAttribute).FullName;
    }
  }
}
