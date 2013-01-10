using System;
using InjectionCop.Parser.TypeParsing;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;
using System.Linq;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.FragmentUsageTests
{
  [TestFixture]
  public class FragmentUsageTest
  {
    protected readonly string c_InjectionCopRuleId = "IC0002";

    [Test]
    public void Check_FindsProblem ()
    {
      var rule = new FragmentUsageRule();
      var method = GetMethodFromSampleClass<DerivedClassWithInvalidFragmentUsage>();

      var result = rule.Check (method);
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    //[Test]
    //public void Check_ChecksFragmentsFromInterface()
    //{
    //  var rule = new FragmentUsageRule();
    //  var method1 = IntrospectionUtility.MethodFactory<DerivedClassWithValidFragmentUsage> (
    //      "Foo", IntrospectionUtility.TypeNodeFactory<int>(), IntrospectionUtility.TypeNodeFactory<string>());
    //  var method = method1;


    //  var methods = IntrospectionUtility.TypeNodeFactory<DerivedClassWithValidFragmentUsage>().GetMembers<Method>().ToArray();


    //  var result = rule.Check (method);
    //  Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    //}

    [Test]
    public void Check_MatchesParametersCorrectly ()
    {
      var rule = new FragmentUsageRule();
      var method1 = IntrospectionUtility.MethodFactory<DerivedClassWithInvalidFragmentUsage> (
          "MethodWithFragmentOnSecondParameter", IntrospectionUtility.TypeNodeFactory<int>(), IntrospectionUtility.TypeNodeFactory<int>());
      var method = method1;

      var result = rule.Check (method);
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Check_FormatsMessage ()
    {
      var rule = new FragmentUsageRule();
      var method = GetMethodFromSampleClass<DerivedClassWithInvalidFragmentUsage>();

      var result = rule.Check (method);
      var problem = result[0];
      Assert.That (
          problem.Resolution.ToString(),
          Is.EqualTo ("Expected fragment of type 'ValidFragmentType' from overriden/implemented method, but got 'InvalidFragmentType'."));
    }

    [Test]
    public void Check_DerivedClassWithoutFragmentUsage ()
    {
      var rule = new FragmentUsageRule();
      var method = GetMethodFromSampleClass<DerivedClassWithoutFragmentUsage>();

      var result = rule.Check (method);
      Assert.That (result, Is.Empty);
    }

    [Test]
    public void Check_DerivedClassWithDuplicatedFragmentUsage ()
    {
      var rule = new FragmentUsageRule();
      var method = GetMethodFromSampleClass<DerivedClassWithValidFragmentUsage>();

      var result = rule.Check (method);
      Assert.That (result, Is.Empty);
    }

    private Method GetMethodFromSampleClass<T> ()
    {
      var method = IntrospectionUtility.MethodFactory<T> (
          "Foo", IntrospectionUtility.TypeNodeFactory<int>(), IntrospectionUtility.TypeNodeFactory<string>());
      return method;
    }
  }
}