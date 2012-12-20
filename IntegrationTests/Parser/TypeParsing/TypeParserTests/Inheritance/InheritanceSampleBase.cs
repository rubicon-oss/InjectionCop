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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Inheritance
{
  public class InheritanceSampleBase : ParserSampleBase
  {
    [Fragment ("InheritanceFragment")]
    protected string _fragmentField;

    [Fragment ("InheritanceFragment")]
    protected string _initialFragmentField;

    protected string _nonFragmentField;
    protected string _initialNonFragmentField;
    
    private string _dummy;

    public InheritanceSampleBase (string parameter)
    {
      _fragmentField = "safe";
      _nonFragmentField = parameter;
    }

    public InheritanceSampleBase ([Fragment ("InheritanceFragment")] string fragmentField, string nonFragmentField)
    {
      _fragmentField = fragmentField;
      _nonFragmentField = nonFragmentField;
    }

    [Fragment ("InheritanceFragment")]
    public string FragmentProperty { get; set; }

    [Fragment ("InheritanceFragment")]
    public string InitialFragmentProperty { get; set; }

    public string NonFragmentProperty { get; set; }

    public string InitialNonFragmentProperty { get; set; }

    [Fragment ("InheritanceFragment")]
    public virtual string VirtualProperty { get; set; }

    public virtual string VirtualMethod ([Fragment ("InheritanceFragment")] string annotatedParameter, string nonAnnotatedParameter)
    {
      return null;
    }

    public string NonVirtualMethod ([Fragment ("InheritanceFragment")] string annotatedParameter, string nonAnnotatedParameter)
    {
      return null;
    }

    public string InvariantMethod ([Fragment ("InheritanceFragment")] string annotatedParameter, string nonAnnotatedParameter)
    {
      return null;
    }

    protected static string UnsafeInheritanceFragmentSource ()
    {
      return "unsafe";
    }

    [return: Fragment ("InheritanceFragment")]
    protected string SafeInheritanceFragmentSource ()
    {
      return "safe" + _dummy;
    }

    protected void RequiresInheritanceFragment ([Fragment ("InheritanceFragment")] string parameter)
    {
      _dummy = parameter;
    }
  }
}
