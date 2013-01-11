// Copyright 2013 rubicon informationstechnologie gmbh
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
using InjectionCop.Parser.TypeParsing;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.FragmentUsageTests
{
  [TestFixture]
  public class InterfaceImplementingMethodFragmentUsageRuleTest
  {
    protected readonly string c_InjectionCopRuleId = "IC0003";

    [Test]
    public void Check_FindsProblem ()
    {
      var rule = new InterfaceImplementingMethodFragmentUsageRule();
      var method = GetMethodFromSampleClass<ImplementingClassWithInvalidFragmentUsage>();

      var result = rule.Check (method);
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Check_NoProblem()
    {
      var rule = new InterfaceImplementingMethodFragmentUsageRule();
      var method = GetMethodFromSampleClass<ImplementingClassWithoutFragmentUsage>();

      var result = rule.Check (method);
      Assert.That (result, Is.Empty);
    }
    
    [Test]
    public void Check_FormatsMessage ()
    {
      var rule = new InterfaceImplementingMethodFragmentUsageRule();
      var method = GetMethodFromSampleClass<ImplementingClassWithInvalidFragmentUsage>();

      var result = rule.Check (method);
      var problem = result[0];
      Assert.That (
          problem.Resolution.ToString(),
          Is.EqualTo ("Expected fragment of type 'ValidFragmentType' from implemented interface method, but got 'InvalidFragmentType'."));
    }

    private Method GetMethodFromSampleClass<T> ()
    {
      var method = IntrospectionUtility.MethodFactory<T> (
          "Foo", IntrospectionUtility.TypeNodeFactory<int>(), IntrospectionUtility.TypeNodeFactory<string>());
      return method;
    }
  }
}