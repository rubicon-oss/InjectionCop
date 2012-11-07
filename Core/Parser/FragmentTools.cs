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
using InjectionCop.Attributes;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  public class FragmentTools
  {
    public static bool Is<F> (Expression expression)
        where F : FragmentAttribute
    {
      bool isFragment = false;

      Parameter parameter = expression as Parameter;
      if (parameter != null)
      {
        isFragment = Contains<F> (parameter.Attributes);
      }

      return isFragment;
    }

    public static bool Returns<F> (Expression expression)
        where F : FragmentAttribute
    {
      bool returnsFragment = false;

      MethodCall methodCall = expression as MethodCall;
      if (methodCall != null)
      {
        Method calleeMethod = IntrospectionTools.ExtractMethod (methodCall);
        returnsFragment = Contains<F> (calleeMethod.ReturnAttributes);
      }

      return returnsFragment;
    }

    public static bool Contains<F> (AttributeNodeCollection attributes)
        where F : FragmentAttribute
    {
      bool containsFragment;

      try
      {
        TypeNode fragmentTypeNode = Helper.TypeNodeFactory<F>();
        containsFragment = attributes.Any (
            attribute =>
            attribute.Type.FullName == fragmentTypeNode.FullName
            );
      }
      catch (ArgumentNullException)
      {
        containsFragment = false;
      }

      return containsFragment;
    }

    public static bool Contains (FragmentAttribute fragmentType, AttributeNodeCollection attributes)
    {
      bool containsFragment = false;

      try
      {
        //Identifier fieldName = Identifier.For("_fragmentType");
        foreach(AttributeNode attribute in attributes)
        {
          if(attribute.Type.FullName == "InjectionCop.Attributes.FragmentAttribute")
          {
            foreach (Literal literal in attribute.Expressions)
            {
              string value = literal.Value as string;
              if (FragmentAttribute.OfType(value) == fragmentType)
              {
                containsFragment = true;
              }

            }
          }
        }
        /*containsFragment = attributes.Any (
            attribute =>
            attribute.
            );*/
      }
      catch (ArgumentNullException)
      {
        containsFragment = false;
      }

      return containsFragment;
    }
  }
}
