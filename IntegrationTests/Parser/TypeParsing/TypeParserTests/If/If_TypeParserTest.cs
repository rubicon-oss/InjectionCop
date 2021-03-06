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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.If
{
  [TestFixture]
  public class If_TypeParserTest: TypeParserTestBase
  {
    [Test]
    public void Parse_ValidExampleInsideIf_NoProblem()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("ValidExampleInsideIf", 
        intTypeNode, intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_InvalidExampleInsideIf_ReturnsProblem()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("InvalidExampleInsideIf", 
        intTypeNode, intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_InvalidExampleInsideElse_ReturnsProblem()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("InvalidExampleInsideElse", 
        intTypeNode, intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_UnsafeAssignmentInsideIf_ReturnsProblem()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("UnsafeAssignmentInsideIf", 
        intTypeNode, intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_UnsafeAssignmentInsideIfTwisted_ReturnsProblem()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("UnsafeAssignmentInsideIfTwisted", 
        intTypeNode, intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_UnsafeAssignmentInsideIfNested_ReturnsProblem()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("UnsafeAssignmentInsideIfNested", 
        intTypeNode, intTypeNode, intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeAssignmentInsideIfNested_No()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("SafeAssignmentInsideIfNested", 
        intTypeNode, intTypeNode, intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_UnsafeAssignmentInsideIfNestedDeeper_ReturnsProblem()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("UnsafeAssignmentInsideIfNestedDeeper", 
        intTypeNode, intTypeNode, intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_UnsafeAssignmentInsideIfNestedElse_ReturnsProblem()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("UnsafeAssignmentInsideIfNestedElse", 
        intTypeNode, intTypeNode, intTypeNode);
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_InvalidCallInsideIfCondition_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<IfSample> ("InvalidCallInsideIfCondition");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
  }
}
