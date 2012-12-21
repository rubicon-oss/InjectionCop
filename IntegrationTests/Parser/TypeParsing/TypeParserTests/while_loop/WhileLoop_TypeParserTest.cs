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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.While_loop
{
  [TestFixture]
  public class WhileLoop_TypeParserTest : TypeParserTestBase
  {
    [Test]
    public void Parse_ValidCallInsideWhile_NoProblem ()
    {
      Method sample = TestHelper.GetSample<WhileLoopSample> ("ValidCallInsideWhile");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_InValidCallInsideWhile_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<WhileLoopSample> ("InValidCallInsideWhile");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;
      
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_InValidCallInsideWhileReprocessingRequired_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<WhileLoopSample> ("InValidCallInsideWhileReprocessingRequired");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;
      
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_InValidAssignmentInsideWhileReprocessingRequired_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<WhileLoopSample> ("InValidAssignmentInsideWhileReprocessingRequired");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;
      
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_InvalidCallInsideNestedWhile_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<WhileLoopSample> ("InvalidCallInsideNestedWhile");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_InValidCallInsideNestedWhileReprocessingRequired_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<WhileLoopSample> ("InValidCallInsideNestedWhileReprocessingRequired");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_InValidCallInsideDeeperNestedWhileReprocessingRequired_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<WhileLoopSample> ("InValidCallInsideDeeperNestedWhileReprocessingRequired");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_ValidCallInsideWhileWithContinue_NoProblem ()
    {
      Method sample = TestHelper.GetSample<WhileLoopSample> ("ValidCallInsideWhileWithContinue");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_InvalidCallInsideWhileWithContinue_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<WhileLoopSample> ("InvalidCallInsideWhileWithContinue");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_InvalidCallInsideIfWithContinue_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<WhileLoopSample> ("InvalidCallInsideIfWithContinue");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_ValidCallInsideWhileWithBreak_NoProblem ()
    {
      Method sample = TestHelper.GetSample<WhileLoopSample> ("ValidCallInsideWhileWithBreak");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_InvalidCallInsideWhileWithBreak_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<WhileLoopSample> ("InvalidCallInsideWhileWithBreak");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_InvalidCallInsideIfWithBreak_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<WhileLoopSample> ("InvalidCallInsideIfWithBreak");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_InValidCallInsideWhileCondition_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<WhileLoopSample> ("InValidCallInsideWhileCondition");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
  }
}
