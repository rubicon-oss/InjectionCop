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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Field
{
  [TestFixture]
  public class Field_TypeParserTest : TypeParserTestBase
  {
    [Test]
    public void Parse_CallWithSafeField_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FieldSample>("CallWithSafeField");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    public void Parse_CallWithUnsafeField_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<FieldSample>("CallWithUnsafeField");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    public void Parse_CallWithWrongFragmentType_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<FieldSample>("CallWithWrongFragmentType");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    public void Parse_NestedUnsafeCall_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<FieldSample> ("NestedUnsafeCall");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    public void Parse_NestedSafeCall_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FieldSample> ("NestedSafeCall");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    public void Parse_NestedCallWithWrongFragmentType_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<FieldSample> ("NestedCallWithWrongFragmentType");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    public void Parse_UnsafeFieldAssignment_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<FieldSample> ("UnsafeFieldAssignment");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    public void Parse_SafeFieldAssignment_NoProblem ()
    {
      Method sample = TestHelper.GetSample<FieldSample> ("SafeFieldAssignment");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    public void Parse_WrongFragmentTypeFieldAssignment_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<FieldSample> ("WrongFragmentTypeFieldAssignment");
      ProblemCollection result = _typeParser.Parse (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }
  }
}
