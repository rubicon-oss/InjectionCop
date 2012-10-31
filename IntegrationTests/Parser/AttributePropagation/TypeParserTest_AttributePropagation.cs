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

namespace InjectionCop.IntegrationTests.Parser.AttributePropagation
{
  class TypeParserTest_AttributePropagation: TypeParserTest
  {
    [Test]
    [Category("AttributePropagation")]
    [Ignore("Feature 'Checking if return value is safe' temporarily removed")]
    public void Check_SafeSource_NoProblem()
    {
      Method sample = TestHelper.GetSample<AttributePropagationSample>("SafeSource");
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category("AttributePropagation")]
    public void Check_SafeCallOfSqlFragmentCallee_NoProblem()
    {
      Method sample = TestHelper.GetSample<AttributePropagationSample>("SafeCallOfSqlFragmentCallee");
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category("AttributePropagation")]
    public void Check_UnsafeCallOfSqlFragmentCallee_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<AttributePropagationSample>("UnsafeCallOfSqlFragmentCallee");
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category("AttributePropagation")]
    public void Check_SafeCallOfMixedCallee_NoProblem()
    {
      Method sample = TestHelper.GetSample<AttributePropagationSample>("SafeCallOfMixedCallee");
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category("AttributePropagation")]
    public void Check_UnsafeCallOfMixedCallee_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<AttributePropagationSample>("UnsafeCallOfMixedCallee");
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }
  }
}
