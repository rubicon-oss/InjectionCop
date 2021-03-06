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
using InjectionCop.Parser;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Switch
{
  [TestFixture]
  public class Switch_TypeParserTest : TypeParserTestBase
  {
    [Test]
    public void Parse_ValidSwitch_NoProblem ()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("ValidSwitch", intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeCallInsideSwitch_ReturnsProblem ()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("UnsafeCallInsideSwitch", intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }


    [Test]
    public void Parse_UnsafeCallAfterSwitch_ReturnsProblem ()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("UnsafeCallAfterSwitch", intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_UnsafeCallAfterNestedSwitch_ReturnsProblem ()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("UnsafeCallAfterNestedSwitch", intTypeNode, intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeCallAfterNestedSwitch_NoProblem ()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("SafeCallAfterNestedSwitch", intTypeNode, intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeCallInsideNestedSwitch_ReturnsProblem ()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("UnsafeCallInsideNestedSwitch", intTypeNode, intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_ValidFallThrough_NoProblem ()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("ValidFallThrough", intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_ValidFallThroughGoto_NoProblem ()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("ValidFallThroughGoto", intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_InvalidFallThrough_ReturnsProblem ()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("InvalidFallThrough", intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_InvalidFallThroughGoto_ReturnsProblem ()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("InvalidFallThroughGoto", intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
  }
}
