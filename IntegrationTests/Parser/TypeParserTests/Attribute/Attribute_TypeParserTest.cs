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

namespace InjectionCop.IntegrationTests.Parser.TypeParserTests.Attribute
{
  [TestFixture]
  public class Attribute_TypeParserTest
  {
    private TypeParser _typeParser;

    [SetUp]
    public void SetUp ()
    {
      _typeParser = new TypeParser();
    }

    [Test]
    [Category ("Attribute")]
    public void Parse_ParameterSampleType_NoProblem ()
    {
      TypeNode sample = Helper.TypeNodeFactory<SampleAttribute>();
      ProblemCollection result = _typeParser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }
  }
}
