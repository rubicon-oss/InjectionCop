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
using System.Linq;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.AddProblem
{
  [TestFixture]
  public class AddProblem_TypeParserTest : TypeParserTestBase
  {
    [Test]
    public void AddProblem_NoViolation_ReturnsNoProblem ()
    {
      Method sample = TestHelper.GetSample<AddProblemSample>("NoViolation");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (result.Count, Is.EqualTo(0));
    }

    [Test]
    public void AddProblem_OneViolation_ReturnsInjectionCopProblem ()
    {
      Method sample = TestHelper.GetSample<AddProblemSample>("OneViolation");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void AddProblem_OneViolation_ReturnsOneProblem ()
    {
      Method sample = TestHelper.GetSample<AddProblemSample>("OneViolation");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (result.Count, Is.EqualTo(1));
    }

    [Test]
    public void AddProblem_TwoViolations_ReturnsInjectionCopProblems ()
    {
      Method sample = TestHelper.GetSample<AddProblemSample>("TwoViolations");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;
      bool returnsInjectionCopProblems = result.All (problem => problem.Id == c_InjectionCopRuleId);
      
      Assert.That (returnsInjectionCopProblems, Is.True);
    }

    [Test]
    public void AddProblem_TwoViolations_ReturnsTwoProblems ()
    {
      Method sample = TestHelper.GetSample<AddProblemSample>("TwoViolations");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (result.Count, Is.EqualTo(2));
    }

    [Test]
    public void AddProblem_OneViolationInWhile_ReturnsInjectionCopProblem ()
    {
      Method sample = TestHelper.GetSample<AddProblemSample>("OneViolationInWhile");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void AddProblem_OneViolationInWhile_ReturnsOneProblem ()
    {
      Method sample = TestHelper.GetSample<AddProblemSample>("OneViolationInWhile");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (result.Count, Is.EqualTo(1));
    }
    
    [Test]
    public void AddProblem_OneViolationInWhileAssignmentAfterCall_ReturnsInjectionCopProblem ()
    {
      Method sample = TestHelper.GetSample<AddProblemSample>("OneViolationInWhileAssignmentAfterCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void AddProblem_OneViolationInWhileAssignmentAfterCall_ReturnsOneProblem ()
    {
      Method sample = TestHelper.GetSample<AddProblemSample>("OneViolationInWhileAssignmentAfterCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (result.Count, Is.EqualTo(1));
    }

    [Test]
    public void AddProblem_TwoViolationsInNestedWhile_ReturnsInjectionCopProblems ()
    {
      Method sample = TestHelper.GetSample<AddProblemSample>("TwoViolationsInNestedWhile");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;
      bool returnsInjectionCopProblems = result.All (problem => problem.Id == c_InjectionCopRuleId);

      Assert.That (returnsInjectionCopProblems, Is.True);
    }

    [Test]
    public void AddProblem_TwoViolationsInNestedWhile_ReturnsTwoProblems ()
    {
      Method sample = TestHelper.GetSample<AddProblemSample>("TwoViolationsInNestedWhile");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (result.Count, Is.EqualTo(2));
    }
  }
}
