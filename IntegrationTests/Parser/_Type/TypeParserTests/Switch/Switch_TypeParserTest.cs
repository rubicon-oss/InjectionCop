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

namespace InjectionCop.IntegrationTests.Parser._Type.TypeParserTests.Switch
{
  [TestFixture]
  public class Switch_TypeParserTest : TypeParserTestBase
  {
    [Test]
    public void Parse_ValidSwitch_NoProblem ()
    {
      TypeNode intTypeNode = IntrospectionTools.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("ValidSwitch", intTypeNode);
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    public void Parse_UnsafeCallInsideSwitch_ReturnsProblem ()
    {
      TypeNode intTypeNode = IntrospectionTools.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("UnsafeCallInsideSwitch", intTypeNode);
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }


    [Test]
    public void Parse_UnsafeCallAfterSwitch_ReturnsProblem ()
    {
      TypeNode intTypeNode = IntrospectionTools.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("UnsafeCallAfterSwitch", intTypeNode);
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    public void Parse_UnsafeCallAfterNestedSwitch_ReturnsProblem ()
    {
      TypeNode intTypeNode = IntrospectionTools.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("UnsafeCallAfterNestedSwitch", intTypeNode, intTypeNode);
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    public void Parse_SafeCallAfterNestedSwitch_NoProblem ()
    {
      TypeNode intTypeNode = IntrospectionTools.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("SafeCallAfterNestedSwitch", intTypeNode, intTypeNode);
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    public void Parse_UnsafeCallInsideNestedSwitch_ReturnsProblem ()
    {
      TypeNode intTypeNode = IntrospectionTools.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SwitchSample> ("UnsafeCallInsideNestedSwitch", intTypeNode, intTypeNode);
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }
  }
}
