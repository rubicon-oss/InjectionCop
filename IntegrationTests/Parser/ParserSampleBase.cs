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
using InjectionCop.Fragment;

namespace InjectionCop.IntegrationTests.Parser
{
  public class ParserSampleBase
  {
    protected string UnsafeSource ()
    {
      return "unsafe command";
    }

    protected string UnsafeSource (string param)
    {
      return param + "unsafe";
    }

    [return: Fragment("SqlFragment")]
    protected string SafeSource ()
    {
      return "safe command";
    }

    [return: Fragment("SqlFragment")]
    protected string SafeSourceRequiresSqlFragment ([SqlFragment] string param)
    {
      return "safe command";
    }

    protected void DummyMethod (string parameter)
    {
    }

    protected string RequiresSqlFragment ([Fragment("SqlFragment")] string param)
    {
      return param;
    }

    protected string RequiresSqlFragment ([Fragment("SqlFragment")] string a, string b, [Fragment("SqlFragment")] string c)
    {
      return a + b + c;
    }
    
    protected bool RequiresSqlFragmentReturnsBool ([SqlFragment] string param)
    {
      return true;
    }

    [return: Fragment("HtmlFragment")]
    public string ReturnsHtmlFragment ()
    {
      return "HtmlFragment";
    }

    
  }
}