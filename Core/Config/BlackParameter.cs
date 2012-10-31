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

namespace InjectionCop.Config
{
  public class BlackParameter
  {
    private string name;
    private string fragmentType;
    private string type;

    public BlackParameter (string _name, string _fragmentType, string _type)
    {
      name = _name;
      fragmentType = _fragmentType;
      type = _type;
    }

    public string Name
    {
      get { return name; }
      set { name = value; }
    }

    public string FragmentType
    {
      get { return fragmentType; }
      set { fragmentType = value; }
    }

    public string Type
    {
      get { return type; }
      set { type = value; }
    }
  }
}
