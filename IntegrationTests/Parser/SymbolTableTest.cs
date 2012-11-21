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
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      bool returnsFragment = _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(returnsFragment, Is.True);
    }

    [Test]
    public void ReturnsFragment_AssignmentWithLiteral_ReturnsFragmentType ()
    {
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithLiteral");
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(fragmentType, Is.EqualTo("Literal"));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithLocalFragmentSetToTrue_ReturnsTrue ()
    {
      _symbolTable.SetSafeness ("local$0", "DummyType", true);
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithLocal");
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      bool returnsFragment = _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(returnsFragment, Is.True);
    }

    [Test]
    public void ReturnsFragment_AssignmentWithLocalNonFragment_ReturnsFalse ()
    {
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithLocal");
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      bool returnsFragment = _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(returnsFragment, Is.False);
    }

    [Test]
    public void ReturnsFragment_AssignmentWithLocalFragmentSetToFalse_ReturnsFalse ()
    {
      _symbolTable.SetSafeness ("local$0", "DummyType", false);
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithLocal");
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      bool returnsFragment = _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(returnsFragment, Is.False);
    }

    [Test]
    public void ReturnsFragment_AssignmentWithLocalFragmentSetToTrue_ReturnsFragmentType ()
    {
      _symbolTable.SetSafeness ("local$0", "DummyType", true);
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithLocal");
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(fragmentType, Is.EqualTo("DummyType"));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithLocalNonFragment_ReturnsNoFragmentType ()
    {
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithLocal");
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(fragmentType, Is.EqualTo(string.Empty));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithLocalFragmentSetToFalse_ReturnsNoFragmentType ()
    {
      _symbolTable.SetSafeness ("local$0", "DummyType", false);
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithLocal");
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(fragmentType, Is.EqualTo(string.Empty));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithParameterFragmentSetToTrue_ReturnsTrue ()
    {
      _symbolTable.SetSafeness ("parameter", "DummyType", true);
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithParameter", intTypeNode);
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      bool returnsFragment = _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(returnsFragment, Is.True);
    }

    [Test]
    public void ReturnsFragment_AssignmentWithParameterNonFragment_ReturnsFalse ()
    {
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithParameter", intTypeNode);
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      bool returnsFragment = _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(returnsFragment, Is.False);
    }

    [Test]
    public void ReturnsFragment_AssignmentWithParameterFragmentSetToFalse_ReturnsFalse ()
    {
      _symbolTable.SetSafeness ("parameter", "DummyType", false);
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithParameter", intTypeNode);
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      bool returnsFragment = _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(returnsFragment, Is.False);
    }

    [Test]
    public void ReturnsFragment_AssignmentWithParameterFragmentSetToTrue_ReturnsFragmentType ()
    {
      _symbolTable.SetSafeness ("parameter", "DummyType", true);
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithParameter", intTypeNode);
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(fragmentType, Is.EqualTo("DummyType"));
    }

    
    [Test]
    public void ReturnsFragment_AssignmentWithParameterNonFragment_ReturnsNoFragmentType ()
    {
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithParameter", intTypeNode);
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(fragmentType, Is.EqualTo(string.Empty));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithParameterFragmentSetToFalse_ReturnsNoFragmentType ()
    {
      _symbolTable.SetSafeness ("parameter", "DummyType", false);
      TypeNode intTypeNode = Helper.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithParameter", intTypeNode);
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(fragmentType, Is.EqualTo(string.Empty));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithSafeMethodCall_ReturnsTrue ()
    {
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithSafeMethodCall");
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      bool returnsFragment = _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(returnsFragment, Is.True);
    }

    
    [Test]
    public void ReturnsFragment_AssignmentWithUnsafeMethodCall_ReturnsFalse ()
    {
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithUnsafeMethodCall");
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      bool returnsFragment = _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(returnsFragment, Is.False);
    }

    [Test]
    public void ReturnsFragment_AssignmentWithSafeMethodCall_ReturnsFragmentType ()
    {
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithSafeMethodCall");
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(fragmentType, Is.EqualTo("SqlFragment"));
    }

    [Test]
    public void ReturnsFragment_AssignmentWithUnsafeMethodCall_ReturnsEmptyFragmentType ()
    {
      Method sample = TestHelper.GetSample<SymbolTableSample>("AssignmentWithUnsafeMethodCall");
      string fragmentType;
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      _symbolTable.ReturnsFragment(sampleExpression, out fragmentType);
      Assert.That(fragmentType, Is.EqualTo(string.Empty));
    }
  }
}
