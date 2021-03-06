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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.NestedCall
{
  [TestFixture]
  public class NestedCall_TypeParserTest : TypeParserTestBase
  {
    [Test]
    public void Parse_NestedValidCallReturn_NoProblem ()
    {
      Method sample = TestHelper.GetSample<NestedCallSample> ("NestedValidCallReturn");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_NestedInvalidCallReturn_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<NestedCallSample> ("NestedInvalidCallReturn");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
  
    [Test]
    public void Parse_NestedInvalidCall_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<NestedCallSample> ("NestedInvalidCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_DeeperNestedInvalidCall_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<NestedCallSample> ("DeeperNestedInvalidCall");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_ValidMethodCallChain_NoProblem ()
    {
      Method sample = TestHelper.GetSample<NestedCallSample> ("ValidMethodCallChain");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_InvalidMethodCallChain_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<NestedCallSample> ("InvalidMethodCallChain");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_ValidMethodCallChainDifferentOperand_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<NestedCallSample> ("ValidMethodCallChainDifferentOperand");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_InvalidMethodCallChainDifferentOperand_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<NestedCallSample> ("InvalidMethodCallChainDifferentOperand");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
  }
}
