﻿// Copyright 2012 rubicon informationstechnologie gmbh
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
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Lambda
{
  [TestFixture]
  public class Lambda_TypeParserTest: TypeParserTestBase
  {
    [Test]
    public void Parse_SafeLambdaCall_NoProblem ()
    {
      Method sample = TestHelper.GetSample<LambdaSample> ("SafeLambdaCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeLambdaCall_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<LambdaSample> ("UnsafeLambdaCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeLambdaCallUsingReturn_NoProblem ()
    {
      Method sample = TestHelper.GetSample<LambdaSample> ("SafeLambdaCallUsingReturn");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_SafeMethodCallInsideLambda_NoProblem()
    {
      Method sample = TestHelper.GetSample<LambdaSample>("SafeMethodCallInsideLambda");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeMethodCallInsideLambda_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<LambdaSample>("UnsafeMethodCallInsideLambda");
      _typeParser.Parse(sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That(TestHelper.ContainsProblemID(c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeReturnInsideLambda_NoProblem()
    {
      Method sample = TestHelper.GetSample<LambdaSample>("SafeReturnInsideLambda");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeReturnInsideLambda_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<LambdaSample>("UnsafeReturnInsideLambda");
      _typeParser.Parse(sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That(TestHelper.ContainsProblemID(c_InjectionCopRuleId, result), Is.True);
    }
  }
}
