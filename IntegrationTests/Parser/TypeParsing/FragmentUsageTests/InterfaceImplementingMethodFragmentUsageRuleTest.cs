using System;
using InjectionCop.Parser.TypeParsing;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;
using System.Linq;

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