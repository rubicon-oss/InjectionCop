﻿// Copyright 2013 rubicon informationstechnologie gmbh
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
using Assert = NUnit.Framework.Assert;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.StringBuilderInference
{
  [TestFixture]
  public class StringBuilder_TypeParserTest: TypeParserTestBase
  {
    [Test]
    public void Parse_SafeStringBuilderInference_NoProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("SafeStringBuilderInference");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_UnsafeStringBuilderInference_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("UnsafeStringBuilderInference");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeStringBuilderReset_NoProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("SafeStringBuilderReset");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeStringBuilderReset_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("UnsafeStringBuilderReset");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeStringBuilderAcrossBlocks_NoProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("SafeStringBuilderAcrossBlocks");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeStringBuilderAcrossBlocks_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("UnsafeStringBuilderAcrossBlocks");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_AppendOfSafeType_NoProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("BooleanAppend");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_AppendOfUnsafeType_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("DoubleAppend");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeStringAppend_NoProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("SafeStringAppend");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeStringAppend_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("UnsafeStringAppend");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_AppendingLiteralDoesNotChangeSafeFragmentType_NoProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("AppendingLiteralDoesNotChangeSafeFragmentType");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_AppendingLiteralDoesNotChangeUnsafeFragmentType_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("AppendingLiteralDoesNotChangeUnsafeFragmentType");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_SafeStringAppendAcrossBlocks_NoProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("SafeStringAppendAcrossBlocks");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_UnsafeStringAppendAcrossBlocks_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("UnsafeStringAppendAcrossBlocks");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeCallsInDifferentBlocks_NoProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("SafeCallsInDifferentBlocks");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_StringBuilderBlockConflict_ReturnsProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("StringBuilderBlockConflict");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeFragmentTypeDefinitionAcrossBlocks_NoProblem()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample>("SafeFragmentTypeDefinitionAcrossBlocks");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_UnsafeFragmentTypeDefinitionAcrossBlocks_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample> ("UnsafeFragmentTypeDefinitionAcrossBlocks");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }

    [Test]
    public void Parse_UndefinedInitializationSetSafe_NoProblem ()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample> ("UndefinedInitializationSetSafe");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_UndefinedInitializationSetUnsafe_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample> ("UndefinedInitializationSetUnsafe");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_UnsafeAppendChain_ReturnsProblem ()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample> ("UnsafeAppendChain");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SafeStringBuilderToString_NoProblem ()
    {
      Method sample = TestHelper.GetSample<StringBuilderSample> ("SafeStringBuilderToString");
      _typeParser.Parse (sample);
      ProblemCollection result = _typeParser.Problems;

      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
  }
}
