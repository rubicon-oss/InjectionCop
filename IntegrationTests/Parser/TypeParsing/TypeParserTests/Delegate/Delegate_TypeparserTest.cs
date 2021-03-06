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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Delegate
{
  [TestFixture]
  public class Delegate_TypeParserTest: TypeParserTestBase
  {
    [Test]
    public void Parse_SafeDelegateCall_NoProblem ()
    {
      Method sample = TestHelper.GetSample<DelegateSample> ("SafeDelegateCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeDelegateCall_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<DelegateSample> ("UnsafeDelegateCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

     [Test]
    public void Parse_SafeDelegateCallUsingReturn_NoProblem ()
    {
      Method sample = TestHelper.GetSample<DelegateSample> ("SafeDelegateCallUsingReturn");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_SafeDelegateFieldCall_NoProblem ()
    {
      Method sample = TestHelper.GetSample<DelegateSample> ("SafeDelegateFieldCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeDelegateFieldCall_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<DelegateSample> ("UnsafeDelegateFieldCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_DelegateWithSafeReturn_NoProblem()
    {
      Method sample = TestHelper.GetSample<DelegateSample>("DelegateWithSafeReturn");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_DelegateWithUnsafeReturn_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<DelegateSample>("DelegateWithUnsafeReturn");
      _typeParser.Parse(sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That(TestHelper.ContainsProblemID(c_InjectionCopRuleId, result), Is.True);
    }
  }
}
