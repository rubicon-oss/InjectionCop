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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Inheritance.MemberMethod
{
  public class InheritanceSampleMethod : InheritanceSampleBase
  {
    public class ExtendedSample : InheritanceSampleBase
    {
      public ExtendedSample ()
          : base ("", "")
      {
      }
    }

    public class ExtendedExtendedSample : ExtendedSample
    {
    }

    public InheritanceSampleMethod ()
        : base ("safe", "safe")
    {
    }
   
    public override string VirtualMethod ([Fragment ("InheritanceFragment")] string annotatedParameter, string nonAnnotatedParameter)
    {
      return "dummy";
    }

    public new string NonVirtualMethod ([Fragment ("InheritanceFragment")] string annotatedParameter, [Fragment ("InheritanceFragment")] string nonAnnotatedParameter)
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

    public void SafeCallOnMethodInheritedFromSuperiorClass()
    {
      ExtendedExtendedSample sample = new ExtendedExtendedSample();
      sample.InvariantMethod ("safe", "safe");
    }

    public void UnsafeCallOnMethodInheritedFromSuperiorClass()
    {
      ExtendedExtendedSample sample = new ExtendedExtendedSample();
      sample.InvariantMethod (UnsafeInheritanceFragmentSource(), "safe");
    }

    public void SafeCallOnNewMethod ()
    {
      NonVirtualMethod ("safe", "safe");
    }

    public void UnsafeCallOnNewMethod ()
    {
      NonVirtualMethod ("safe", UnsafeInheritanceFragmentSource());
    }

    public void SafeStaticBindingOnNewMethod ()
    {
      InheritanceSampleBase sample = new InheritanceSampleMethod();
      sample.NonVirtualMethod ("", "");
    }

    public void UnsafeStaticBindingOnNewMethod ()
    {
      InheritanceSampleBase sample = new InheritanceSampleMethod();
      sample.NonVirtualMethod (UnsafeInheritanceFragmentSource(), "");
    }

    public void SafeCallBaseMethod ()
    {
      base.NonVirtualMethod ("safe", "safe");
    }

    public void UnsafeCallBaseMethod ()
    {
      base.NonVirtualMethod (UnsafeInheritanceFragmentSource(), "safe");
    }

    public void SafeCallOnOverriddenMethod()
    {
      VirtualMethod ("safe", "safe");
    }

    public void AnotherSafeCallOnOverriddenMethod()
    {
      VirtualMethod ("safe", UnsafeInheritanceFragmentSource());
    }

    public void UnsafeCallOnOverriddenMethod()
    {
      VirtualMethod (UnsafeInheritanceFragmentSource(), "safe");
    }

    public void SafeDynamicBinding ()
    {
      InheritanceSampleBase sample = new InheritanceSampleMethod();
      sample.VirtualMethod ("safe", "safe");
    }

    public void UnsafeDynamicBinding ()
    {
      InheritanceSampleBase sample = new InheritanceSampleMethod();
      sample.VirtualMethod (UnsafeInheritanceFragmentSource(), "safe");
    }
  }
}
