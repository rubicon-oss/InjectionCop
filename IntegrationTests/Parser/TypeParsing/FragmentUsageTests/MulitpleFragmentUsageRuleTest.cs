using System;
using InjectionCop.Parser.TypeParsing;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.FragmentUsageTests
{
  [TestFixture]
  public class MulitpleFragmentUsageRuleTest
  {
    protected readonly string c_InjectionCopRuleId = "IC0004";

    [Test]
    public void CheckMember_InvalidFragmentUsage ()
    {
      var sampleMethod = GetSampleMethod ("InvalidFragmentUsage");

      var rule = new MultipleFragmentUsageRule();
      var result = rule.Check (sampleMethod);
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void CheckMember_FormatsMessage ()
    {
      var sampleMethod = GetSampleMethod ("InvalidFragmentUsage");

      var rule = new MultipleFragmentUsageRule();
      var result = rule.Check (sampleMethod);
      Assert.That (result[0].Resolution.ToString(), Is.EqualTo("Parameter 'parameter' has multiple fragment types assigned."));
    }

    [Test]
    public void CheckMember_ValidFragmentUsage ()
    {
      var sampleMethod = GetSampleMethod ("ValidFragmentUsage");

      var rule = new MultipleFragmentUsageRule();
      var result = rule.Check (sampleMethod);
      Assert.That (result, Is.Empty);
    }

    [Test]
    public void CheckMember_NoFragmentUsage()
    {
      var sampleMethod = GetSampleMethod ("NoFragmentUsage");

      var rule = new MultipleFragmentUsageRule();
      var result = rule.Check (sampleMethod);
      Assert.That (result, Is.Empty);
    }

    private Method GetSampleMethod (string methodName)
    {
      return IntrospectionUtility.MethodFactory<ClassWithMultipleFragments> (
          methodName, IntrospectionUtility.TypeNodeFactory<int>());
    }
  }
}