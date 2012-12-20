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
  internal class InheritanceSampleProperty : InheritanceSampleBase
  {
    public InheritanceSampleProperty ()
        : base ("safe", "safe")
    {
    }

    [Fragment ("InheritanceFragment")]
    public new string InitialNonFragmentProperty { get; set; }

    public new string InitialFragmentProperty { get; set; }

    [Fragment ("InheritanceFragment")]
    public override string VirtualProperty { get; set; }

    protected void UnsafeAssignmentOnInheritedProperty ()
    {
      FragmentProperty = UnsafeInheritanceFragmentSource();
    }

    protected void SafeAssignmentOnInheritedPropertyWithLiteral ()
    {
      FragmentProperty = "safe";
    }

    protected void SafeAssignmentOnInheritedProperty ()
    {
      FragmentProperty = SafeInheritanceFragmentSource();
    }

    protected void SafeAssignmentOnPropertyHidingParent ()
    {
      InitialNonFragmentProperty = SafeInheritanceFragmentSource();
    }

    protected void UnsafeAssignmentOnPropertyHidingParent ()
    {
      InitialFragmentProperty = UnsafeInheritanceFragmentSource();
    }

    protected void SafeCallWithInheritedProperty ()
    {
      RequiresInheritanceFragment (FragmentProperty);
    }

    protected void UnsafeCallWithInheritedProperty ()
    {
      RequiresInheritanceFragment (NonFragmentProperty);
    }

    protected void SafeCallWithPropertyHidingParent ()
    {
      RequiresInheritanceFragment (InitialNonFragmentProperty);
    }
    
    protected void UnsafeCallWithPropertyHidingParent ()
    {
      RequiresInheritanceFragment (InitialFragmentProperty);
    }

    public void SafeStaticBindingOnNewProperty ()
    {
      InheritanceSampleBase sample = new InheritanceSampleProperty();
      sample.FragmentProperty = SafeInheritanceFragmentSource();
    }

    public void UnsafeStaticBindingOnNewProperty ()
    {
      InheritanceSampleBase sample = new InheritanceSampleProperty();
      sample.FragmentProperty = UnsafeInheritanceFragmentSource();
    }
    
    public void SafeSetOnOverriddenProperty()
    {
      VirtualProperty = SafeInheritanceFragmentSource();
    }
    
    public void AnotherSafeSetOfOverriddenProperty()
    {
      VirtualProperty = "safe";
    }

    public void UnsafeSetOnOverriddenProperty()
    {
      VirtualProperty = UnsafeInheritanceFragmentSource();
    }
    
    public void SafeDynamicBindingOnProperty ()
    {
      InheritanceSampleBase sample = new InheritanceSampleProperty();
      sample.VirtualProperty = SafeInheritanceFragmentSource();
    }

    public void UnsafeDynamicBindingOnProperty ()
    {
      InheritanceSampleBase sample = new InheritanceSampleProperty();
      sample.VirtualProperty = UnsafeInheritanceFragmentSource();
    }
  }
}
