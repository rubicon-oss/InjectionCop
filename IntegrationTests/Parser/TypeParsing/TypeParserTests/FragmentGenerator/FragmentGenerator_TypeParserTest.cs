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
using System.Collections.Generic;
using InjectionCop.Config;
using InjectionCop.Parser.TypeParsing;
using NUnit.Framework;
using Microsoft.FxCop.Sdk;
using InjectionCop.Utilities;
using Rhino.Mocks;
using Is = NUnit.Framework.Is;

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

    [Test]
    public void Check_CallsBlackLisManagerToCheckForFragmentGenerator ()
    {
      TypeNode sample = IntrospectionUtility.TypeNodeFactory<FragmentGeneratorSample>();
      var blacklistManager = MockRepository.GenerateStub<IBlacklistManager>();
      const bool isGenerator = true;

      blacklistManager.Stub (
          manager =>
              manager.GetFragmentTypes (
                  Arg<string>.Is.Anything,
                  Arg<string>.Is.Anything,
                  Arg<string>.Matches (_ => _ != "UnsafeWithoutGenerator"),
                  Arg<IList<string>>.Is.Anything)).Return (new FragmentSignature (new string[0], "", isGenerator));

      blacklistManager.Stub (_ => _.GetFragmentTypes (sample.DeclaringModule.Name, sample.FullName, "UnsafeWithoutGenerator", new string[0]))
          .Return (new FragmentSignature (new string[0], "", isGenerator));

      _typeParser = new TypeParser (blacklistManager);
      _typeParser.BeforeAnalysis();
      _typeParser.Check (sample);

      Assert.That(TestHelper.ContainsProblemID(c_InjectionCopRuleId, _typeParser.Problems), Is.False);
    }

    [Test]
    public void Check_BlackListDoesNotContainMethod_RaisesProblem()
    {
      TypeNode sample = IntrospectionUtility.TypeNodeFactory<FragmentGeneratorSample>();
      var blacklistManager = MockRepository.GenerateStub<IBlacklistManager>();
      const bool isGenerator = true;

      blacklistManager.Stub(
          manager =>
              manager.GetFragmentTypes(
                  Arg<string>.Is.Anything,
                  Arg<string>.Is.Anything,
                  Arg<string>.Matches(_ => _ != "UnsafeWithoutGenerator"),
                  Arg<IList<string>>.Is.Anything)).Return(new FragmentSignature(new string[0], "", isGenerator));

      blacklistManager.Stub(_ => _.GetFragmentTypes(sample.DeclaringModule.Name, sample.FullName, "UnsafeWithoutGenerator", new string[0]))
          .Return(null);

      _typeParser = new TypeParser(blacklistManager);
      _typeParser.BeforeAnalysis();
      _typeParser.Check(sample);

      Assert.That(TestHelper.ContainsProblemID(c_InjectionCopRuleId, _typeParser.Problems), Is.True);
    }

  }
}
