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
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Indexer
{
  [TestFixture]
  public class Indexer_TypeParserTest: TypeParserTestBase
  {
    [Test]
    public void Parse_IndexerSafeAssignmentOnParameter_NoProblem()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<IndexerSample>("IndexerSafeAssignmentOnParameter", stringTypeNode.GetArrayType(1));
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_IndexerUnsafeAssignmentOnParameter_ReturnsProblem()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<IndexerSample>("IndexerUnsafeAssignmentOnParameter", stringTypeNode.GetArrayType(1));
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeCallUsingIndexer_NoProblem()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<IndexerSample>("SafeCallUsingIndexer", stringTypeNode.GetArrayType(1));
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeCallUsingIndexer_ReturnsProblem()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<IndexerSample>("UnsafeCallUsingIndexer", stringTypeNode.GetArrayType(1));
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_UnsafeCallWithElementSetUnsafeByIndexer_ReturnsProblem()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<IndexerSample>("UnsafeCallWithElementSetUnsafeByIndexer", stringTypeNode.GetArrayType(1));
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_UnsafeCallWithSingleVariableSetSafeByIndexer_ReturnsProblem()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<IndexerSample>("UnsafeCallWithSingleVariableSetSafeByIndexer", stringTypeNode.GetArrayType(1));
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeCallUsingArray_NoProblem()
    {
      Method sample = TestHelper.GetSample<IndexerSample>("SafeCallUsingArray");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_SafeLocallyInitializedArray_NoProblem()
    {
      Method sample = TestHelper.GetSample<IndexerSample>("SafeLocallyInitializedArray");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_UnsafeLocallyInitializedArray_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<IndexerSample>("UnsafeLocallyInitializedArray");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_UnsafeLocallyInitializedArrayFirstIndexerAssignmentUnsafe_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<IndexerSample>("UnsafeLocallyInitializedArrayFirstIndexerAssignmentUnsafe");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeLocallyInitializedArrayEqualFragmentTypes_NoProblem()
    {
      Method sample = TestHelper.GetSample<IndexerSample>("SafeLocallyInitializedArrayEqualFragmentTypes");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeLocallyInitializedArrayDifferentFragmentTypes_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<IndexerSample>("UnsafeLocallyInitializedArrayDifferentFragmentTypes");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeLocallyInitializedBiggerArray_NoProblem()
    {
      Method sample = TestHelper.GetSample<IndexerSample>("SafeLocallyInitializedBiggerArray");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
  }
}
