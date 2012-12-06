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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.BlacklistManagerIntegration
{
  [TestFixture]
  public class BlacklistManagerIntegration_TypeParserTest : TypeParserTestBase
  {
    [Test]
    public void Parse_UnsafeBlacklistedCall_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<BlacklistManagerIntegrationSample>("UnsafeBlacklistedCall");
      ProblemCollection result = _typeParser.Parse(sample);
      
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_UnsafeBlacklistedCall_ReturnsExactlyOneProblem ()
    {
      Method sample = TestHelper.GetSample<BlacklistManagerIntegrationSample>("UnsafeBlacklistedCall");
      ProblemCollection result = _typeParser.Parse(sample);
      
      Assert.That (result.Count, Is.EqualTo(1));
    }

    [Test]
    public void Parse_SafeBlacklistedCall_NoProblem ()
    {    
      Method sample = TestHelper.GetSample<BlacklistManagerIntegrationSample>("SafeBlacklistedCall");
      ProblemCollection result = _typeParser.Parse(sample);
      
      Assert.That (result.Count, Is.EqualTo(0));
    }

    [Test]
    public void Parse_ListedAndUnlistedViolation_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<BlacklistManagerIntegrationSample>("ListedAndUnlistedViolation");
      ProblemCollection result = _typeParser.Parse(sample);
      
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_ListedAndUnlistedViolation_ReturnsExactlyTwoProblems ()
    {
      Method sample = TestHelper.GetSample<BlacklistManagerIntegrationSample>("ListedAndUnlistedViolation");
      ProblemCollection result = _typeParser.Parse(sample);
      
      Assert.That (result.Count, Is.EqualTo(2));
    }

    [Test]
    public void Parse_MixedViolations_ReturnsProblem ()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<BlacklistManagerIntegrationSample> ("MixedViolations", intTypeNode);
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_FragmentDefinedInXmlSafeCall_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<BlacklistManagerIntegrationSample>("FragmentDefinedInXmlSafeCall");
      ProblemCollection result = _typeParser.Parse(sample);
      
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_FragmentDefinedInXmlUnsafeCall_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<BlacklistManagerIntegrationSample>("FragmentDefinedInXmlUnsafeCall");
      ProblemCollection result = _typeParser.Parse(sample);
      
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_MixedViolations_FindsAllProblems ()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<BlacklistManagerIntegrationSample> ("MixedViolations", intTypeNode);
      ProblemCollection result = _typeParser.Parse (sample);
      bool problemIdsCorrect = true;
      foreach (var problem in result)
      {
        problemIdsCorrect = problemIdsCorrect && problem.Id == c_InjectionCopRuleId;
      }
      bool allViolationsFound = result.Count == 4;
      Assert.That (problemIdsCorrect && allViolationsFound, Is.True);
    }
  }
}
