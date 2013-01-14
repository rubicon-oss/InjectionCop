Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.NestedCall
	<TestFixture()>
	Public Class NestedCall_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_NestedValidCallReturn_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of NestedCallSample)("NestedValidCallReturn", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_NestedInvalidCallReturn_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of NestedCallSample)("NestedInvalidCallReturn", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_NestedInvalidCall_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of NestedCallSample)("NestedInvalidCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_DeeperNestedInvalidCall_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of NestedCallSample)("DeeperNestedInvalidCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_ValidMethodCallChain_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of NestedCallSample)("ValidMethodCallChain", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_InvalidMethodCallChain_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of NestedCallSample)("InvalidMethodCallChain", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_ValidMethodCallChainDifferentOperand_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of NestedCallSample)("ValidMethodCallChainDifferentOperand", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_InvalidMethodCallChainDifferentOperand_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of NestedCallSample)("InvalidMethodCallChainDifferentOperand", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
