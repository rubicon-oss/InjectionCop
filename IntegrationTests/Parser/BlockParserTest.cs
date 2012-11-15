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
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser
{
  [TestFixture]
  public class BlockParserTest
  {
    private BlockParser _blockParser;

    [SetUp]
    public void SetUp()
    {
      IBlackTypes blackList = new IDbCommandBlackTypesStub();
      _blockParser = new BlockParser (blackList, new TypeParser(blackList));
    }

    [Test]
    public void Parse_PostConditionOnlySafeSymbols ()
    {
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("PostConditionOnlySafeSymbols");
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse(sample);
      bool correctPostCondition = basicBlock.PostConditionSymbolTable.IsSafe ("local$0")
                                  && basicBlock.PostConditionSymbolTable.IsSafe ("local$1");

      Assert.That (correctPostCondition, Is.True);
    }

    [Test]
    public void Parse_PostConditionSafeAndUnsafeSymbols ()
    {
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("PostConditionSafeAndUnsafeSymbols");
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse(sample);
      bool correctPostCondition = basicBlock.PostConditionSymbolTable.IsSafe ("local$0")
                                  && basicBlock.PostConditionSymbolTable.IsNotSafe ("local$1");

      Assert.That (correctPostCondition, Is.True);
    }

    [Test]
    public void Parse_UnsafePreCondition()
    {
      TypeNode stringTypeNode = Helper.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("UnsafePreCondition", stringTypeNode);
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse(sample);
      bool correctPreCondition = basicBlock.PreConditionSafeSymbols.Contains ("unSafe");

      Assert.That (correctPreCondition, Is.True);
    }

    [Test]
    public void Parse_SafePreCondition ()
    {
      TypeNode stringTypeNode = Helper.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("SafePreCondition", stringTypeNode);
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse (sample);
      bool correctPreCondition = basicBlock.PreConditionSafeSymbols.Count == 0;

      Assert.That (correctPreCondition, Is.True);
    }
    
    [Test]
    public void Parse_MultipleUnsafePreCondition ()
    {
      TypeNode stringTypeNode = Helper.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("MultipleUnsafePreCondition", stringTypeNode, stringTypeNode);
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse (sample);
      bool correctPreCondition = basicBlock.PreConditionSafeSymbols.Contains ("unSafe1")
                                 && basicBlock.PreConditionSafeSymbols.Contains ("unSafe2");

      Assert.That (correctPreCondition, Is.True);
    }

    [Test]
    public void Parse_BlockInternalSafenessCondition_InternalSafenessSymbolNotInPreCondition ()
    {
      TypeNode stringTypeNode = Helper.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("BlockInternalSafenessCondition", stringTypeNode);
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse (sample);
      bool correctPreCondition = basicBlock.PreConditionSafeSymbols.Contains ("x")
                                 && basicBlock.PreConditionSafeSymbols.Count == 1;

      Assert.That (correctPreCondition, Is.True);
    }

    [Test]
    public void Parse_SetSuccessor ()
    {
      TypeNode stringTypeNode = Helper.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("SetSuccessor", stringTypeNode);
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse (sample);
      bool correctPreCondition = (basicBlock.SuccessorKeys.Length == 1);

      Assert.That (correctPreCondition, Is.True);
    }
  }
}
