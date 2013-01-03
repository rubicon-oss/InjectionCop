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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Parameter.CallByReference
{
  [TestFixture]
  public class CallByReference_TypeParserTest: TypeParserTestBase
  {
    [Test]
    public void Parse_FragmentRefParameterSafe_NoProblem ()
    {
      Method sample = TestHelper.GetSample<CallByReferenceSample> ("FragmentRefParameterSafeCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_FragmentRefParameterUnsafe_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<CallByReferenceSample> ("FragmentRefParameterUnsafeCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_RefParameterSafeOperand_NoProblem ()
    {
      Method sample = TestHelper.GetSample<CallByReferenceSample> ("RefParameterSafeOperand");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_RefParameterUnsafeOperand_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<CallByReferenceSample> ("RefParameterUnsafeOperand");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_RefParameterSafeVariableTurningUnsafe_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<CallByReferenceSample> ("RefParameterSafeVariableTurningUnsafe");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_FragmentRefParameterSafeReturn_NoProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<CallByReferenceSample> ("FragmentRefParameterSafeReturn", stringTypeNode.GetReferenceType());
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_FragmentRefParameterUnsafeReturn_ReturnsProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<CallByReferenceSample> ("FragmentRefParameterUnsafeReturn", stringTypeNode.GetReferenceType());
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_FragmentRefParameterSafeReturnWithAssignment_NoProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<CallByReferenceSample> ("FragmentRefParameterSafeReturnWithAssignment", stringTypeNode.GetReferenceType());
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_FragmentRefParameterUnsafeReturnWithAssignment_ReturnsProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<CallByReferenceSample> ("FragmentRefParameterUnsafeReturnWithAssignment", stringTypeNode.GetReferenceType());
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeFragmentRefParameterInsideCondition_NoProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<CallByReferenceSample> ("SafeFragmentRefParameterInsideCondition", stringTypeNode.GetReferenceType());
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeFragmentRefParameterInsideCondition_ReturnsProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<CallByReferenceSample> ("UnsafeFragmentRefParameterInsideCondition", stringTypeNode.GetReferenceType());
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeFragmentRefParameterInsideWhile_NoProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<CallByReferenceSample> ("SafeFragmentRefParameterInsideWhile", stringTypeNode.GetReferenceType());
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    [Ignore("Möglicherweise Problem mit assignment")]
    public void Parse_UnsafeFragmentRefParameterInsideWhile_ReturnsProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<CallByReferenceSample> ("UnsafeFragmentRefParameterInsideWhile", stringTypeNode.GetReferenceType());
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_UnsafeReturnWithAssignment_ReturnsProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<CallByReferenceSample> ("UnsafeReturnWithAssignment", stringTypeNode.GetReferenceType());
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
  }
}
