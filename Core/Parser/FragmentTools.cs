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
    public static bool Is(FragmentAttribute fragmentAttribute, Expression expression)
    {
      bool isFragment = false;

      Parameter parameter = expression as Parameter;
      if (parameter != null)
      {
        isFragment = Contains(fragmentAttribute, parameter.Attributes);
      }

      return isFragment;
    }

    public static bool Returns(FragmentAttribute fragmentAttribute, Expression expression)
    {
      bool returnsFragment = false;

      MethodCall methodCall = expression as MethodCall;
      if (methodCall != null)
      {
        Method calleeMethod = IntrospectionTools.ExtractMethod (methodCall);
        returnsFragment = Contains(fragmentAttribute, calleeMethod.ReturnAttributes);
      }

      return returnsFragment;
    }

    public static bool Contains (FragmentAttribute fragmentAttribute, AttributeNodeCollection attributes)
    {
      bool contains = false;
      if(attributes != null)
      {
        contains = attributes.Any (attribute => Is (fragmentAttribute, attribute));
      }
      return contains;
    }

    public static bool Is (FragmentAttribute fragmentAttribute, AttributeNode attribute)
    {
      bool isFragment = false;
      if (FragmentAttribute.IsFragment(attribute))
      {
        foreach (Literal literal in attribute.Expressions)
        {
          string value = literal.Value as string;
          if (FragmentAttribute.OfType (value) == fragmentAttribute)
          {
            isFragment = true;
          }
        }
      }
      return isFragment;
    }
  }
}
