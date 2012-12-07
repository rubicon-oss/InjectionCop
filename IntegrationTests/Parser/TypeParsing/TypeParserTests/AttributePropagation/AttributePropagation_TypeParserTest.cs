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
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.AttributePropagation
{
  [TestFixture]
  public class AttributePropagation_TypeParserTest: TypeParserTestBase
  {
    [Test]
    [Category("AttributePropagation")]
    [Ignore("Feature 'Checking if return value is safe' temporarily removed")]
    public void Parse_SafeSource_NoProblem()
    {
      Method sample = TestHelper.GetSample<AttributePropagationSample>("SafeSource");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    [Category("AttributePropagation")]
    public void Parse_SafeCallOfSqlFragmentCallee_NoProblem()
    {
      Method sample = TestHelper.GetSample<AttributePropagationSample>("SafeCallOfSqlFragmentCallee");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    [Category("AttributePropagation")]
    public void Parse_UnsafeCallOfSqlFragmentCallee_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<AttributePropagationSample>("UnsafeCallOfSqlFragmentCallee");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    [Category("AttributePropagation")]
    public void Parse_SafeCallOfMixedCallee_NoProblem()
    {
      Method sample = TestHelper.GetSample<AttributePropagationSample>("SafeCallOfMixedCallee");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    [Category("AttributePropagation")]
    public void Parse_UnsafeCallOfMixedCallee_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<AttributePropagationSample>("UnsafeCallOfMixedCallee");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
  }
}
