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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.TryCatchFinally
{
  [TestFixture]
  public class TryCatchFinally_TypeParserTest: TypeParserTestBase
  {
    [Test]
    public void Parse_SafeCallInsideTry_NoProblem ()
    {
      Method sample = TestHelper.GetSample<TryCatchFinallySample> ("SafeCallInsideTry");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_UnsafeCallInsideTry_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<TryCatchFinallySample> ("UnsafeCallInsideTry");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeCallInsideCatch_NoProblem ()
    {
      Method sample = TestHelper.GetSample<TryCatchFinallySample> ("SafeCallInsideCatch");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeCallInsideCatch_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<TryCatchFinallySample> ("UnsafeCallInsideCatch");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeCallInsideFinally_NoProblem ()
    {
      Method sample = TestHelper.GetSample<TryCatchFinallySample> ("SafeCallInsideFinally");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeCallInsideFinally_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<TryCatchFinallySample> ("UnsafeCallInsideFinally");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_UnsafeCallNestedTry_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<TryCatchFinallySample> ("UnsafeCallNestedTry");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_UnsafeCallNestedCatch_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<TryCatchFinallySample> ("UnsafeCallNestedCatch");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_UnsafeCallNestedFinally_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<TryCatchFinallySample> ("UnsafeCallNestedFinally");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
  }
}
