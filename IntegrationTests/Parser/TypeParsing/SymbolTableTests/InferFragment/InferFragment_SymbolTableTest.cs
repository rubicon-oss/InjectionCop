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
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.SymbolTableTests.InferFragment
{
  [TestFixture]
  public class SymbolTableTest
  {
    private SymbolTable _symbolTable;
    private static readonly Fragment _emptyFragment = Fragment.CreateEmpty();
    private static readonly Fragment _literal = Fragment.CreateLiteral();

    public static Fragment EmptyFragment
    {
      get { return _emptyFragment; }
    }

    public static Fragment Literal
    {
      get { return _literal; }
    }

    [SetUp]
    public void SetUp ()
    {
      IBlacklistManager blacklistManager = new IDbCommandBlacklistManagerStub();
      _symbolTable = new SymbolTable (blacklistManager);
    }

    [Test]
    public void InferFragmentType_AssignmentWithLiteral_ReturnsTrue ()
    {
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithLiteral");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.Not.EqualTo(EmptyFragment));
    }

    [Test]
    public void InferFragmentType_AssignmentWithLiteral_InfersFragmentType ()
    {
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithLiteral");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(Literal));
    }

    [Test]
    public void InferFragmentType_AssignmentWithLocalFragmentSetToTrue_ReturnsTrue ()
    {
      _symbolTable.MakeSafe ("local$0", Fragment.CreateNamed("DummyType"));
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithLocal");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.Not.EqualTo(EmptyFragment));
    }

    [Test]
    public void InferFragmentType_AssignmentWithLocalNonFragment_ReturnsFalse ()
    {
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithLocal");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void InferFragmentType_AssignmentWithLocalFragmentSetToFalse_ReturnsFalse ()
    {
      _symbolTable.MakeUnsafe ("local$0");
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithLocal");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void InferFragmentType_AssignmentWithLocalFragmentSetToTrue_InfersFragmentType ()
    {
      _symbolTable.MakeSafe ("local$0", Fragment.CreateNamed("DummyType"));
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithLocal");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(Fragment.CreateNamed("DummyType")));
    }

    [Test]
    public void InferFragmentType_AssignmentWithLocalNonFragment_ReturnsNoFragmentType ()
    {
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithLocal");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void InferFragmentType_AssignmentWithLocalFragmentSetToFalse_ReturnsNoFragmentType ()
    {
      _symbolTable.MakeUnsafe ("local$0");
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithLocal");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[4];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void InferFragmentType_AssignmentWithParameterFragmentSetToTrue_ReturnsTrue ()
    {
      _symbolTable.MakeSafe ("parameter", Fragment.CreateNamed("DummyType"));
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithParameter", intTypeNode);
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.Not.EqualTo(EmptyFragment));
    }

    [Test]
    public void InferFragmentType_AssignmentWithParameterNonFragment_ReturnsFalse ()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithParameter", intTypeNode);
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void InferFragmentType_AssignmentWithParameterFragmentSetToFalse_ReturnsFalse ()
    {
      _symbolTable.MakeUnsafe ("parameter");
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithParameter", intTypeNode);
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void InferFragmentType_AssignmentWithParameterFragmentSetToTrue_InfersFragmentType ()
    {
      _symbolTable.MakeSafe ("parameter", Fragment.CreateNamed("DummyType"));
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithParameter", intTypeNode);
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(Fragment.CreateNamed("DummyType")));
    }

    
    [Test]
    public void InferFragmentType_AssignmentWithParameterNonFragment_ReturnsNoFragmentType ()
    {
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithParameter", intTypeNode);
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void InferFragmentType_AssignmentWithParameterFragmentSetToFalse_ReturnsNoFragmentType ()
    {
      _symbolTable.MakeUnsafe ("parameter");
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithParameter", intTypeNode);
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void InferFragmentType_AssignmentWithSafeMethodCall_ReturnsTrue ()
    {
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithSafeMethodCall");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.Not.EqualTo(EmptyFragment));
    }

    
    [Test]
    public void InferFragmentType_AssignmentWithUnsafeMethodCall_ReturnsFalse ()
    {
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithUnsafeMethodCall");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void InferFragmentType_AssignmentWithSafeMethodCall_InfersFragmentType ()
    {
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithSafeMethodCall");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(Fragment.CreateNamed("SqlFragment")));
    }

    [Test]
    public void InferFragmentType_AssignmentWithUnsafeMethodCall_ReturnsEmptyFragmentType ()
    {
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithUnsafeMethodCall");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(EmptyFragment));
    }

    [Test]
    public void InferFragmentType_AssignmentWithFragmentField_ReturnsEmptyFragmentType (/*[Values("AnyType1", "AnyType2")] string symbolTableType*/)
    {
      //_symbolTable.MakeSafe ("_field", symbolTableType);
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithFragmentField");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(Fragment.CreateNamed("SampleFragment")));
    }

    [Test]
    public void InferFragmentType_AssignmentWithNonFragmentField_ReturnsEmptyFragmentType ()
    {
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithNonFragmentField");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(Fragment.CreateEmpty()));
    }

    [Test]
    public void InferFragmentType_AssignmentWithFragmentProperty_ReturnsPropertyFragmentType ()
    {
      Method sample = TestHelper.GetSample<InferFragmentSample>("AssignmentWithFragmentProperty");
      Block assignmentBlock = (Block)sample.Body.Statements[0];
      AssignmentStatement assignment = (AssignmentStatement)assignmentBlock.Statements[1];
      Expression sampleExpression = assignment.Source;
      var fragmentType = _symbolTable.InferFragmentType(sampleExpression);
      Assert.That(fragmentType, Is.EqualTo(Fragment.CreateNamed("PropertyFragmentType")));
    }
  }
}
