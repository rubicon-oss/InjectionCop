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

namespace InjectionCop.Attributes
{
  [AttributeUsage (AttributeTargets.Parameter
                   | AttributeTargets.ReturnValue)]
  public class FragmentAttribute : Attribute
  {
    private readonly string _fragmentType;

    public FragmentAttribute ()
    {
      _fragmentType = "Fragment";
    }

    public FragmentAttribute (string fragmentType)
    {
      _fragmentType = fragmentType;
    }

    public string FragmentType
    {
      get { return _fragmentType; }
    }

    public static bool operator == (FragmentAttribute a, FragmentAttribute b)
    {
      if (System.Object.ReferenceEquals (a, b))
      {
        return true;
      }

      if (((object) a == null) || ((object) b == null))
      {
        return false;
      }

      return a._fragmentType == b._fragmentType;
    }

    public static bool operator != (FragmentAttribute a, FragmentAttribute b)
    {
      return !(a == b);

    }
  }
}