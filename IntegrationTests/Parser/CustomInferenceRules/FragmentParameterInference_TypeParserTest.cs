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
using InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.CustomInferenceRules
{
  [TestFixture]
  public class FragmentParameterInference_TypeParserTest: TypeParserTestBase
  {
    [Test]
    public void Parse_ConcatWithLiterals_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("ConcatWithLiterals");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_BinaryConcatEqualFragments_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("BinaryConcatEqualFragments");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_BinaryConcatLiteralAndFragment_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("BinaryConcatLiteralAndFragment");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_BinaryConcatFragmentAndLiteral_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("BinaryConcatFragmentAndLiteral");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_BinaryConcatDifferentFragments_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("BinaryConcatDifferentFragments");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_BinaryConcatEqualFragmentVariables_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("BinaryConcatEqualFragmentVariables");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_BinaryConcatDifferentFragmentVariables_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("BinaryConcatDifferentFragmentVariables");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_BinaryConcatEqualFragmentVariablesAcrossBlocks_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("BinaryConcatEqualFragmentVariablesAcrossBlocks");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_BinaryConcatDifferentFragmentVariablesAcrossBlocks_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("BinaryConcatDifferentFragmentVariablesAcrossBlocks");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_BinaryConcatSafeNestedCall_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("BinaryConcatSafeNestedCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_BinaryConcatUnsafeNestedCall_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("BinaryConcatUnsafeNestedCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_TernaryConcatSafeCall_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("TernaryConcatSafeCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_TernaryConcatUnsafeCall_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("TernaryConcatUnsafeCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_QuarternaryConcatSafeCall_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("QuarternaryConcatSafeCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_QuarternaryConcatUnsafeCall_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("QuarternaryConcatUnsafeCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_BinaryStringFormatSafeCall_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("BinaryStringFormatSafeCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_BinaryStringFormatSafeCallUsingSqlFragment_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("BinaryStringFormatSafeCallUsingSqlFragment");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_BinaryStringFormatUnsafeCall_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("BinaryStringFormatUnsafeCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_TernaryStringFormatSafeCall_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("TernaryStringFormatSafeCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_TernaryStringFormatUnsafeCall_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("TernaryStringFormatUnsafeCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_QuarternaryStringFormatSafeCall_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("QuarternaryStringFormatSafeCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_QuarternaryStringFormatUnsafeCall_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("QuarternaryStringFormatUnsafeCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeConcatCallWithArray_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FragmentParameterInferenceSample> ("SafeConcatCallWithArray");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
  }
}
