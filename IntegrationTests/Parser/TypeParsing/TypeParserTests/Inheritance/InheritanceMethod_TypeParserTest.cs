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
using System.Reflection;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Inheritance
{
  [TestFixture]
  public class InheritanceMethod_TypeParserTest : TypeParserTestBase
  {
    [Test]
    public void Parse_SafeCallOnInheritedMethod_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeCallOnInheritedMethod");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeCallOnInheritedMethod_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeCallOnInheritedMethod");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeCallOnMethodInheritedFromSuperiorClass_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeCallOnMethodInheritedFromSuperiorClass");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeCallOnMethodInheritedFromSuperiorClass_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeCallOnMethodInheritedFromSuperiorClass");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeCallOnNewMethod_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeCallOnNewMethod");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_UnsafeCallOnNewMethod_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeCallOnNewMethod");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeStaticBindingOnNewMethod_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeStaticBindingOnNewMethod");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeStaticBindingOnNewMethod_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeStaticBindingOnNewMethod");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeCallBaseMethod_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeCallBaseMethod");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_UnsafeCallBaseMethod_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeCallBaseMethod");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeCallOnOverriddenMethod_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeCallOnOverriddenMethod");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_AnotherSafeCallOnOverriddenMethod_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "AnotherSafeCallOnOverriddenMethod");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_UnsafeCallOnOverriddenMethod_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeCallOnOverriddenMethod");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeDynamicBinding_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeDynamicBinding");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeDynamicBinding_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeDynamicBinding");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    /*
    [Test]
    [Ignore]
    public void reflectioncheck ()
    {
      InheritanceSampleBase isb = new InheritanceSampleBase("","");
      ParameterInfo[] pisb = (isb.GetType().GetMethod ("VirtualMethod")).GetParameters();
      TypeNode isbtn = IntrospectionUtility.TypeNodeFactory<InheritanceSampleBase>();

      InheritanceSampleMethod ism = new InheritanceSampleMethod();
      ParameterInfo[] pism = (ism.GetType().GetMethod ("VirtualMethod")).GetParameters();
      TypeNode ismtn = IntrospectionUtility.TypeNodeFactory<InheritanceSampleMethod>();
      ism.UnsafeCallOnOverriddenMethod();

      Assert.Fail();
    }*/

  }
}
