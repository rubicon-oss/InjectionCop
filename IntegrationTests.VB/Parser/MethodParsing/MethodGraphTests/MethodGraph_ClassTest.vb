Imports InjectionCop.Parser
Imports InjectionCop.Parser.BlockParsing
Imports InjectionCop.Parser.MethodParsing
Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System
Imports System.Linq

Namespace Parser.MethodParsing.MethodGraphTests
  <TestFixture()>
  Public Class MethodGraph_ClassTest
    Inherits MethodGraph_TestBase

    <Test()>
    Public Sub GetInitialBlockId_DeclarationWithReturn_ReturnsIdOfInitialBlock()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("DeclarationWithReturn", New TypeNode() {})
      Dim initialBlock As Block = TryCast(sampleMethod.Body.Statements(0), Block)
      If initialBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Assert.That(methodGraph.InitialBlock.Id, [Is].EqualTo(initialBlock.UniqueKey))
      End If
    End Sub

    <Test()>
    Public Sub GetBlockById_FirstBlockIdOfDeclarationWithReturnSample_ReturnsInitialBasicBlock()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("DeclarationWithReturn", New TypeNode() {})
      Dim initialBlock As Block = TryCast(sampleMethod.Body.Statements(0), Block)
      If initialBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim initialBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(initialBlock.UniqueKey)
        Assert.That(initialBasicBlock.Id, [Is].EqualTo(initialBlock.UniqueKey))
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub GetBlockById_SuccessorOfFirstBlockIdOfDeclarationWithReturnSample_ReturnsReturnBasicBlock()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("DeclarationWithReturn", New TypeNode() {})
      Dim initialBlock As Block = TryCast(sampleMethod.Body.Statements(0), Block)
      Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)
      If initialBlock IsNot Nothing AndAlso returnBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim returnBlockId As Integer = returnBlock.UniqueKey
        Dim returnBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(returnBlockId)
        Assert.That(returnBasicBlock.Id, [Is].EqualTo(returnBlock.UniqueKey))
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <ExpectedException(GetType(InjectionCopException), ExpectedMessage:="The given key was not present in the MethodGraph")>
    <Test()>
    Public Sub GetBlockById_InvalidId_ThrowsException()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("DeclarationWithReturn", New TypeNode() {})
      Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
      methodGraph.GetBasicBlockById(-1)
    End Sub

    <Test()>
    Public Sub MethodGraph_DeclarationWithReturn_ReturnsCorrectInitialBlockSuccessors()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("DeclarationWithReturn", New TypeNode() {})
      Dim initialBlock As Block = TryCast(sampleMethod.Body.Statements(0), Block)
      Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)
      If initialBlock IsNot Nothing AndAlso returnBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim initialBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(initialBlock.UniqueKey)
        Dim initialBasicBlockConnectedWithReturn As Boolean = initialBasicBlock.SuccessorKeys.Any(Function(key As Integer) key = returnBlock.UniqueKey)
        Assert.That(initialBasicBlockConnectedWithReturn, [Is].[True])
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_DeclarationWithReturn_ReturnBlockHasNoSuccessors()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("DeclarationWithReturn", New TypeNode() {})
      Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)
      If returnBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim returnBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(returnBlock.UniqueKey)
        Assert.That(returnBasicBlock.SuccessorKeys.Length, [Is].EqualTo(0))
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_DeclarationWithReturn_ReturnBlockHasReturnFragmentPrecondition()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("DeclarationWithReturn", New TypeNode() {})
      Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)
      If returnBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim returnBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(returnBlock.UniqueKey)
        Dim preConditionFragmentType As String = returnBasicBlock.PreConditions(0).FragmentType
        Assert.That(preConditionFragmentType, [Is].EqualTo("ReturnFragmentType"))
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_IfStatementTrueBlockOnly_ReturnsCorrectConditionSuccessors()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("IfStatementTrueBlockOnly", New TypeNode() {stringTypeNode})
      Dim conditionBlock As Block = TryCast(sampleMethod.Body.Statements(0), Block)
      Dim trueBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)
      Dim preReturnBlock As Block = TryCast(sampleMethod.Body.Statements(2), Block)
      If conditionBlock IsNot Nothing AndAlso preReturnBlock IsNot Nothing AndAlso trueBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim conditionBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(conditionBlock.UniqueKey)
        Dim arg_102_0 As Boolean
        If conditionBasicBlock.SuccessorKeys.Any(Function(key As Integer) key = trueBlock.UniqueKey) Then
          arg_102_0 = conditionBasicBlock.SuccessorKeys.Any(Function(key As Integer) key = preReturnBlock.UniqueKey)
        Else
          arg_102_0 = False
        End If
        Dim conditionBasicBlockSuccessorsOk As Boolean = arg_102_0
        Assert.That(conditionBasicBlockSuccessorsOk, [Is].[True])
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_IfStatementTrueBlockOnly_ReturnsCorrectTrueBlockSuccessors()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("IfStatementTrueBlockOnly", New TypeNode() {stringTypeNode})
      Dim trueBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)
      Dim preReturnBlock As Block = TryCast(sampleMethod.Body.Statements(2), Block)
      If preReturnBlock IsNot Nothing AndAlso trueBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim trueBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(trueBlock.UniqueKey)
        Dim trueBasicBlockSuccessorsOk As Boolean = trueBasicBlock.SuccessorKeys.Any(Function(key As Integer) key = preReturnBlock.UniqueKey)
        Assert.That(trueBasicBlockSuccessorsOk, [Is].[True])
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_IfStatementTrueBlockOnly_ReturnsCorrectPreReturnSuccessors()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("IfStatementTrueBlockOnly", New TypeNode() {stringTypeNode})
      Dim preReturnBlock As Block = TryCast(sampleMethod.Body.Statements(2), Block)
      Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(3), Block)
      If returnBlock IsNot Nothing AndAlso preReturnBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim preReturnBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(preReturnBlock.UniqueKey)
        Dim preReturnBasicBlockSuccessorsOk As Boolean = preReturnBasicBlock.SuccessorKeys.Any(Function(key As Integer) key = returnBlock.UniqueKey)
        Assert.That(preReturnBasicBlockSuccessorsOk, [Is].[True])
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_IfStatementTrueBlockOnly_ReturnsCorrectReturnSuccessors()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("IfStatementTrueBlockOnly", New TypeNode() {stringTypeNode})
      Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(3), Block)
      If returnBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim preReturnBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(returnBlock.UniqueKey)
        Assert.That(preReturnBasicBlock.SuccessorKeys.Length, [Is].EqualTo(0))
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_ForLoop_ReturnsCorrectPreForSuccessors()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("ForLoop", New TypeNode() {})
      Dim preForBlock As Block = TryCast(sampleMethod.Body.Statements(0), Block)
      Dim conditionBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)

      If preForBlock IsNot Nothing AndAlso conditionBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim preForBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(preForBlock.UniqueKey)
        Dim preForBasicBlockSuccessorsCorrect As Boolean = preForBasicBlock.SuccessorKeys.Length = 1 AndAlso preForBasicBlock.SuccessorKeys(0) = conditionBlock.UniqueKey

        Assert.That(preForBasicBlockSuccessorsCorrect, [Is].[True])
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_ForLoop_ReturnsCorrectInnerForSuccessors()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("ForLoop", New TypeNode() {})
      Dim innerForBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)
      Dim conditionBlock As Block = TryCast(sampleMethod.Body.Statements(2), Block)

      If innerForBlock IsNot Nothing AndAlso conditionBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim innerForBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(innerForBlock.UniqueKey)
        Dim innerForBasicBlockSuccessorsCorrect As Boolean = innerForBasicBlock.SuccessorKeys.Length = 2 AndAlso innerForBasicBlock.SuccessorKeys.Contains(conditionBlock.UniqueKey)

        Assert.That(innerForBasicBlockSuccessorsCorrect, [Is].[True])
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_ForLoop_ReturnsCorrectConditionSuccessors()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("ForLoop", New TypeNode() {})
      Dim innerLoopBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)
      Dim preReturnBlock As Block = TryCast(sampleMethod.Body.Statements(2), Block)

      If innerLoopBlock IsNot Nothing AndAlso preReturnBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim conditionBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(innerLoopBlock.UniqueKey)

        Dim conditionBasicBlockSuccessorsCorrect As Boolean = conditionBasicBlock.SuccessorKeys.Length = 2 AndAlso
          conditionBasicBlock.SuccessorKeys.Contains(innerLoopBlock.UniqueKey) AndAlso
          conditionBasicBlock.SuccessorKeys.Contains(preReturnBlock.UniqueKey)

        Assert.That(conditionBasicBlockSuccessorsCorrect, [Is].[True])
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_ForLoop_ReturnsCorrectPreReturnSuccessors()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("ForLoop", New TypeNode() {})
      Dim preReturnBlock As Block = TryCast(sampleMethod.Body.Statements(2), Block)
      Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(3), Block)

      If preReturnBlock IsNot Nothing AndAlso returnBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim preReturnBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(preReturnBlock.UniqueKey)
        Dim preReturnBasicBlockSuccessorsCorrect As Boolean = preReturnBasicBlock.SuccessorKeys.Length = 1 AndAlso preReturnBasicBlock.SuccessorKeys(0) = returnBlock.UniqueKey
        Assert.That(preReturnBasicBlockSuccessorsCorrect, [Is].[True])
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_ForLoop_ReturnsCorrectReturnSuccessors()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("ForLoop", New TypeNode() {})
      Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(3), Block)
      If returnBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim returnBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(returnBlock.UniqueKey)
        Assert.That(returnBasicBlock.SuccessorKeys.Length, [Is].EqualTo(0))
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_DeclarationWithReturn_ReturnsCorrectPostConditions()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("DeclarationWithReturn", New TypeNode() {})
      Dim initialBlock As Block = TryCast(sampleMethod.Body.Statements(0), Block)
      If initialBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim initialBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(initialBlock.UniqueKey)
        Dim postConditionFragmentType As String = initialBasicBlock.PostConditionSymbolTable.GetFragmentType("local$0")
        Assert.That(postConditionFragmentType, [Is].EqualTo("__Literal__"))
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_ValidReturnWithIf_ReturnsCorrectPostConditions()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("ValidReturnWithIf", New TypeNode() {stringTypeNode})
      Dim preReturnBlock As Block = TryCast(sampleMethod.Body.Statements(2), Block)
      If preReturnBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim preReturnBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(preReturnBlock.UniqueKey)
        Dim postConditionFragmentType As String = preReturnBasicBlock.PostConditionSymbolTable.GetFragmentType("local$1")
        Assert.That(postConditionFragmentType, [Is].EqualTo(SymbolTable.EMPTY_FRAGMENT))
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_FragmentOutParameterSafeReturn_AddsPreconditionToReturnBlock()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("FragmentOutParameterSafeReturn", New TypeNode() {stringTypeNode.GetReferenceType()})
      Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(2), Block)
      If returnBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim returnBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(returnBlock.UniqueKey)
        Dim preConditionSymbolName As String = returnBasicBlock.PreConditions(0).Symbol
        Dim preConditionFragmentType As String = returnBasicBlock.PreConditions(0).FragmentType
        Dim correctPreCondition As Boolean = preConditionSymbolName = "safe" AndAlso preConditionFragmentType = "SqlFragment"
        Assert.That(correctPreCondition, [Is].[True])
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_FragmentRefParameterSafeReturn_AddsPreconditionToReturnBlock()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("FragmentRefParameterSafeReturn", New TypeNode() {stringTypeNode.GetReferenceType()})
      Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(2), Block)
      If returnBlock IsNot Nothing Then
        Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
        Dim returnBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(returnBlock.UniqueKey)
        Dim preConditionSymbolName As String = returnBasicBlock.PreConditions(0).Symbol
        Dim preConditionFragmentType As String = returnBasicBlock.PreConditions(0).FragmentType
        Dim correctPreCondition As Boolean = preConditionSymbolName = "safe" AndAlso preConditionFragmentType = "SqlFragment"
        Assert.That(correctPreCondition, [Is].[True])
      Else
        Assert.Ignore("Bad Sample")
      End If
    End Sub

    <Test()>
    Public Sub MethodGraph_TryCatchFinally_TryBlockIsParsed()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("TryCatchFinally", New TypeNode() {})
      Dim outerTryNode As TryNode = CType(sampleMethod.Body.Statements(1), TryNode)
      Dim innerTryNode As TryNode = CType(outerTryNode.Block.Statements(0), TryNode)
      Dim tryBlockUniqueKey As Integer = innerTryNode.Block.UniqueKey
      Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
      Dim tryBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(tryBlockUniqueKey)
      Assert.That(tryBasicBlock, [Is].[Not].Null)
    End Sub

    <Test()>
    Public Sub MethodGraph_TryCatchFinally_CatchBlockIsParsed()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("TryCatchFinally", New TypeNode() {})
      Dim outerTryNode As TryNode = CType(sampleMethod.Body.Statements(1), TryNode)
      Dim innerTryNode As TryNode = CType(outerTryNode.Block.Statements(0), TryNode)
      Dim catchNode As CatchNode = innerTryNode.Catchers(0)
      Dim catchBlockUniqueKey As Integer = catchNode.Block.UniqueKey
      Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
      Dim catchBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(catchBlockUniqueKey)
      Assert.That(catchBasicBlock, [Is].[Not].Null)
    End Sub

    <Test()>
    Public Sub MethodGraph_TryCatchFinally_FinallyBlockIsParsed()
      Dim sampleMethod As Method = TestHelper.GetSample(Of MethodGraph_ClassSample)("TryCatchFinally", New TypeNode() {})
      Dim outerTryNode As TryNode = CType(sampleMethod.Body.Statements(1), TryNode)
      Dim finallyBlock As Block = outerTryNode.[Finally].Block
      Dim finallyBlockUniqueKey As Integer = finallyBlock.UniqueKey
      Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
      Dim finallyBasicBlock As BasicBlock = methodGraph.GetBasicBlockById(finallyBlockUniqueKey)
      Assert.That(finallyBasicBlock, [Is].[Not].Null)
    End Sub
  End Class
End Namespace
