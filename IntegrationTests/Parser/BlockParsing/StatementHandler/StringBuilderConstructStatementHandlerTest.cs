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
using InjectionCop.Parser;
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Parser.BlockParsing.PreCondition;
using InjectionCop.Parser.BlockParsing.StatementHandler;
using InjectionCop.Parser.BlockParsing.StatementHandler.AssignmentStatementHandler;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;
using Rhino.Mocks;

namespace InjectionCop.IntegrationTests.Parser.BlockParsing.StatementHandler
{
  [TestFixture]
  public class StringBuilderConstructStatementHandlerTest
  {
    private StringBuilderConstructStatementHandler _handler;
    private Dictionary<string, bool> _stringBuilderFragmentTypesDefined;
    private ISymbolTable _symbolTable;

    [SetUp]
    public void SetUp ()
    {
      MockRepository mocks = new MockRepository();
      IBlacklistManager blacklistManager = mocks.Stub<IBlacklistManager>();
      BlockParserContext blockParserContext = new BlockParserContext (
          new ProblemPipeStub(),
          Fragment.CreateNamed ("returnFragmentType"),
          new List<ReturnCondition>(),
          blacklistManager,
          delegate { });
      
      _handler = new StringBuilderConstructStatementHandler (blockParserContext);
      _symbolTable = new SymbolTable (blacklistManager);
      _stringBuilderFragmentTypesDefined = new Dictionary<string, bool>();
    }

    [Test]
    public void HandleStatement_NonStringBuilderAssignment_VariableNotMapped ()
    {
      Method sampleMethod = IntrospectionUtility.MethodFactory<StringBuilderConstructStatementHandlerSample> ("NonStringBuilderAssignment");
      Block sampleBlock = (Block) sampleMethod.Body.Statements[0];
      Statement sample = sampleBlock.Statements[1];
      
      HandleContext context = new HandleContext (
          sample,
          _symbolTable,
          new List<IPreCondition>(),
          new List<string>(),
          new List<BlockAssignment>(),
          new List<int>(),
          new Dictionary<string, bool>(),
          _stringBuilderFragmentTypesDefined);
      
      _handler.Handle (context);

      bool variableAdded = _stringBuilderFragmentTypesDefined.ContainsKey ("local$0");
      Assert.That (variableAdded, Is.False);
    }

    [Test]
    public void HandleStatement_InitializationWithEmptyConstructor_VariableFragmentTypeIsNull ()
    {
      Method sampleMethod = IntrospectionUtility.MethodFactory<StringBuilderConstructStatementHandlerSample> ("InitializationWithEmptyConstructor");
      Block sampleBlock = (Block) sampleMethod.Body.Statements[0];
      Statement sample = sampleBlock.Statements[1];
      
      HandleContext context = new HandleContext (
          sample,
          _symbolTable,
          new List<IPreCondition>(),
          new List<string>(),
          new List<BlockAssignment>(),
          new List<int>(),
          new Dictionary<string, bool>(),
          _stringBuilderFragmentTypesDefined);
      
      _handler.Handle (context);

      bool mappingCorrect = _stringBuilderFragmentTypesDefined.ContainsKey ("local$0") && _stringBuilderFragmentTypesDefined["local$0"] == false;
      Assert.That (mappingCorrect, Is.True);
    }
    
    [Test]
    public void HandleStatement_InitializationWithCapacity_VariableFragmentTypeIsNull ()
    {
      Method sampleMethod = IntrospectionUtility.MethodFactory<StringBuilderConstructStatementHandlerSample> ("InitializationWithCapacity");
      Block sampleBlock = (Block) sampleMethod.Body.Statements[0];
      Statement sample = sampleBlock.Statements[1];
      
      HandleContext context = new HandleContext (
          sample,
          _symbolTable,
          new List<IPreCondition>(),
          new List<string>(),
          new List<BlockAssignment>(),
          new List<int>(),
          new Dictionary<string, bool>(),
          _stringBuilderFragmentTypesDefined);
      
      _handler.Handle (context);

      bool mappingCorrect = _stringBuilderFragmentTypesDefined.ContainsKey ("local$0") && _stringBuilderFragmentTypesDefined["local$0"] == false;
      Assert.That (mappingCorrect, Is.True);
    }
    
    [Test]
    public void HandleStatement_InitializationWithCapacityAndMaximum_VariableFragmentTypeIsNull ()
    {
      Method sampleMethod = IntrospectionUtility.MethodFactory<StringBuilderConstructStatementHandlerSample> ("InitializationWithCapacityAndMaximum");
      Block sampleBlock = (Block) sampleMethod.Body.Statements[0];
      Statement sample = sampleBlock.Statements[1];
      
      HandleContext context = new HandleContext (
          sample,
          _symbolTable,
          new List<IPreCondition>(),
          new List<string>(),
          new List<BlockAssignment>(),
          new List<int>(),
          new Dictionary<string, bool>(),
          _stringBuilderFragmentTypesDefined);
      
      _handler.Handle (context);

      bool mappingCorrect = _stringBuilderFragmentTypesDefined.ContainsKey ("local$0") && _stringBuilderFragmentTypesDefined["local$0"] == false;
      Assert.That (mappingCorrect, Is.True);
    }
    
    [Test]
    public void HandleStatement_InitializationWithLiteral_VariableFragmentTypeIsLiteral ()
    {
      Method sampleMethod = IntrospectionUtility.MethodFactory<StringBuilderConstructStatementHandlerSample> ("InitializationWithLiteral");
      Block sampleBlock = (Block) sampleMethod.Body.Statements[0];
      Statement sample = sampleBlock.Statements[1];
      
      HandleContext context = new HandleContext (
          sample,
          _symbolTable,
          new List<IPreCondition>(),
          new List<string>(),
          new List<BlockAssignment>(),
          new List<int>(),
          new Dictionary<string, bool>(),
          _stringBuilderFragmentTypesDefined);
      
      _handler.Handle (context);

      bool symbolTableEntryCorrect = _symbolTable.GetFragmentType("local$0") == Fragment.CreateLiteral();
      Assert.That (symbolTableEntryCorrect, Is.True);
    }
    
    [Test]
    public void HandleStatement_InitializationWithFragment_VariableFragmentTypeIsFragment ()
    {
      Method sampleMethod = IntrospectionUtility.MethodFactory<StringBuilderConstructStatementHandlerSample> ("InitializationWithFragment");
      Block sampleBlock = (Block) sampleMethod.Body.Statements[0];
      Statement sample = sampleBlock.Statements[1];
      
      HandleContext context = new HandleContext (
          sample,
          _symbolTable,
          new List<IPreCondition>(),
          new List<string>(),
          new List<BlockAssignment>(),
          new List<int>(),
          new Dictionary<string, bool>(),
          _stringBuilderFragmentTypesDefined);
      
      _handler.Handle (context);

      bool symbolTableEntryCorrect = _symbolTable.GetFragmentType("local$0") == Fragment.CreateNamed("StringBuilderFragment");
      Assert.That (symbolTableEntryCorrect, Is.True);
    }
    
    [Test]
    public void HandleStatement_InitializationWithEmptyFragment_VariableFragmentTypeIsEmptyFragment ()
    {
      Method sampleMethod = IntrospectionUtility.MethodFactory<StringBuilderConstructStatementHandlerSample> ("InitializationWithEmptyFragment");
      Block sampleBlock = (Block) sampleMethod.Body.Statements[0];
      Statement sample = sampleBlock.Statements[1];
      
      HandleContext context = new HandleContext (
          sample,
          _symbolTable,
          new List<IPreCondition>(),
          new List<string>(),
          new List<BlockAssignment>(),
          new List<int>(),
          new Dictionary<string, bool>(),
          _stringBuilderFragmentTypesDefined);
      
      _handler.Handle (context);

      bool symbolTableEntryCorrect = _symbolTable.GetFragmentType("local$0") == Fragment.CreateEmpty();
      Assert.That (symbolTableEntryCorrect, Is.True);
    }
    
    [Test]
    public void HandleStatement_InitializationWithLiteralAndInt_VariableFragmentTypeIsLiteral ()
    {
      Method sampleMethod = IntrospectionUtility.MethodFactory<StringBuilderConstructStatementHandlerSample> ("InitializationWithLiteralAndInt");
      Block sampleBlock = (Block) sampleMethod.Body.Statements[0];
      Statement sample = sampleBlock.Statements[1];
      
      HandleContext context = new HandleContext (
          sample,
          _symbolTable,
          new List<IPreCondition>(),
          new List<string>(),
          new List<BlockAssignment>(),
          new List<int>(),
          new Dictionary<string, bool>(),
          _stringBuilderFragmentTypesDefined);
      
      _handler.Handle (context);

      bool symbolTableEntryCorrect = _symbolTable.GetFragmentType("local$0") == Fragment.CreateLiteral();
      Assert.That (symbolTableEntryCorrect, Is.True);
    }
    
    [Test]
    public void HandleStatement_InitializationWithFragmentAndInts_VariableFragmentTypeIsFragment ()
    {
      Method sampleMethod = IntrospectionUtility.MethodFactory<StringBuilderConstructStatementHandlerSample> ("InitializationWithFragmentAndInts");
      Block sampleBlock = (Block) sampleMethod.Body.Statements[0];
      Statement sample = sampleBlock.Statements[1];
      
      HandleContext context = new HandleContext (
          sample,
          _symbolTable,
          new List<IPreCondition>(),
          new List<string>(),
          new List<BlockAssignment>(),
          new List<int>(),
          new Dictionary<string, bool>(),
          _stringBuilderFragmentTypesDefined);
      
      _handler.Handle (context);

      bool symbolTableEntryCorrect = _symbolTable.GetFragmentType("local$0") == Fragment.CreateNamed("StringBuilderFragment");
      Assert.That (symbolTableEntryCorrect, Is.True);
    }

  }
}
