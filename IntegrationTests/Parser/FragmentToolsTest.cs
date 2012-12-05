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
using InjectionCop.Parser;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser
{
  [TestFixture]
  public class FragmentToolsTest
  {
    [Test]
    public void IsFragment_ContainsFragmentParameter_True()
    {
        TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
        Method sample = TestHelper.GetSample<FragmentToolsSample>("ContainsFragmentParameter", stringTypeNode);
        bool isResult = FragmentTools.IsFragment(sample.Parameters[0].Attributes[0]);
        Assert.That(isResult, Is.True);
    }

    [Test]
    public void IsFragment_ContainsNonFragmentParameter_False()
    {
        TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
        Method sample = TestHelper.GetSample<FragmentToolsSample>("ContainsNonFragmentParameter", stringTypeNode);
        bool isResult = FragmentTools.IsFragment(sample.Parameters[0].Attributes[0]);
        Assert.That(isResult, Is.False);
    }

    [Test]
    public void IsFragment_ContainsStronglyTypedSqlFragmentParameter_True()
    {
        TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
        Method sample = TestHelper.GetSample<FragmentToolsSample>("ContainsStronglyTypedSqlFragmentParameter", stringTypeNode);
        bool isResult = FragmentTools.IsFragment(sample.Parameters[0].Attributes[0]);
        Assert.That(isResult, Is.True);
    }

    [Test]
    public void ContainsFragment_ContainsFragmentParameter_True()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentToolsSample>("ContainsFragmentParameter", stringTypeNode);
      bool isResult = FragmentTools.ContainsFragment(sample.Parameters[0].Attributes);
      Assert.That(isResult, Is.True);
    }

    [Test]
    public void ContainsFragment_ContainsNonFragmentParameter_False()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentToolsSample>("ContainsNonFragmentParameter", stringTypeNode);
      bool isResult = FragmentTools.ContainsFragment(sample.Parameters[0].Attributes);
      Assert.That(isResult, Is.False);
    }

    [Test]
    public void ContainsFragment_ContainsStronglyTypedSqlFragmentParameter_True()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentToolsSample>("ContainsStronglyTypedSqlFragmentParameter", stringTypeNode);
      bool isResult = FragmentTools.ContainsFragment(sample.Parameters[0].Attributes);
      Assert.That(isResult, Is.True);
    }

    [Test]
    public void GetFragmentType_ContainsFragmentParameter_ReturnsType()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentToolsSample>("ContainsFragmentParameter", stringTypeNode);
      string fragmentType = FragmentTools.GetFragmentType(sample.Parameters[0].Attributes);
      Assert.That(fragmentType, Is.EqualTo("FragmentType"));
    }

    [Test]
    public void GetFragmentType_ContainsStronglyTypedSqlFragmentParameter_True()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentToolsSample>("ContainsStronglyTypedSqlFragmentParameter", stringTypeNode);
      string fragmentType = FragmentTools.GetFragmentType(sample.Parameters[0].Attributes);
      Assert.That(fragmentType, Is.EqualTo("SqlFragment"));
    }

    [Test]
    public void GetFragmentType_ContainsNonFragmentParameter_ThrowsException()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentToolsSample>("ContainsNonFragmentParameter", stringTypeNode);
      string returnedFragment = FragmentTools.GetFragmentType(sample.Parameters[0].Attributes);
      Assert.That (returnedFragment, Is.EqualTo (SymbolTable.EMPTY_FRAGMENT));
    }
  }
}
