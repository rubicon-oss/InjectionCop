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

namespace InjectionCop.IntegrationTests.Parser.If
{
  class TypeParserTest_If: TypeParserTest
  {
    [Test]
    [Category("If")]
    public void Check_ValidExampleInsideIf_NoProblem()
    {
      Method sample = TestHelper.GetSample<IfSample>("ValidExampleInsideIf");
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    [Category("If")]
    public void Check_InvalidExampleInsideIf_ReturnsProblem()
    {
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("InvalidExampleInsideIf", intTypeNode, intTypeNode);
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    
    [Test]
    [Category("If")]
    public void Check_InvalidExampleInsideElse_ReturnsProblem()
    {
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("InvalidExampleInsideElse", intTypeNode, intTypeNode);
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    [Category("If")]
    public void Check_UnsafeAssignmentInsideIf_ReturnsProblem()
    {
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<IfSample>("UnsafeAssignmentInsideIf", intTypeNode, intTypeNode);
      ProblemCollection result = parser.Check (sample);

      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }
  }
}
