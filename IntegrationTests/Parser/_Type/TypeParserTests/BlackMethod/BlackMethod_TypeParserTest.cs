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

namespace InjectionCop.IntegrationTests.Parser._Type.TypeParserTests.BlackMethod
{
  [TestFixture]
  public class BlackMethod_BlockParserTest: TypeParserTestBase
  {
    [Test]
    [Category("BlackMethod")]
    public void Parse_BlackMethodCallLiteral_NoProblem()
    {
      Method sample = TestHelper.GetSample<BlackMethodSample>("BlackMethodCallLiteral");
      ProblemCollection result = _typeParser.Parse(sample);
      
      Assert.That(TestHelper.ContainsProblemID("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category("BlackMethod")]
    public void Parse_BlackMethodCallUnsafeSourceNoParameter_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<BlackMethodSample>("BlackMethodCallUnsafeSourceNoParameter");
      ProblemCollection result = _typeParser.Parse(sample);

      Assert.That(TestHelper.ContainsProblemID("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category("BlackMethod")]
    public void Parse_BlackMethodCallSafeSource_NoProblem()
    {
      Method sample = TestHelper.GetSample<BlackMethodSample>("BlackMethodCallSafeSource");
      ProblemCollection result = _typeParser.Parse(sample);

      Assert.That(TestHelper.ContainsProblemID("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category("BlackMethod")]
    public void Parse_BlackMethodCallUnsafeSourceWithSafeParameter_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<BlackMethodSample>("BlackMethodCallUnsafeSourceWithSafeParameter");
      ProblemCollection result = _typeParser.Parse(sample);

      Assert.That(TestHelper.ContainsProblemID("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category("BlackMethod")]
    public void Parse_WhiteMethodCall_NoProblem()
    {
      Method sample = TestHelper.GetSample<BlackMethodSample>("WhiteMethodCall");
      ProblemCollection result = _typeParser.Parse(sample);

      Assert.That(TestHelper.ContainsProblemID("IC_SQLi", result), Is.False);
    }
  }
}
