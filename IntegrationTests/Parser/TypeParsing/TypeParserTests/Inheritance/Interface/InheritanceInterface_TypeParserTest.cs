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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Inheritance.Interface
{
  [TestFixture]
  public class InheritanceInterface_TypeParserTest : TypeParserTestBase
  {
    [Test]
    public void Parse_SafeCallOnInheritedMethod_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleInterface>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeCallOnInterfaceMethodWithFragmentParameter");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeCallOnInterfaceMethodWithFragmentParameter_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleInterface>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeCallOnInterfaceMethodWithFragmentParameter");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_InterfaceReturnFragmentsAreConsidered_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleInterface>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "InterfaceReturnFragmentsAreConsidered");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_SafeCallOnExplicitInterfaceMethodWithFragmentParameter_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleInterface>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeCallOnExplicitInterfaceMethodWithFragmentParameter");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeCallOnExplicitInterfaceMethodWithFragmentParameter_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleInterface>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeCallOnExplicitInterfaceMethodWithFragmentParameter");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_InterfaceReturnFragmentsOfExplicitlyDeclaredMethodAreConsidered_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleInterface>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "InterfaceReturnFragmentsOfExplicitlyDeclaredMethodAreConsidered");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_SafeCallOnClassImplementingInterfaceMethodWithFragmentParameter_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleInterface>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "SafeCallOnClassImplementingInterfaceMethodWithFragmentParameter");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeCallOnClassImplementingInterfaceMethodWithFragmentParameter_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleInterface>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "UnsafeCallOnClassImplementingInterfaceMethodWithFragmentParameter");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_InterfaceReturnFragmentsOfClassImplementingInterfaceMethodAreConsidered_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleInterface>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "InterfaceReturnFragmentsOfClassImplementingInterfaceMethodAreConsidered");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_ValidReturnOnImplicitInterfaceMethod_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory (typeof (InterfaceSampleImplicitDeclarations));
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "MethodWithReturnFragment");
      
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_InvalidReturnOnImplicitInterfaceMethod_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory (typeof (InterfaceSampleImplicitDeclarationsInvalidReturn));
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "MethodWithReturnFragment");

      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_ValidReturnOnExplicitInterfaceMethod_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory (typeof (InterfaceSampleExplicitDeclarations));
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Inheritance.Interface.IInheritanceSample.MethodWithReturnFragment");
      
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_InvalidReturnOnExplicitInterfaceMethod_ReturnsProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory (typeof (InterfaceSampleExplicitDeclarationsInvalidReturn));
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, "InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Inheritance.Interface.IInheritanceSample.MethodWithReturnFragment");

      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
  }
}
