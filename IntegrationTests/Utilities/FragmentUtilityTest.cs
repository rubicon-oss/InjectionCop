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
using InjectionCop.IntegrationTests.Parser;
using InjectionCop.Parser;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Utilities
{
  [TestFixture]
  public class FragmentUtilityTest
  {
    [Test]
    public void IsFragment_ContainsFragmentParameter_True()
    {
        TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
        Method sample = TestHelper.GetSample<FragmentUtilitySample>("ContainsFragmentParameter", stringTypeNode);
        bool isResult = FragmentUtility.IsFragment(sample.Parameters[0].Attributes[0]);
        Assert.That(isResult, Is.True);
    }

    [Test]
    public void IsFragment_ContainsNonFragmentParameter_False()
    {
        TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
        Method sample = TestHelper.GetSample<FragmentUtilitySample>("ContainsNonFragmentParameter", stringTypeNode);
        bool isResult = FragmentUtility.IsFragment(sample.Parameters[0].Attributes[0]);
        Assert.That(isResult, Is.False);
    }

    [Test]
    public void IsFragment_ContainsStronglyTypedSqlFragmentParameter_True()
    {
        TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
        Method sample = TestHelper.GetSample<FragmentUtilitySample>("ContainsStronglyTypedSqlFragmentParameter", stringTypeNode);
        bool isResult = FragmentUtility.IsFragment(sample.Parameters[0].Attributes[0]);
        Assert.That(isResult, Is.True);
    }

    [Test]
    public void ContainsFragment_ContainsFragmentParameter_True()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentUtilitySample>("ContainsFragmentParameter", stringTypeNode);
      bool isResult = FragmentUtility.ContainsFragment(sample.Parameters[0].Attributes);
      Assert.That(isResult, Is.True);
    }

    [Test]
    public void ContainsFragment_ContainsNonFragmentParameter_False()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentUtilitySample>("ContainsNonFragmentParameter", stringTypeNode);
      bool isResult = FragmentUtility.ContainsFragment(sample.Parameters[0].Attributes);
      Assert.That(isResult, Is.False);
    }

    [Test]
    public void ContainsFragment_ContainsStronglyTypedSqlFragmentParameter_True()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentUtilitySample>("ContainsStronglyTypedSqlFragmentParameter", stringTypeNode);
      bool isResult = FragmentUtility.ContainsFragment(sample.Parameters[0].Attributes);
      Assert.That(isResult, Is.True);
    }

    [Test]
    public void GetFragmentType_ContainsFragmentParameter_ReturnsType()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentUtilitySample>("ContainsFragmentParameter", stringTypeNode);
      var fragmentType = FragmentUtility.GetFragmentType(sample.Parameters[0].Attributes);
      Assert.That(fragmentType, Is.EqualTo(Fragment.CreateNamed("FragmentType")));
    }

    [Test]
    public void GetFragmentType_ContainsStronglyTypedSqlFragmentParameter_True()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentUtilitySample>("ContainsStronglyTypedSqlFragmentParameter", stringTypeNode);
      var fragmentType = FragmentUtility.GetFragmentType(sample.Parameters[0].Attributes);
      Assert.That(fragmentType, Is.EqualTo(Fragment.CreateNamed("SqlFragment")));
    }

    [Test]
    public void GetFragmentType_ContainsNonFragmentParameter_ThrowsException()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentUtilitySample>("ContainsNonFragmentParameter", stringTypeNode);
      var returnedFragment = FragmentUtility.GetFragmentType(sample.Parameters[0].Attributes);
      Assert.That (returnedFragment, Is.EqualTo (SymbolTable.EMPTY_FRAGMENT));
    }

    [Test]
    public void ReturnFragmentType_NonAnnotatedMethod_ReturnsEmptyFragment ()
    {
      Method sample = TestHelper.GetSample<FragmentUtilitySample> ("NoReturnFragment");
      var returnFragment = FragmentUtility.ReturnFragmentType (sample);

      Assert.That (returnFragment, Is.EqualTo(SymbolTable.EMPTY_FRAGMENT));
    }

    [Test]
    public void ReturnFragmentType_MethodWithAnnotatedReturn_ReturnsNull ()
    {
      Method sample = TestHelper.GetSample<FragmentUtilitySample> ("ReturnFragment");
      var returnFragment = FragmentUtility.ReturnFragmentType (sample);

      Assert.That (returnFragment, Is.EqualTo(Fragment.CreateNamed("ReturnFragmentType")));
    }

    [Test]
    public void ReturnFragmentType_ImplementedInterfaceMethod_ReturnsFragment ()
    {
      Method sample = TestHelper.GetSample<ClassWithMethodReturningFragment> ("MethodWithReturnFragment");
      var returnFragment = FragmentUtility.ReturnFragmentType (sample);

      Assert.That (returnFragment, Is.EqualTo(Fragment.CreateNamed("ReturnFragmentType")));
    }
    
    [Test]
    public void ReturnFragmentType_ExplicitlyImplementedInterfaceMethod_ReturnsFragment ()
    {
      Method sample = TestHelper.GetSample<ClassWithExplicitlyDeclaredMethodReturningFragment> ("InjectionCop.IntegrationTests.Utilities.InterfaceWithReturnFragment.MethodWithReturnFragment");
      var returnFragment = FragmentUtility.ReturnFragmentType (sample);

      Assert.That (returnFragment, Is.EqualTo (Fragment.CreateNamed("ReturnFragmentType")));
    }

    [Test]
    public void IsFragmentGenerator_ReturnFragment_False()
    {
      Method sampleMethod = TestHelper.GetSample<FragmentUtilitySample>("ReturnFragment");
      Assert.That(FragmentUtility.IsFragmentGenerator(sampleMethod), Is.False);
    }

    [Test]
    public void IsFragmentGenerator_FragmentGenerator_True()
    {
      Method sampleMethod = TestHelper.GetSample<FragmentUtilitySample>("FragmentGenerator");
      Assert.That(FragmentUtility.IsFragmentGenerator(sampleMethod), Is.True);
    }

    [Test]
    public void IsFragmentGenerator_CustomFragmentGenerator_True()
    {
      Method sampleMethod = TestHelper.GetSample<FragmentUtilitySample>("CustomFragmentGenerator");
      Assert.That(FragmentUtility.IsFragmentGenerator(sampleMethod), Is.True);
    }
  }
}
