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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.BlackMethod
{
  [TestFixture]
  public class BlackMethod_BlockParserTest: TypeParserTestBase
  {
    [Test]
    [Category("BlackMethod")]
    public void Parse_BlackMethodCallLiteral_NoProblem()
    {
      Method sample = TestHelper.GetSample<BlackMethodSample>("BlackMethodCallLiteral");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;
      
      Assert.That(TestHelper.ContainsProblemID(c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    [Category("BlackMethod")]
    [Ignore]
    public void Parse_BlackMethodCallUnsafeSourceNoParameter_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<BlackMethodSample>("BlackMethodCallUnsafeSourceNoParameter");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That(TestHelper.ContainsProblemID(c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    [Category("BlackMethod")]
    public void Parse_BlackMethodCallSafeSource_NoProblem()
    {
      Method sample = TestHelper.GetSample<BlackMethodSample>("BlackMethodCallSafeSource");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That(TestHelper.ContainsProblemID(c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    [Category("BlackMethod")]
    [Ignore]
    public void Parse_BlackMethodCallUnsafeSourceWithSafeParameter_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<BlackMethodSample>("BlackMethodCallUnsafeSourceWithSafeParameter");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That(TestHelper.ContainsProblemID(c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    [Category("BlackMethod")]
    public void Parse_WhiteMethodCall_NoProblem()
    {
      Method sample = TestHelper.GetSample<BlackMethodSample>("WhiteMethodCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That(TestHelper.ContainsProblemID(c_InjectionCopRuleId, result), Is.False);
    }
  }
}
