// Copyright 2013 rubicon informationstechnologie gmbh
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
using NUnit.Framework;
using Microsoft.FxCop.Sdk;
using InjectionCop.Utilities;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.FragmentGenerator
{
  [TestFixture]
  public class FragmentGenerator_TypeParserTest: TypeParserTestBase
  {
    [Test]
    public void Check_FragmentGeneratorSample_ReturnsProblem()
    {
      TypeNode sample = IntrospectionUtility.TypeNodeFactory<FragmentGeneratorSample>();
      _typeParser.Check(sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That(TestHelper.ContainsProblemID(c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Check_FragmentGeneratorAnnotationTest_NoProblem()
    {
      TypeNode sample = IntrospectionUtility.TypeNodeFactory<FragmentGeneratorAnnotationTest>();
      _typeParser.Check(sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That(TestHelper.ContainsProblemID(c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Check_CustomFragmentGeneratorAnnotationTest_NoProblem()
    {
      TypeNode sample = IntrospectionUtility.TypeNodeFactory<CustomFragmentGeneratorAnnotationTest>();
      _typeParser.Check(sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That(TestHelper.ContainsProblemID(c_InjectionCopRuleId, result), Is.False);
    }
  }
}
