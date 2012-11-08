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
using InjectionCop.Parser;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.BlockParserTests.AssignmentPropagation
{
  [TestFixture]
  public class AssignmentPropagation_BlockParserTest: BlockParserTestBase
  {
    [Test]
    [Category("AssignmentPropagation")]
    public void Check_ValidSafenessPropagation_NoProblem()
    {
      Method sample = TestHelper.GetSample<AssignmentPropagationSample>("ValidSafenessPropagation");
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category("AssignmentPropagation")]
    public void Check_InvalidSafenessPropagationParameter_ReturnsProblem()
    {
      TypeNode stringTypeNode = Helper.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample>("InvalidSafenessPropagationParameter", stringTypeNode);
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category("AssignmentPropagation")]
    public void Check_ValidSafenessPropagationParameter_NoProblem()
    {
      TypeNode stringTypeNode = Helper.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<AssignmentPropagationSample>("ValidSafenessPropagationParameter", stringTypeNode);
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category("AssignmentPropagation")]
    public void Check_InvalidSafenessPropagationVariable_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<AssignmentPropagationSample>("InvalidSafenessPropagationVariable");
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category("AssignmentPropagation")]
    public void Check_ValidSafenessPropagationVariable_NoProblem()
    {
      Method sample = TestHelper.GetSample<AssignmentPropagationSample>("ValidSafenessPropagationVariable");
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }
  }
}
