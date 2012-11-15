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

namespace InjectionCop.IntegrationTests.Parser.TypeParserTests.Parameter
{
  [TestFixture]
  public class Parameter_BlockParserTest : TypeParserTestBase
  {
    [Test]
    [Category ("Parameter")]
    public void Parse_UnsafeMethodParameter_ReturnsProblem ()
    {
      TypeNode stringTypeNode = Helper.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<ParameterSample> ("UnsafeMethodParameter", stringTypeNode);
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category ("Parameter")]
    public void Parse_SafeMethodParameter_NoProblem ()
    {
      TypeNode stringTypeNode = Helper.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<ParameterSample> ("SafeMethodParameter", stringTypeNode);
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category ("Parameter")]
    public void Parse_FragmentOutParameterSafe_NoProblem ()
    {
      Method sample = TestHelper.GetSample<ParameterSample> ("FragmentOutParameterSafe");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category ("Parameter")]
    public void Parse_FragmentOutParameterUnsafe_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<ParameterSample> ("FragmentOutParameterUnsafe");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category ("Parameter")]
    public void Parse_OutParameterUnsafeOperand_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<ParameterSample> ("OutParameterUnsafeOperand");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category ("Parameter")]
    public void Parse_OutParameterSafeOperand_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<ParameterSample> ("OutParameterSafeOperand");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category ("Parameter")]
    [Ignore]
    public void Parse_FragmentRefParameterSafe_NoProblem ()
    {
      Method sample = TestHelper.GetSample<ParameterSample> ("FragmentRefParameterSafe");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category ("Parameter")]
    [Ignore]
    public void Parse_FragmentRefParameterUnsafe_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<ParameterSample> ("FragmentRefParameterUnsafe");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category ("Parameter")]
    [Ignore]
    public void Parse_RefParameterUnsafeOperand_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<ParameterSample> ("RefParameterUnsafeOperand");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category ("Parameter")]
    [Ignore]
    public void Parse_RefParameterSafeOperand_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<ParameterSample> ("RefParameterSafeOperand");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }
  }
}