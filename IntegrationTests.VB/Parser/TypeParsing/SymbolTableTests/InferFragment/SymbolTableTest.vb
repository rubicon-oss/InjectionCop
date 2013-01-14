Imports InjectionCop.Config
Imports InjectionCop.Parser
Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.SymbolTableTests.InferFragment
  <TestFixture()>
  Public Class SymbolTableTest
    Private _symbolTable As SymbolTable

    Private Shared _emptyFragment As String = "__EmptyFragment__"

    Private Shared _literal As String = "__Literal__"

    Public Shared ReadOnly Property EmptyFragment() As String
      Get
        Return SymbolTableTest._emptyFragment
      End Get
    End Property

    Public Shared ReadOnly Property Literal() As String
      Get
        Return SymbolTableTest._literal
      End Get
    End Property

    <SetUp()>
    Public Sub SetUp()
      Dim blacklistManager As IBlacklistManager = New IDbCommandBlacklistManagerStub()
      Me._symbolTable = New SymbolTable(blacklistManager)
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithLiteral_ReturnsTrue()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithLiteral", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].[Not].EqualTo(SymbolTableTest.EmptyFragment))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithLiteral_InfersFragmentType()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithLiteral", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo(SymbolTableTest.Literal))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithLocalFragmentSetToTrue_ReturnsTrue()
      Me._symbolTable.MakeSafe("local$1", "DummyType")
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithLocal", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(4), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].[Not].EqualTo(SymbolTableTest.EmptyFragment))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithLocalNonFragment_ReturnsFalse()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithLocal", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(4), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo(SymbolTableTest.EmptyFragment))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithLocalFragmentSetToFalse_ReturnsFalse()
      Me._symbolTable.MakeUnsafe("local$0")
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithLocal", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(4), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo(SymbolTableTest.EmptyFragment))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithLocalFragmentSetToTrue_InfersFragmentType()
      Me._symbolTable.MakeSafe("local$1", "DummyType")
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithLocal", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(4), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo("DummyType"))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithLocalNonFragment_ReturnsNoFragmentType()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithLocal", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(4), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo(SymbolTableTest.EmptyFragment))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithLocalFragmentSetToFalse_ReturnsNoFragmentType()
      Me._symbolTable.MakeUnsafe("local$0")
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithLocal", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(4), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo(SymbolTableTest.EmptyFragment))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithParameterFragmentSetToTrue_ReturnsTrue()
      Me._symbolTable.MakeSafe("parameter", "DummyType")
      Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithParameter", New TypeNode() {intTypeNode})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].[Not].EqualTo(SymbolTableTest.EmptyFragment))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithParameterNonFragment_ReturnsFalse()
      Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithParameter", New TypeNode() {intTypeNode})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo(SymbolTableTest.EmptyFragment))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithParameterFragmentSetToFalse_ReturnsFalse()
      Me._symbolTable.MakeUnsafe("parameter")
      Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithParameter", New TypeNode() {intTypeNode})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo(SymbolTableTest.EmptyFragment))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithParameterFragmentSetToTrue_InfersFragmentType()
      Me._symbolTable.MakeSafe("parameter", "DummyType")
      Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithParameter", New TypeNode() {intTypeNode})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo("DummyType"))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithParameterNonFragment_ReturnsNoFragmentType()
      Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithParameter", New TypeNode() {intTypeNode})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo(SymbolTableTest.EmptyFragment))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithParameterFragmentSetToFalse_ReturnsNoFragmentType()
      Me._symbolTable.MakeUnsafe("parameter")
      Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithParameter", New TypeNode() {intTypeNode})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo(SymbolTableTest.EmptyFragment))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithSafeMethodCall_ReturnsTrue()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithSafeMethodCall", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].[Not].EqualTo(SymbolTableTest.EmptyFragment))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithUnsafeMethodCall_ReturnsFalse()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithUnsafeMethodCall", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo(SymbolTableTest.EmptyFragment))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithSafeMethodCall_InfersFragmentType()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithSafeMethodCall", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo("SqlFragment"))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithUnsafeMethodCall_ReturnsEmptyFragmentType()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithUnsafeMethodCall", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo(SymbolTableTest.EmptyFragment))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithFragmentField_ReturnsEmptyFragmentType()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithFragmentField", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo("SampleFragment"))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithNonFragmentField_ReturnsEmptyFragmentType()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithNonFragmentField", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo(SymbolTable.EMPTY_FRAGMENT))
    End Sub

    <Test()>
    Public Sub InferFragmentType_AssignmentWithFragmentProperty_ReturnsPropertyFragmentType()
      Dim sample As Method = TestHelper.GetSample(Of InferFragmentSample)("AssignmentWithFragmentProperty", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignment As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignment.Source
      Dim fragmentType As String = Me._symbolTable.InferFragmentType(sampleExpression)
      Assert.That(fragmentType, [Is].EqualTo("PropertyFragmentType"))
    End Sub
  End Class
End Namespace
