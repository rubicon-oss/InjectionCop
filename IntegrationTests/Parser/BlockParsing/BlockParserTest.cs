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
using InjectionCop.Config;
using InjectionCop.Parser;
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Parser.TypeParsing;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.BlockParsing
{
  [TestFixture]
  public class BlockParserTest
  {
    private BlockParser _blockParser;

    [SetUp]
    public void SetUp()
    {
      IBlacklistManager blackList = new IDbCommandBlacklistManagerStub();
      _blockParser = new BlockParser (blackList, new TypeParser());
    }

    [Test]
    public void Parse_PostConditionOnlySafeSymbols ()
    {
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("PostConditionOnlySafeSymbols");
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse(sample);
      bool correctPostCondition = basicBlock.PostConditionSymbolTable.IsFragment ("local$0", "SqlFragment")
                                  && basicBlock.PostConditionSymbolTable.IsFragment ("local$1", "SqlFragment");

      Assert.That (correctPostCondition, Is.True);
    }

    [Test]
    public void Parse_PostConditionSafeAndUnsafeSymbols ()
    {
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("PostConditionSafeAndUnsafeSymbols");
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse(sample);
      bool correctPostCondition = basicBlock.PostConditionSymbolTable.IsFragment ("local$0", "SqlFragment")
                                  && !basicBlock.PostConditionSymbolTable.IsFragment ("local$1", "SqlFragment");

      Assert.That (correctPostCondition, Is.True);
    }
    
    [Test]
    public void Parse_UnsafePreCondition()
    {
      TypeNode stringTypeNode = IntrospectionTools.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("UnsafePreCondition", stringTypeNode);
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse(sample);
      bool correctPreCondition = basicBlock.PreConditions[0].Symbol == "unSafe"
                                 && basicBlock.PreConditions[0].FragmentType == "SqlFragment";

      Assert.That (correctPreCondition, Is.True);
    }

    [Test]
    public void Parse_SafePreCondition ()
    {
      TypeNode stringTypeNode = IntrospectionTools.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("SafePreCondition", stringTypeNode);
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse (sample);
      bool correctPreCondition = basicBlock.PreConditions.Length == 0;

      Assert.That (correctPreCondition, Is.True);
    }
    
    
    [Test]
    public void Parse_MultipleUnsafePreCondition ()
    {
      TypeNode stringTypeNode = IntrospectionTools.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("MultipleUnsafePreCondition", stringTypeNode, stringTypeNode);
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse (sample);
      
      bool correctPreCondition0 = basicBlock.PreConditions[0].Symbol == "unSafe1"
                                 && basicBlock.PreConditions[0].FragmentType == "SqlFragment";

      bool correctPreCondition1 = basicBlock.PreConditions[1].Symbol == "unSafe2"
                                 && basicBlock.PreConditions[1].FragmentType == "SqlFragment";

      Assert.That (correctPreCondition0 && correctPreCondition1, Is.True);
    }

    
    [Test]
    public void Parse_BlockInternalSafenessCondition_InternalSafenessSymbolNotInPreCondition ()
    {
      TypeNode stringTypeNode = IntrospectionTools.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("BlockInternalSafenessCondition", stringTypeNode);
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse (sample);
      
      bool correctPreCondition = basicBlock.PreConditions[0].Symbol == "x"
                                 && basicBlock.PreConditions[0].FragmentType == "SqlFragment";

      Assert.That (correctPreCondition, Is.True);
    }

    [Test]
    public void Parse_SetSuccessor ()
    {
      TypeNode stringTypeNode = IntrospectionTools.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("SetSuccessor", stringTypeNode);
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse (sample);
      bool correctPreCondition = (basicBlock.SuccessorKeys.Length == 1);

      Assert.That (correctPreCondition, Is.True);
    }
  }
}
