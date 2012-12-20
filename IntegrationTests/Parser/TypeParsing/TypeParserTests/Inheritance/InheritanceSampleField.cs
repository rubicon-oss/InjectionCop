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
  public class InheritanceSampleField : InheritanceSampleBase
  {
    [Fragment ("InheritanceFragment")]
    protected new string _initialNonFragmentField;

    protected new string _initialFragmentField;

    public InheritanceSampleField ()
        : base ("safe", "safe")
    {
    }

    protected void UnsafeAssignmentOnInheritedField ()
    {
      _fragmentField = UnsafeInheritanceFragmentSource();
    }

    protected void SafeAssignmentOnInheritedFieldWithLiteral ()
    {
      _fragmentField = "safe";
    }

    protected void SafeAssignmentOnInheritedField ()
    {
      _fragmentField = SafeInheritanceFragmentSource();
    }

    protected void SafeAssignmentOnFieldSetByHidingParent ()
    {
      _initialNonFragmentField = SafeInheritanceFragmentSource();
    }

    protected void UnsafeAssignmentOnFieldResetByHidingParent ()
    {
      _initialFragmentField = UnsafeInheritanceFragmentSource();
    }

    protected void SafeCallWithInheritedField ()
    {
      RequiresInheritanceFragment (_fragmentField);
    }

    protected void UnsafeCallWithInheritedField ()
    {
      RequiresInheritanceFragment (_nonFragmentField);
    }

    protected void SafeCallWithFieldSetByHidingParentField ()
    {
      RequiresInheritanceFragment (_initialNonFragmentField);
    }

    protected void UnsafeCallWithFieldSetByHidingParentField ()
    {
      RequiresInheritanceFragment (_initialFragmentField);
    }

    protected void SafeAssignmentOnBaseField ()
    {
      base._initialFragmentField = SafeInheritanceFragmentSource();
    }

    protected void UnsafeAssignmentOnBaseField ()
    {
      base._initialFragmentField = UnsafeInheritanceFragmentSource();
    }
  }
}
