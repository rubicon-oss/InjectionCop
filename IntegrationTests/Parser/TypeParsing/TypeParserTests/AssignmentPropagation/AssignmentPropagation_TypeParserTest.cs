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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.AssignmentPropagation
{
  [TestFixture]
  public class AssignmentPropagation_TypeParserTest: TypeParserTestBase
  {
    [Test]
    [Category("AssignmentPropagation")]
    public void Parse_ValidSafenessPropagation_NoProblem()
    {
      Method sample = TestHelper.GetSample<AssignmentPropagationSample>("ValidSafenessPropagation");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    [Category("AssignmentPropagation")]
    public void Parse_InvalidSafenessPropagationParameter_ReturnsProblem()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample>("InvalidSafenessPropagationParameter", stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    [Category("AssignmentPropagation")]
    public void Parse_ValidSafenessPropagationParameter_NoProblem()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample>("ValidSafenessPropagationParameter", stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    [Category("AssignmentPropagation")]
    public void Parse_InvalidSafenessPropagationVariable_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<AssignmentPropagationSample>("InvalidSafenessPropagationVariable");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    [Category("AssignmentPropagation")]
    public void Parse_ValidSafenessPropagationVariable_NoProblem()
    {
      Method sample = TestHelper.GetSample<AssignmentPropagationSample>("ValidSafenessPropagationVariable");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_ValidReturnWithIf_NoProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample> ("ValidReturnWithIf", stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_InvalidReturnWithIf_NoProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample> ("InvalidReturnWithIf", stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_InvalidReturnWithIfFragmentTypeConsidered_NoProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample> ("InvalidReturnWithIfFragmentTypeConsidered", stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_InvalidReturnWithTempVariable_NoProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample> ("InvalidReturnWithTempVariable", stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_InvalidReturnWithParameterReset_ReturnsProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample> ("InvalidReturnWithParameterReset", stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_ValidReturnWithParameterReset_NoProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample> ("ValidReturnWithParameterReset", stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_InvalidReturnWithFieldReset_ReturnsProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample> ("InvalidReturnWithFieldReset", stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_ValidReturnWithFieldReset_NoProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample> ("ValidReturnWithFieldReset", stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_InvalidReturnWithField_ReturnsProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample> ("InvalidReturnWithField", stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_ValidReturnWithField_NoProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample> ("ValidReturnWithField", stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_InvalidReturnWithFieldAndLoops_ReturnsProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample> ("InvalidReturnWithFieldAndLoops", stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_ValidReturnWithFieldAndLoops_NoProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample> ("ValidReturnWithFieldAndLoops", stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
  }
}
