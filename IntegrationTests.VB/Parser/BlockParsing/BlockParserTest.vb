Imports InjectionCop.Config
Imports InjectionCop.Parser
Imports InjectionCop.Parser.BlockParsing
Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System
Imports System.Collections.Generic

Namespace Parser.BlockParsing
	<TestFixture()>
	Public Class BlockParserTest
		Private Const c_returnFragmentType As String = "ReturnFragmentType"

		Private _blockParser As BlockParser

		Private _problemPipeStub As ProblemPipeStub

		Private _blacklist As IBlacklistManager

		Private _returnPreCondition As ReturnCondition

		<SetUp()>
		Public Sub SetUp()
			Me._blacklist = New IDbCommandBlacklistManagerStub()
			Me._problemPipeStub = New ProblemPipeStub()
			Me._returnPreCondition = New ReturnCondition("returnPreCondition", "ReturnPreConditionFragmentType")
			Dim returnPreConditions As System.Collections.Generic.List(Of ReturnCondition) = New System.Collections.Generic.List(Of ReturnCondition)() From { Me._returnPreCondition }
			Me._blockParser = New BlockParser(Me._blacklist, Me._problemPipeStub, "ReturnFragmentType", returnPreConditions)
		End Sub

		<Test()>
		Public Sub Parse_PostConditionOnlySafeSymbols()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("PostConditionOnlySafeSymbols", New TypeNode() {})
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Dim basicBlock As BasicBlock = Me._blockParser.Parse(sample)
			Dim correctPostCondition As Boolean = basicBlock.PostConditionSymbolTable.IsAssignableTo("local$0", "SqlFragment") AndAlso basicBlock.PostConditionSymbolTable.IsAssignableTo("local$1", "SqlFragment")
			Assert.That(correctPostCondition, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_PostConditionSafeAndUnsafeSymbols()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("PostConditionSafeAndUnsafeSymbols", New TypeNode() {})
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Dim basicBlock As BasicBlock = Me._blockParser.Parse(sample)
			Dim correctPostCondition As Boolean = basicBlock.PostConditionSymbolTable.IsAssignableTo("local$0", "SqlFragment") AndAlso Not basicBlock.PostConditionSymbolTable.IsAssignableTo("local$1", "SqlFragment")
			Assert.That(correctPostCondition, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafePreCondition()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("UnsafePreCondition", New TypeNode() { stringTypeNode })
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Dim basicBlock As BasicBlock = Me._blockParser.Parse(sample)
			Dim correctPreCondition As Boolean = basicBlock.PreConditions(0).Symbol = "unSafe" AndAlso basicBlock.PreConditions(0).FragmentType = "SqlFragment"
			Assert.That(correctPreCondition, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafePreCondition_OnlyReturnConditionCreated()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("SafePreCondition", New TypeNode() { stringTypeNode })
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Dim basicBlock As BasicBlock = Me._blockParser.Parse(sample)
			Dim correctPreCondition As Boolean = basicBlock.PreConditions.Length = 1
			Assert.That(correctPreCondition, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_MultipleUnsafePreCondition()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("MultipleUnsafePreCondition", New TypeNode() { stringTypeNode, stringTypeNode })
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Dim basicBlock As BasicBlock = Me._blockParser.Parse(sample)
			Dim correctPreCondition0 As Boolean = basicBlock.PreConditions(0).Symbol = "unSafe1" AndAlso basicBlock.PreConditions(0).FragmentType = "SqlFragment"
			Dim correctPreCondition As Boolean = basicBlock.PreConditions(1).Symbol = "unSafe2" AndAlso basicBlock.PreConditions(1).FragmentType = "SqlFragment"
			Assert.That(correctPreCondition0 AndAlso correctPreCondition, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_BlockInternalSafenessCondition_InternalSafenessSymbolNotInPreCondition()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("BlockInternalSafenessCondition", New TypeNode() { stringTypeNode })
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Dim basicBlock As BasicBlock = Me._blockParser.Parse(sample)
			Dim correctPreCondition As Boolean = basicBlock.PreConditions(0).Symbol = "x" AndAlso basicBlock.PreConditions(0).FragmentType = "SqlFragment"
			Assert.That(correctPreCondition, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SetSuccessor()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("SetSuccessor", New TypeNode() { stringTypeNode })
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Dim basicBlock As BasicBlock = Me._blockParser.Parse(sample)
			Dim correctPreCondition As Boolean = basicBlock.SuccessorKeys.Length = 1
			Assert.That(correctPreCondition, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_ReturnFragmentRequiredLiteralReturn_NoProblem()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("ReturnFragmentRequiredLiteralReturn", New TypeNode() {})
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(1), Block)
			Me._blockParser.Parse(sample)
			Dim noProblemsArised As Boolean = Me._problemPipeStub.Problems.Count = 0
			Assert.That(noProblemsArised, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeReturnWhenFragmentRequired_ReturnsLocalVariableThatGetsReturned()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("UnsafeReturnWhenFragmentRequired", New TypeNode() {})
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(1), Block)
			Dim returnedBlock As BasicBlock = Me._blockParser.Parse(sample)
			Dim preConditionSymbol As String = returnedBlock.PreConditions(0).Symbol
			Assert.That(preConditionSymbol, [Is].EqualTo("local$0"))
		End Sub

		<Test()>
		Public Sub Parse_UnsafeReturnWhenFragmentRequiredMoreComplex_ReturnsLocalVariableThatGetsReturned()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("UnsafeReturnWhenFragmentRequiredMoreComplex", New TypeNode() {})
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(1), Block)
			Dim returnedBlock As BasicBlock = Me._blockParser.Parse(sample)
			Dim preConditionSymbol As String = returnedBlock.PreConditions(0).Symbol
			Assert.That(preConditionSymbol, [Is].EqualTo("local$2"))
		End Sub

		<Test()>
		Public Sub Parse_ReturnFragmentRequiredUnsafeReturn_ReturnsCorrectReturnFragmentType(<Values("ReturnFragmentType1", "ReturnFragmentType2")> returnFragmentType As String)
			Me._blockParser = New BlockParser(Me._blacklist, Me._problemPipeStub, returnFragmentType, New System.Collections.Generic.List(Of ReturnCondition)())
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("UnsafeReturnWhenFragmentRequired", New TypeNode() {})
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(1), Block)
			Dim returnedBlock As BasicBlock = Me._blockParser.Parse(sample)
			Dim preConditionFragment As String = returnedBlock.PreConditions(0).FragmentType
			Assert.That(preConditionFragment, [Is].EqualTo(returnFragmentType))
		End Sub

		<Test()>
		Public Sub Parse_BlockWithReturnNodeNotReturningAnything_OnlyReturnConditionCreated()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("DummyProcedure", New TypeNode() {})
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Dim returnedBlock As BasicBlock = Me._blockParser.Parse(sample)
			Dim countPreconditions As Integer = returnedBlock.PreConditions.Length
			Assert.That(countPreconditions, [Is].EqualTo(1))
		End Sub

		<Test()>
		Public Sub Parse_BlockWithoutReturnNode_NoPreconditionCreated()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("UnsafeReturnWhenFragmentRequired", New TypeNode() {})
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Dim returnedBlock As BasicBlock = Me._blockParser.Parse(sample)
			Dim countPreconditions As Integer = returnedBlock.PreConditions.Length
			Assert.That(countPreconditions, [Is].EqualTo(0))
		End Sub

		<Test()>
		Public Sub Parse_ValidReturnWithIf_ReturnsLocalVariableThatGetsReturned()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("ValidReturnWithLiteralAssignmentInsideIf", New TypeNode() { stringTypeNode })
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(3), Block)
			Dim returnedBlock As BasicBlock = Me._blockParser.Parse(sample)
			Dim preConditionSymbol As String = returnedBlock.PreConditions(0).Symbol
			Assert.That(preConditionSymbol, [Is].EqualTo("local$1"))
		End Sub

		<Test()>
		Public Sub Parse_ValidReturnWithIf_ReturnsCorrectReturnFragmentType()
			Me._blockParser = New BlockParser(Me._blacklist, Me._problemPipeStub, "DummyFragment", New System.Collections.Generic.List(Of ReturnCondition)())
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("ValidReturnWithLiteralAssignmentInsideIf", New TypeNode() { stringTypeNode })
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(3), Block)
			Dim returnedBlock As BasicBlock = Me._blockParser.Parse(sample)
			Dim preConditionFragmentType As String = returnedBlock.PreConditions(0).FragmentType
			Assert.That(preConditionFragmentType, [Is].EqualTo("DummyFragment"))
		End Sub

		<Test()>
		Public Sub MethodGraph_ValidReturnWithIfBlockWithLocalAssignment_ReturnsLocalAssignment()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("ValidReturnWithLiteralAssignmentInsideIf", New TypeNode() { stringTypeNode })
			Dim preReturnBlock As Block = TryCast(sampleMethod.Body.Statements(2), Block)
			Dim preReturnBasicBlock As BasicBlock = Me._blockParser.Parse(preReturnBlock)
			Assert.That(preReturnBasicBlock.BlockAssignments.Length, [Is].EqualTo(1))
		End Sub

		<Test()>
		Public Sub MethodGraph_ValidReturnWithIfBlockWithLocalAssignment_ReturnsCorrectLocalAssignment()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("ValidReturnWithLiteralAssignmentInsideIf", New TypeNode() { stringTypeNode })
			Dim preReturnBlock As Block = TryCast(sampleMethod.Body.Statements(2), Block)
			Dim preReturnBasicBlock As BasicBlock = Me._blockParser.Parse(preReturnBlock)
			Dim source As String = preReturnBasicBlock.BlockAssignments(0).SourceSymbol
			Dim target As String = preReturnBasicBlock.BlockAssignments(0).TargetSymbol
			Dim correctLocalAssignment As Boolean = source = "local$0" AndAlso target = "local$1"
			Assert.That(correctLocalAssignment, [Is].[True])
		End Sub

		<Test()>
		Public Sub MethodGraph_ValidReturnWithIfBlockWithoutLocalAssignment_ReturnsNoLocalAssignment()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("ValidReturnWithLiteralAssignmentInsideIf", New TypeNode() { stringTypeNode })
			Dim ifBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)
			Dim ifBasicBlock As BasicBlock = Me._blockParser.Parse(ifBlock)
			Assert.That(ifBasicBlock.BlockAssignments.Length, [Is].EqualTo(0))
		End Sub

		<Test()>
		Public Sub MethodGraph_ValidReturnWithIfBlockWithoutLocalAssignment_ReturnsCorrectPostCondition()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("ValidReturnWithLiteralAssignmentInsideIf", New TypeNode() { stringTypeNode })
			Dim ifBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)
			Dim ifBasicBlock As BasicBlock = Me._blockParser.Parse(ifBlock)
			Dim postConditionFragmentType As String = ifBasicBlock.PostConditionSymbolTable.GetFragmentType("local$0")
			Assert.That(postConditionFragmentType, [Is].EqualTo("__Literal__"))
		End Sub

		<Test()>
		Public Sub Parse_DeclarationWithReturn_ReturnsCorrectPostConditions()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("DeclarationWithReturn", New TypeNode() {})
			Dim initialBlock As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Dim initialBasicBlock As BasicBlock = Me._blockParser.Parse(initialBlock)
			Dim local0FragmentType As String = initialBasicBlock.PostConditionSymbolTable.GetFragmentType("local$0")
			Dim local1FragmentType As String = initialBasicBlock.PostConditionSymbolTable.GetFragmentType("local$1")
			Dim correctPostConditions As Boolean = local0FragmentType = local1FragmentType AndAlso local0FragmentType = "__Literal__"
			Assert.That(correctPostConditions, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_ValidReturnWithParameterAssignmentInsideIf_ReturnsLocalAssignment()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("ValidReturnWithParameterAssignmentInsideIf", New TypeNode() { stringTypeNode })
			Dim ifBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)
			Dim ifBasicBlock As BasicBlock = Me._blockParser.Parse(ifBlock)
			Assert.That(ifBasicBlock.BlockAssignments.Length, [Is].EqualTo(1))
		End Sub

		<Test()>
		Public Sub Parse_ValidReturnWithParameterAssignmentInsideIf_ReturnsCorrectPostCondition()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("ValidReturnWithParameterAssignmentInsideIf", New TypeNode() { stringTypeNode })
			Dim ifBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)
			Dim ifBasicBlock As BasicBlock = Me._blockParser.Parse(ifBlock)
			Dim postConditionFragmentType As String = ifBasicBlock.PostConditionSymbolTable.GetFragmentType("local$0")
			Assert.That(postConditionFragmentType, [Is].EqualTo(SymbolTable.EMPTY_FRAGMENT))
		End Sub

		<Test()>
		Public Sub Parse_ValidReturnWithParameterResetAndAssignmentInsideIf_ReturnsNoLocalAssignment()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("ValidReturnWithParameterResetAndAssignmentInsideIf", New TypeNode() { stringTypeNode })
			Dim ifBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)
			Dim ifBasicBlock As BasicBlock = Me._blockParser.Parse(ifBlock)
			Assert.That(ifBasicBlock.BlockAssignments.Length, [Is].EqualTo(0))
		End Sub

		<Test()>
		Public Sub Parse_ValidReturnWithParameterResetAndAssignmentInsideIf_ReturnsCorrectPostCondition()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("ValidReturnWithParameterResetAndAssignmentInsideIf", New TypeNode() { stringTypeNode })
			Dim ifBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)
			Dim ifBasicBlock As BasicBlock = Me._blockParser.Parse(ifBlock)
			Dim postConditionFragmentType As String = ifBasicBlock.PostConditionSymbolTable.GetFragmentType("local$0")
			Assert.That(postConditionFragmentType, [Is].EqualTo("__Literal__"))
		End Sub

		<Test()>
		Public Sub Parse_ReturnLiteral_ReturnConditionIsAddedToReturnBlock()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("ReturnLiteral", New TypeNode() {})
			Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(1), Block)
			Dim returnBasicBlock As BasicBlock = Me._blockParser.Parse(returnBlock)
			Dim preConditionSymbol As String = returnBasicBlock.PreConditions(1).Symbol
			Dim preConditionFragmentType As String = returnBasicBlock.PreConditions(1).FragmentType
			Dim correctPrecondition As Boolean = preConditionSymbol = Me._returnPreCondition.Symbol AndAlso preConditionFragmentType = Me._returnPreCondition.FragmentType
			Assert.That(correctPrecondition, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_OutReturnPreconditionCheckSafe_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("OutReturnPreconditionCheckSafe", New TypeNode() { stringTypeNode.GetReferenceType() })
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Me._blockParser.Parse(sample)
			Dim noProblemsArised As Boolean = Me._problemPipeStub.Problems.Count = 0
			Assert.That(noProblemsArised, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_OutReturnPreconditionCheckUnSafe_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("OutReturnPreconditionCheckUnSafe", New TypeNode() { stringTypeNode.GetReferenceType() })
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Me._blockParser.Parse(sample)
			Dim problemFound As Boolean = Me._problemPipeStub.Problems.Count <> 0
			Assert.That(problemFound, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_OutReturnPreconditionCheckSafeLiteralAssignment_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("OutReturnPreconditionCheckSafeLiteralAssignment", New TypeNode() { stringTypeNode.GetReferenceType() })
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Me._blockParser.Parse(sample)
			Dim noProblemsArised As Boolean = Me._problemPipeStub.Problems.Count = 0
			Assert.That(noProblemsArised, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_OutReturnPreconditionConditional_ReturnConditionIsAddedToReturnBlock()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("OutReturnPreconditionConditional", New TypeNode() { stringTypeNode.GetReferenceType() })
			Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(2), Block)
			Dim returnBasicBlock As BasicBlock = Me._blockParser.Parse(returnBlock)
			Dim preConditionSymbol As String = returnBasicBlock.PreConditions(0).Symbol
			Dim preConditionFragmentType As String = returnBasicBlock.PreConditions(0).FragmentType
			Dim correctPrecondition As Boolean = preConditionSymbol = Me._returnPreCondition.Symbol AndAlso preConditionFragmentType = Me._returnPreCondition.FragmentType
			Assert.That(correctPrecondition, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_OutReturnPreconditionConditionalWithReturnInsideIf_ReturnConditionIsAddedToReturnBlock()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("OutReturnPreconditionConditionalWithReturnInsideIf", New TypeNode() { stringTypeNode.GetReferenceType() })
			Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(3), Block)
			Dim returnBasicBlock As BasicBlock = Me._blockParser.Parse(returnBlock)
			Dim preConditionSymbol As String = returnBasicBlock.PreConditions(0).Symbol
			Dim preConditionFragmentType As String = returnBasicBlock.PreConditions(0).FragmentType
			Dim correctPrecondition As Boolean = preConditionSymbol = Me._returnPreCondition.Symbol AndAlso preConditionFragmentType = Me._returnPreCondition.FragmentType
			Assert.That(correctPrecondition, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_OutReturnPreconditionConditionalWithReturnAfterIf_ReturnConditionIsAddedToReturnBlock()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("OutReturnPreconditionConditionalWithReturnAfterIf", New TypeNode() { stringTypeNode.GetReferenceType() })
			Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(2), Block)
			Dim returnBasicBlock As BasicBlock = Me._blockParser.Parse(returnBlock)
			Dim preConditionSymbol As String = returnBasicBlock.PreConditions(0).Symbol
			Dim preConditionFragmentType As String = returnBasicBlock.PreConditions(0).FragmentType
			Dim correctPrecondition As Boolean = preConditionSymbol = Me._returnPreCondition.Symbol AndAlso preConditionFragmentType = Me._returnPreCondition.FragmentType
			Assert.That(correctPrecondition, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_RefReturnPreconditionCheckSafe_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("RefReturnPreconditionCheckSafe", New TypeNode() { stringTypeNode.GetReferenceType() })
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Me._blockParser.Parse(sample)
			Dim noProblemsArised As Boolean = Me._problemPipeStub.Problems.Count = 0
			Assert.That(noProblemsArised, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_RefReturnPreconditionCheckUnSafe_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("RefReturnPreconditionCheckUnSafe", New TypeNode() { stringTypeNode.GetReferenceType() })
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Me._blockParser.Parse(sample)
			Dim problemFound As Boolean = Me._problemPipeStub.Problems.Count <> 0
			Assert.That(problemFound, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_RefReturnPreconditionCheckSafeLiteralAssignment_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("RefReturnPreconditionCheckSafeLiteralAssignment", New TypeNode() { stringTypeNode.GetReferenceType() })
			Dim sample As Block = TryCast(sampleMethod.Body.Statements(0), Block)
			Me._blockParser.Parse(sample)
			Dim noProblemsArised As Boolean = Me._problemPipeStub.Problems.Count = 0
			Assert.That(noProblemsArised, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_RefReturnPreconditionConditional_ReturnConditionIsAddedToReturnBlock()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("RefReturnPreconditionConditional", New TypeNode() { stringTypeNode.GetReferenceType() })
			Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(2), Block)
			Dim returnBasicBlock As BasicBlock = Me._blockParser.Parse(returnBlock)
			Dim preConditionSymbol As String = returnBasicBlock.PreConditions(0).Symbol
			Dim preConditionFragmentType As String = returnBasicBlock.PreConditions(0).FragmentType
			Dim correctPrecondition As Boolean = preConditionSymbol = Me._returnPreCondition.Symbol AndAlso preConditionFragmentType = Me._returnPreCondition.FragmentType
			Assert.That(correctPrecondition, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_RefReturnPreconditionConditionalWithReturnInsideIf_ReturnConditionIsAddedToReturnBlock()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("RefReturnPreconditionConditionalWithReturnInsideIf", New TypeNode() { stringTypeNode.GetReferenceType() })
			Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(3), Block)
			Dim returnBasicBlock As BasicBlock = Me._blockParser.Parse(returnBlock)
			Dim preConditionSymbol As String = returnBasicBlock.PreConditions(0).Symbol
			Dim preConditionFragmentType As String = returnBasicBlock.PreConditions(0).FragmentType
			Dim correctPrecondition As Boolean = preConditionSymbol = Me._returnPreCondition.Symbol AndAlso preConditionFragmentType = Me._returnPreCondition.FragmentType
			Assert.That(correctPrecondition, [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_RefReturnPreconditionConditionalWithReturnAfterIf_ReturnConditionIsAddedToReturnBlock()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethod As Method = TestHelper.GetSample(Of BlockParserSample)("RefReturnPreconditionConditionalWithReturnAfterIf", New TypeNode() { stringTypeNode.GetReferenceType() })
			Dim returnBlock As Block = TryCast(sampleMethod.Body.Statements(2), Block)
			Dim returnBasicBlock As BasicBlock = Me._blockParser.Parse(returnBlock)
			Dim preConditionSymbol As String = returnBasicBlock.PreConditions(0).Symbol
			Dim preConditionFragmentType As String = returnBasicBlock.PreConditions(0).FragmentType
			Dim correctPrecondition As Boolean = preConditionSymbol = Me._returnPreCondition.Symbol AndAlso preConditionFragmentType = Me._returnPreCondition.FragmentType
			Assert.That(correctPrecondition, [Is].[True])
		End Sub
	End Class
End Namespace
