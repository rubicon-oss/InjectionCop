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
using InjectionCop.Parser;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.BlockParserTests.If
{
  [TestFixture]
  public class If_TypeParserTest: BlockParserTestBase
  {
    [Test]
    [Category("If")]
    public void Check_ValidExampleInsideIf_NoProblem()
    {
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("ValidExampleInsideIf", 
        intTypeNode, intTypeNode);
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category("If")]
    public void Check_InvalidExampleInsideIf_ReturnsProblem()
    {
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("InvalidExampleInsideIf", 
        intTypeNode, intTypeNode);
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    
    [Test]
    [Category("If")]
    public void Check_InvalidExampleInsideElse_ReturnsProblem()
    {
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("InvalidExampleInsideElse", 
        intTypeNode, intTypeNode);
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category("If")]
    public void Check_UnsafeAssignmentInsideIf_ReturnsProblem()
    {
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("UnsafeAssignmentInsideIf", 
        intTypeNode, intTypeNode);
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category("If")]
    public void Check_UnsafeAssignmentInsideIfTwisted_ReturnsProblem()
    {
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("UnsafeAssignmentInsideIfTwisted", 
        intTypeNode, intTypeNode);
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category("If")]
    public void Check_UnsafeAssignmentInsideIfNested_ReturnsProblem()
    {
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("UnsafeAssignmentInsideIfNested", 
        intTypeNode, intTypeNode, intTypeNode);
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category("If")]
    public void Check_SafeAssignmentInsideIfNested_No()
    {
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("SafeAssignmentInsideIfNested", 
        intTypeNode, intTypeNode, intTypeNode);
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }
    
    [Test]
    [Category("If")]
    public void Check_UnsafeAssignmentInsideIfNestedDeeper_ReturnsProblem()
    {
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("UnsafeAssignmentInsideIfNestedDeeper", 
        intTypeNode, intTypeNode, intTypeNode);
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category("If")]
    public void Check_UnsafeAssignmentInsideIfNestedElse_ReturnsProblem()
    {
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("UnsafeAssignmentInsideIfNestedElse", 
        intTypeNode, intTypeNode, intTypeNode);
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }
  }
}
