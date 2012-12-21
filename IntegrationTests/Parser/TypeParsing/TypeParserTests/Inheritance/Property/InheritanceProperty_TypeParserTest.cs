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
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Inheritance.Property
{
  [TestFixture]
  public class InheritanceProperty_TypeParserTest : TypeParserTestBase
  {
    [Test]
    public void Parse_UnsafeAssignmentOnInheritedProperty_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeAssignmentOnInheritedProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    
    [Test]
    public void Parse_SafeAssignmentOnInheritedPropertyWithLiteral_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeAssignmentOnInheritedPropertyWithLiteral");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_SafeAssignmentOnInheritedProperty_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeAssignmentOnInheritedProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    
    [Test]
    public void Parse_SafeAssignmentOnPropertyHidingParent_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeAssignmentOnPropertyHidingParent");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_UnsafeAssignmentOnPropertyHidingParent_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeAssignmentOnPropertyHidingParent");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_SafeCallWithInheritedProperty_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeCallWithInheritedProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeCallWithInheritedProperty_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeCallWithInheritedProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeCallWithPropertyHidingParent_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeCallWithPropertyHidingParent");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_UnsafeCallWithPropertyHidingParent_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeCallWithPropertyHidingParent");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeStaticBindingOnNewProperty_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeStaticBindingOnNewProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeStaticBindingOnNewProperty_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeStaticBindingOnNewProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeSetOnOverriddenProperty_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeSetOnOverriddenProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_AnotherSafeSetOfOverriddenProperty_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "AnotherSafeSetOfOverriddenProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_UnsafeSetOnOverriddenProperty_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeSetOnOverriddenProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeDynamicBindingOnProperty_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeDynamicBindingOnProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_UnsafeDynamicBindingOnProperty_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeDynamicBindingOnProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeAssignmentOnBaseProperty_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeAssignmentOnBaseProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeAssignmentOnBaseProperty_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeAssignmentOnBaseProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeAssignmentOfBaseProperty_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeAssignmentOfBaseProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeAssignmentOfBaseProperty_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeAssignmentOfBaseProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeMethodCallUsingBaseProperty_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeMethodCallUsingBaseProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_UnsafeMethodCallUsingBaseProperty_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleProperty>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeMethodCallUsingBaseProperty");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
  }
}
