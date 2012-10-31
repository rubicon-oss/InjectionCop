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
using InjectionCop.Attributes;

namespace InjectionCop.IntegrationTests.Parser
{
  class TypeParserSample
  {
    protected string UnsafeSource()
    {
      return "unsafe command";
    }

    protected string UnsafeSource (string param)
    {
      return param + "unsafe";
    }

    [return: SqlFragment]
    protected string SafeSource()
    {
      return "safe command";
    }

    protected string RequiresSqlFragment([SqlFragment] string param)
    {
      return param;
    }

    protected string RequiresSqlFragment([SqlFragment] string a, string b, [SqlFragment] string c)
    {
      return a+b+c;
    }

    private string safeProp;
    private string unsafeProp;

    public string SafeProp
    { 
      [return: SqlFragment]
      get { return safeProp; }

      [param: SqlFragment]
      set { safeProp = value; }
    }

    public string UnsafeProp 
    { 
      get { return unsafeProp; }
      set { unsafeProp = value; }
    }
  }
}
