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
  public class InheritanceSampleMethod : InheritanceSampleBase
  {
    public InheritanceSampleMethod ()
        : base ("safe", "safe")
    {
    }

    /// <summary>
    /// Overriding a method from base class, note that FragmentAttribute of nonAnnotatedParameter is ignored because signature of virtual method is used
    /// </summary>
    /// <param name="annotatedParameter"></param>
    /// <param name="nonAnnotatedParameter"></param>
    /// <returns></returns>
    public override string VirtualMethod ([Fragment ("InheritanceFragment")] string annotatedParameter, [Fragment ("InheritanceFragment")] string nonAnnotatedParameter)
    {
      return "dummy";
    }

    public void SafeCallOnInheritedMethod()
    {
      InvariantMethod ("safe", "safe");
    }

    public void UnsafeCallOnInheritedMethod()
    {
      InvariantMethod (UnsafeInheritanceFragmentSource(), "safe");
    }

    public void SafeCallOnOverriddenMethod()
    {
      VirtualMethod ("safe", "safe");
    }

    public void UnsafeCallOnOverriddenMethod()
    {
      VirtualMethod ("safe", UnsafeInheritanceFragmentSource());
    }
  }
}
