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

namespace InjectionCop.Fragment
{
  public struct Fragment
  {
    private static readonly Dictionary<string, string> _fragmentTypeCache = new Dictionary<string, string>();

    public enum Sort
    {
      Literal = 0,
      NamedFragment = 1
    }

    private readonly Sort _sort;
    private readonly string _fragmentType;

    public Fragment (Sort sort, string fragmentType)
    {
      _sort = sort;
      if (!_fragmentTypeCache.ContainsKey (fragmentType))
      {
        _fragmentTypeCache[fragmentType] = fragmentType;
      }
      _fragmentType = fragmentType;
    }

    public Sort FragmentSort 
    {
      get { return _sort; }
    }

    public string FragmentType 
    {
      get { return _fragmentType; }
    }
  }
}
