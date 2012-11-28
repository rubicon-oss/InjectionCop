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
  public class SymbolTableTest
  {
    private SymbolTable _symbolTable;
    private static readonly string _emptyFragment = "__EmptyFragment__";
    private static readonly string _literal = "__Literal__";

    public static string EmptyFragment
    {
      get { return _emptyFragment; }
    }

    public static string Literal
    {
      get { return _literal; }
    }

    [SetUp]
    public void SetUp ()
    {
      IBlackTypes blackTypes = new IDbCommandBlackTypesStub();
      _symbolTable = new SymbolTable (blackTypes);
    }

    [Test]
    public void ReturnsFragment_AssignmentWithLiteral_ReturnsTrue ()
    {
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithLiteral");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.Not.EqualTo(EmptyFragment));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithLiteral_ReturnsFragmentType ()
    {
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithLiteral");
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(Literal));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithLocalFragmentSetToTrue_ReturnsTrue ()
    {
      _symbolTable.MakeSafe ("local$0", "DummyType");
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithLocal");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.Not.EqualTo(EmptyFragment));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithLocalNonFragment_ReturnsFalse ()
    {
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithLocal");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithLocalFragmentSetToFalse_ReturnsFalse ()
    {
      _symbolTable.MakeUnsafe ("local$0");
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithLocal");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithLocalFragmentSetToTrue_ReturnsFragmentType ()
    {
      _symbolTable.MakeSafe ("local$0", "DummyType");
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithLocal");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo("DummyType"));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithLocalNonFragment_ReturnsNoFragmentType ()
    {
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithLocal");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithLocalFragmentSetToFalse_ReturnsNoFragmentType ()
    {
      _symbolTable.MakeUnsafe ("local$0");
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithLocal");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithParameterFragmentSetToTrue_ReturnsTrue ()
    {
      _symbolTable.MakeSafe ("parameter", "DummyType");
      TypeNode intTypeNode = IntrospectionTools.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithParameter", intTypeNode);
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.Not.EqualTo(EmptyFragment));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithParameterNonFragment_ReturnsFalse ()
    {
      TypeNode intTypeNode = IntrospectionTools.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithParameter", intTypeNode);
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithParameterFragmentSetToFalse_ReturnsFalse ()
    {
      _symbolTable.MakeUnsafe ("parameter");
      TypeNode intTypeNode = IntrospectionTools.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithParameter", intTypeNode);
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithParameterFragmentSetToTrue_ReturnsFragmentType ()
    {
      _symbolTable.MakeSafe ("parameter", "DummyType");
      TypeNode intTypeNode = IntrospectionTools.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithParameter", intTypeNode);
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo("DummyType"));
    }

    
    [Test]
    public void ReturnsFragment_AssignmentWithParameterNonFragment_ReturnsNoFragmentType ()
    {
      TypeNode intTypeNode = IntrospectionTools.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithParameter", intTypeNode);
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithParameterFragmentSetToFalse_ReturnsNoFragmentType ()
    {
      _symbolTable.MakeUnsafe ("parameter");
      TypeNode intTypeNode = IntrospectionTools.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithParameter", intTypeNode);
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithSafeMethodCall_ReturnsTrue ()
    {
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithSafeMethodCall");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.Not.EqualTo(EmptyFragment));
    }

    
    [Test]
    public void ReturnsFragment_AssignmentWithUnsafeMethodCall_ReturnsFalse ()
    {
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithUnsafeMethodCall");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithSafeMethodCall_ReturnsFragmentType ()
    {
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithSafeMethodCall");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo("SqlFragment"));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithUnsafeMethodCall_ReturnsEmptyFragmentType ()
    {
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithUnsafeMethodCall");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      string fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }
  }
}
