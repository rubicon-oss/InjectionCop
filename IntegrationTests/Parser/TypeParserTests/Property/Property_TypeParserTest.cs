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

namespace InjectionCop.IntegrationTests.Parser.TypeParserTests.Property
{
  [TestFixture]
  public class Property_TypeParserTest: TypeParserTestBase
  {
    [Test]
    [Category("Property")]
    public void Check_CallWithUnsafeProperty_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<PropertySample>("CallWithUnsafeProperty");
      ProblemCollection result = parser.Check(sample);

      Assert.That(TestHelper.ContainsProblemID("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category("Property")]
    public void Check_CallWithSafeProperty_NoProblem()
    {
      Method sample = TestHelper.GetSample<PropertySample>("CallWithSafeProperty");
      ProblemCollection result = parser.Check(sample);

      Assert.That(TestHelper.ContainsProblemID("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category("Property")]
    public void Check_SetPropertySafe_NoProblem()
    {
      Method sample = TestHelper.GetSample<PropertySample>("SetPropertySafe");
      ProblemCollection result = parser.Check(sample);

      Assert.That(TestHelper.ContainsProblemID("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category("Property")]
    public void Check_SetPropertyUnsafe_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<PropertySample>("SetPropertyUnsafe");
      ProblemCollection result = parser.Check(sample);

      Assert.That(TestHelper.ContainsProblemID("IC_SQLi", result), Is.True);
    }
  }
}
