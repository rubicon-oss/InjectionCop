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

namespace InjectionCop.IntegrationTests.Parser.BlackMethod
{
  class TypeParserTest_BlackMethod: TypeParserTest
  {
    [Test]
    [Category("BlackMethod")]
    public void Check_BlackMtcLiteral_NoProblem()
    {
      Method sample = TestHelper.GetSample<BlackMethodSample>("BlackMtcLiteral");
      ProblemCollection result = parser.Check(sample);
      
      Assert.That(TestHelper.ContainsProblemID("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category("BlackMethod")]
    public void Check_BlackMtcUnsafeSourceNoParameter_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<BlackMethodSample>("BlackMtcUnsafeSourceNoParameter");
      ProblemCollection result = parser.Check(sample);

      Assert.That(TestHelper.ContainsProblemID("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category("BlackMethod")]
    public void Check_BlackMtcSafeSource_NoProblem()
    {
      Method sample = TestHelper.GetSample<BlackMethodSample>("BlackMtcSafeSource");
      ProblemCollection result = parser.Check(sample);

      Assert.That(TestHelper.ContainsProblemID("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category("BlackMethod")]
    public void Check_BlackMtcUnsafeSourceWithSafeParameter_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<BlackMethodSample>("BlackMtcUnsafeSourceWithSafeParameter");
      ProblemCollection result = parser.Check(sample);

      Assert.That(TestHelper.ContainsProblemID("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category("BlackMethod")]
    public void Check_WhiteMtc_NoProblem()
    {
      Method sample = TestHelper.GetSample<BlackMethodSample>("WhiteMtc");
      ProblemCollection result = parser.Check(sample);

      Assert.That(TestHelper.ContainsProblemID("IC_SQLi", result), Is.False);
    }
  }
}
