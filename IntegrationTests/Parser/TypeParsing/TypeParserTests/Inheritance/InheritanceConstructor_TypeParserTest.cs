﻿// Copyright 2012 rubicon informationstechnologie gmbh
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
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Inheritance
{
  [TestFixture]
  public class InheritanceConstructor_TypeParserTest : TypeParserTestBase
  {
    [Test]
    public void Parse_ConstructorChainingWithBaseClassCorrectFragment_NoProblem ()
    {
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleConstructor>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, ".ctor");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_ConstructorChainingWithBaseClassIncorrectFragment_ReturnsProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleConstructor>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, ".ctor", stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_ConstructorChainingWithBaseClassConstructorWithoutFragment_NoProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      TypeNode sampleTypeNode = IntrospectionUtility.TypeNodeFactory<InheritanceSampleConstructor>();
      Method sample = IntrospectionUtility.MethodFactory (sampleTypeNode, ".ctor", stringTypeNode, stringTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

  }
}
