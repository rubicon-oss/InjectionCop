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
using System.Collections.Generic;
using InjectionCop.Config;
using InjectionCop.Parser;
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.BlockParsing
{
  [TestFixture]
  public class BlockParserTest
  {
    private const string c_returnFragmentType = "ReturnFragmentType";

    private BlockParser _blockParser;
    private ProblemPipeStub _problemPipeStub;
    private IBlacklistManager _blacklist;
    private PreCondition _returnPreCondition;

    [SetUp]
    public void SetUp()
    {
      _blacklist = new IDbCommandBlacklistManagerStub();
      _problemPipeStub = new ProblemPipeStub();
      _returnPreCondition = new PreCondition ("returnPreCondition", "ReturnPreConditionFragmentType");
      List<PreCondition> returnPreConditions = new List<PreCondition> { _returnPreCondition };
      _blockParser = new BlockParser (_blacklist, _problemPipeStub, c_returnFragmentType, returnPreConditions);
    }

    [Test]
    public void Parse_PostConditionOnlySafeSymbols ()
    {
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("PostConditionOnlySafeSymbols");
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse(sample);
      bool correctPostCondition = basicBlock.PostConditionSymbolTable.IsAssignableTo ("local$0", "SqlFragment")
                                  && basicBlock.PostConditionSymbolTable.IsAssignableTo ("local$1", "SqlFragment");

      Assert.That (correctPostCondition, Is.True);
    }

    [Test]
    public void Parse_PostConditionSafeAndUnsafeSymbols ()
    {
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("PostConditionSafeAndUnsafeSymbols");
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse(sample);
      bool correctPostCondition = basicBlock.PostConditionSymbolTable.IsAssignableTo ("local$0", "SqlFragment")
                                  && !basicBlock.PostConditionSymbolTable.IsAssignableTo ("local$1", "SqlFragment");

      Assert.That (correctPostCondition, Is.True);
    }
    
    [Test]
    public void Parse_UnsafePreCondition()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("UnsafePreCondition", stringTypeNode);
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse(sample);
      bool correctPreCondition = basicBlock.PreConditions[0].Symbol == "unSafe"
                                 && basicBlock.PreConditions[0].FragmentType == "SqlFragment";

      Assert.That (correctPreCondition, Is.True);
    }

    [Test]
    public void Parse_SafePreCondition_OnlyReturnConditionCreated ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("SafePreCondition", stringTypeNode);
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse (sample);
      bool correctPreCondition = basicBlock.PreConditions.Length == 1;

      Assert.That (correctPreCondition, Is.True);
    }
    
    
    [Test]
    public void Parse_MultipleUnsafePreCondition ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
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
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
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
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("SetSuccessor", stringTypeNode);
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock basicBlock = _blockParser.Parse (sample);
      bool correctPreCondition = (basicBlock.SuccessorKeys.Length == 1);

      Assert.That (correctPreCondition, Is.True);
    }

    [Test]
    public void Parse_ReturnFragmentRequiredLiteralReturn_NoProblem ()
    {
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ReturnFragmentRequiredLiteralReturn");
      Block sample = sampleMethod.Body.Statements[1] as Block;
      _blockParser.Parse (sample);
      bool noProblemsArised = _problemPipeStub.Problems.Count == 0;

      Assert.That (noProblemsArised, Is.True);
    }

    [Test]
    public void Parse_UnsafeReturnWhenFragmentRequired_ReturnsLocalVariableThatGetsReturned ()
    {
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("UnsafeReturnWhenFragmentRequired");
      Block sample = sampleMethod.Body.Statements[1] as Block;
      BasicBlock returnedBlock = _blockParser.Parse (sample);
      string preConditionSymbol = returnedBlock.PreConditions[0].Symbol;
      
      Assert.That (preConditionSymbol, Is.EqualTo ("local$0"));
    }

    [Test]
    public void Parse_UnsafeReturnWhenFragmentRequiredMoreComplex_ReturnsLocalVariableThatGetsReturned ()
    {
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("UnsafeReturnWhenFragmentRequiredMoreComplex");
      Block sample = sampleMethod.Body.Statements[1] as Block;
      BasicBlock returnedBlock = _blockParser.Parse (sample);
      string preConditionSymbol = returnedBlock.PreConditions[0].Symbol;
      
      Assert.That (preConditionSymbol, Is.EqualTo ("local$2"));
    }

    [Test]
    public void Parse_ReturnFragmentRequiredUnsafeReturn_ReturnsCorrectReturnFragmentType ([Values("ReturnFragmentType1", "ReturnFragmentType2")] string returnFragmentType)
    {
      _blockParser = new BlockParser (_blacklist, _problemPipeStub, returnFragmentType, new List<PreCondition>());
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("UnsafeReturnWhenFragmentRequired");
      Block sample = sampleMethod.Body.Statements[1] as Block;
      BasicBlock returnedBlock = _blockParser.Parse (sample);
      string preConditionFragment = returnedBlock.PreConditions[0].FragmentType;
      
      Assert.That (preConditionFragment, Is.EqualTo (returnFragmentType));
    }

    [Test]
    public void Parse_BlockWithReturnNodeNotReturningAnything_OnlyReturnConditionCreated ()
    {
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("DummyProcedure");
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock returnedBlock = _blockParser.Parse (sample);
      int countPreconditions = returnedBlock.PreConditions.Length;

      Assert.That (countPreconditions, Is.EqualTo (1));
    }

    [Test]
    public void Parse_BlockWithoutReturnNode_NoPreconditionCreated ()
    {
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("UnsafeReturnWhenFragmentRequired");
      Block sample = sampleMethod.Body.Statements[0] as Block;
      BasicBlock returnedBlock = _blockParser.Parse (sample);
      int countPreconditions = returnedBlock.PreConditions.Length;

      Assert.That (countPreconditions, Is.EqualTo (0));
    }

    [Test]
    public void Parse_ValidReturnWithIf_ReturnsLocalVariableThatGetsReturned ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ValidReturnWithLiteralAssignmentInsideIf", stringTypeNode);
      Block sample = sampleMethod.Body.Statements[3] as Block;
      BasicBlock returnedBlock = _blockParser.Parse (sample);
      string preConditionSymbol = returnedBlock.PreConditions[0].Symbol;
      
      Assert.That (preConditionSymbol, Is.EqualTo ("local$1"));
    }

    [Test]
    public void Parse_ValidReturnWithIf_ReturnsCorrectReturnFragmentType ()
    {
      _blockParser = new BlockParser (_blacklist, _problemPipeStub, "DummyFragment", new List<PreCondition>());
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ValidReturnWithLiteralAssignmentInsideIf", stringTypeNode);
      Block sample = sampleMethod.Body.Statements[3] as Block;
      BasicBlock returnedBlock = _blockParser.Parse (sample);
      string preConditionFragmentType = returnedBlock.PreConditions[0].FragmentType;
      
      Assert.That (preConditionFragmentType, Is.EqualTo ("DummyFragment"));
    }

    [Test]
    public void MethodGraph_ValidReturnWithIfBlockWithLocalAssignment_ReturnsLocalAssignment ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ValidReturnWithLiteralAssignmentInsideIf", stringTypeNode);
      Block preReturnBlock = sampleMethod.Body.Statements[2] as Block;
      BasicBlock preReturnBasicBlock = _blockParser.Parse (preReturnBlock);
      
      Assert.That (preReturnBasicBlock.BlockAssignments.Length, Is.EqualTo(1));
    }

    [Test]
    public void MethodGraph_ValidReturnWithIfBlockWithLocalAssignment_ReturnsCorrectLocalAssignment ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ValidReturnWithLiteralAssignmentInsideIf", stringTypeNode);
      Block preReturnBlock = sampleMethod.Body.Statements[2] as Block;
      BasicBlock preReturnBasicBlock = _blockParser.Parse (preReturnBlock);
      string source = preReturnBasicBlock.BlockAssignments[0].SourceSymbol;
      string target = preReturnBasicBlock.BlockAssignments[0].TargetSymbol;

      bool correctLocalAssignment = source == "local$0" && target == "local$1";
      Assert.That (correctLocalAssignment, Is.True);
    }

    [Test]
    public void MethodGraph_ValidReturnWithIfBlockWithoutLocalAssignment_ReturnsNoLocalAssignment ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ValidReturnWithLiteralAssignmentInsideIf", stringTypeNode);
      Block ifBlock = sampleMethod.Body.Statements[1] as Block;
      BasicBlock ifBasicBlock = _blockParser.Parse (ifBlock);
      
      Assert.That (ifBasicBlock.BlockAssignments.Length, Is.EqualTo(0));
    }

    [Test]
    public void MethodGraph_ValidReturnWithIfBlockWithoutLocalAssignment_ReturnsCorrectPostCondition ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ValidReturnWithLiteralAssignmentInsideIf", stringTypeNode);
      Block ifBlock = sampleMethod.Body.Statements[1] as Block;
      BasicBlock ifBasicBlock = _blockParser.Parse (ifBlock);
      string postConditionFragmentType = ifBasicBlock.PostConditionSymbolTable.GetFragmentType("local$0");

      Assert.That (postConditionFragmentType, Is.EqualTo("__Literal__"));
    }

    [Test]
    public void Parse_DeclarationWithReturn_ReturnsCorrectPostConditions ()
    {
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("DeclarationWithReturn");
      Block initialBlock = sampleMethod.Body.Statements[0] as Block;
      BasicBlock initialBasicBlock = _blockParser.Parse (initialBlock);
      string local0FragmentType = initialBasicBlock.PostConditionSymbolTable.GetFragmentType ("local$0");
      string local1FragmentType = initialBasicBlock.PostConditionSymbolTable.GetFragmentType ("local$1");
      bool correctPostConditions = local0FragmentType == local1FragmentType && local0FragmentType == "__Literal__";

      Assert.That (correctPostConditions, Is.True);
    }

    [Test]
    public void Parse_ValidReturnWithParameterAssignmentInsideIf_ReturnsLocalAssignment ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ValidReturnWithParameterAssignmentInsideIf", stringTypeNode);
      Block ifBlock = sampleMethod.Body.Statements[1] as Block;
      BasicBlock ifBasicBlock = _blockParser.Parse (ifBlock);
      
      Assert.That (ifBasicBlock.BlockAssignments.Length, Is.EqualTo(1));
    }

    [Test]
    public void Parse_ValidReturnWithParameterAssignmentInsideIf_ReturnsCorrectPostCondition ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ValidReturnWithParameterAssignmentInsideIf", stringTypeNode);
      Block ifBlock = sampleMethod.Body.Statements[1] as Block;
      BasicBlock ifBasicBlock = _blockParser.Parse (ifBlock);
      string postConditionFragmentType = ifBasicBlock.PostConditionSymbolTable.GetFragmentType("local$0");

      Assert.That (postConditionFragmentType, Is.EqualTo(SymbolTable.EMPTY_FRAGMENT));
    }

      [Test]
    public void Parse_ValidReturnWithParameterResetAndAssignmentInsideIf_ReturnsNoLocalAssignment ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ValidReturnWithParameterResetAndAssignmentInsideIf", stringTypeNode);
      Block ifBlock = sampleMethod.Body.Statements[1] as Block;
      BasicBlock ifBasicBlock = _blockParser.Parse (ifBlock);
      
      Assert.That (ifBasicBlock.BlockAssignments.Length, Is.EqualTo(0));
    }

    [Test]
    public void Parse_ValidReturnWithParameterResetAndAssignmentInsideIf_ReturnsCorrectPostCondition ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ValidReturnWithParameterResetAndAssignmentInsideIf", stringTypeNode);
      Block ifBlock = sampleMethod.Body.Statements[1] as Block;
      BasicBlock ifBasicBlock = _blockParser.Parse (ifBlock);
      string postConditionFragmentType = ifBasicBlock.PostConditionSymbolTable.GetFragmentType("local$0");

      Assert.That (postConditionFragmentType, Is.EqualTo("__Literal__"));
    }

    [Test]
    public void Parse_ReturnLiteral_ReturnConditionIsAddedToReturnBlock ()
    {
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ReturnLiteral");
      Block returnBlock = sampleMethod.Body.Statements[1] as Block;
      BasicBlock returnBasicBlock = _blockParser.Parse (returnBlock);
      string preConditionSymbol = returnBasicBlock.PreConditions[1].Symbol;
      string preConditionFragmentType = returnBasicBlock.PreConditions[1].FragmentType;
      bool correctPrecondition = preConditionSymbol == _returnPreCondition.Symbol
                                 && preConditionFragmentType == _returnPreCondition.FragmentType;

      Assert.That (correctPrecondition, Is.True);
    }

    [Test]
    public void Parse_ReturnPreconditionCheckSafe_NoProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ReturnPreconditionCheckSafe", stringTypeNode.GetReferenceType());
      Block sample = sampleMethod.Body.Statements[0] as Block;
      _blockParser.Parse (sample);
      bool noProblemsArised = _problemPipeStub.Problems.Count == 0;

      Assert.That (noProblemsArised, Is.True);
    }

    [Test]
    public void Parse_ReturnPreconditionCheckUnSafe_ReturnsProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ReturnPreconditionCheckUnSafe", stringTypeNode.GetReferenceType());
      Block sample = sampleMethod.Body.Statements[0] as Block;
      _blockParser.Parse (sample);
      bool problemFound = _problemPipeStub.Problems.Count != 0;

      Assert.That (problemFound, Is.True);
    }

    [Test]
    public void Parse_ReturnPreconditionCheckSafeLiteralAssignment_NoProblem ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ReturnPreconditionCheckSafeLiteralAssignment", stringTypeNode.GetReferenceType());
      Block sample = sampleMethod.Body.Statements[0] as Block;
      _blockParser.Parse (sample);
      bool noProblemsArised = _problemPipeStub.Problems.Count == 0;

      Assert.That (noProblemsArised, Is.True);
    }

    [Test]
    public void Parse_ReturnPreconditionConditional_ReturnConditionIsAddedToReturnBlock ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ReturnPreconditionConditional", stringTypeNode.GetReferenceType());
      Block returnBlock = sampleMethod.Body.Statements[2] as Block;
      BasicBlock returnBasicBlock = _blockParser.Parse (returnBlock);
      string preConditionSymbol = returnBasicBlock.PreConditions[0].Symbol;
      string preConditionFragmentType = returnBasicBlock.PreConditions[0].FragmentType;
      bool correctPrecondition = preConditionSymbol == _returnPreCondition.Symbol
                                 && preConditionFragmentType == _returnPreCondition.FragmentType;

      Assert.That (correctPrecondition, Is.True);
    }

    [Test]
    public void Parse_ReturnPreconditionConditionalWithReturnInsideIf_ReturnConditionIsAddedToReturnBlock ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ReturnPreconditionConditionalWithReturnInsideIf", stringTypeNode.GetReferenceType());
      Block returnBlock = sampleMethod.Body.Statements[3] as Block;
      BasicBlock returnBasicBlock = _blockParser.Parse (returnBlock);
      string preConditionSymbol = returnBasicBlock.PreConditions[0].Symbol;
      string preConditionFragmentType = returnBasicBlock.PreConditions[0].FragmentType;
      bool correctPrecondition = preConditionSymbol == _returnPreCondition.Symbol
                                 && preConditionFragmentType == _returnPreCondition.FragmentType;

      Assert.That (correctPrecondition, Is.True);
    }

    [Test]
    public void Parse_ReturnPreconditionConditionalWithReturnAfterIf_ReturnConditionIsAddedToReturnBlock ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<BlockParserSample> ("ReturnPreconditionConditionalWithReturnAfterIf", stringTypeNode.GetReferenceType());
      Block returnBlock = sampleMethod.Body.Statements[2] as Block;
      BasicBlock returnBasicBlock = _blockParser.Parse (returnBlock);
      string preConditionSymbol = returnBasicBlock.PreConditions[0].Symbol;
      string preConditionFragmentType = returnBasicBlock.PreConditions[0].FragmentType;
      bool correctPrecondition = preConditionSymbol == _returnPreCondition.Symbol
                                 && preConditionFragmentType == _returnPreCondition.FragmentType;

      Assert.That (correctPrecondition, Is.True);
    }
  }
}
