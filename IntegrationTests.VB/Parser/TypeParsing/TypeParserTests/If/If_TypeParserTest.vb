Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.[If]
	<TestFixture()>
	Public Class If_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_ValidExampleInsideIf_NoProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of IfSample)("ValidExampleInsideIf", New TypeNode() { intTypeNode, intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_InvalidExampleInsideIf_ReturnsProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of IfSample)("InvalidExampleInsideIf", New TypeNode() { intTypeNode, intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InvalidExampleInsideElse_ReturnsProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of IfSample)("InvalidExampleInsideElse", New TypeNode() { intTypeNode, intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeAssignmentInsideIf_ReturnsProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of IfSample)("UnsafeAssignmentInsideIf", New TypeNode() { intTypeNode, intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeAssignmentInsideIfTwisted_ReturnsProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of IfSample)("UnsafeAssignmentInsideIfTwisted", New TypeNode() { intTypeNode, intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeAssignmentInsideIfNested_ReturnsProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of IfSample)("UnsafeAssignmentInsideIfNested", New TypeNode() { intTypeNode, intTypeNode, intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeAssignmentInsideIfNested_No()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of IfSample)("SafeAssignmentInsideIfNested", New TypeNode() { intTypeNode, intTypeNode, intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeAssignmentInsideIfNestedDeeper_ReturnsProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of IfSample)("UnsafeAssignmentInsideIfNestedDeeper", New TypeNode() { intTypeNode, intTypeNode, intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeAssignmentInsideIfNestedElse_ReturnsProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of IfSample)("UnsafeAssignmentInsideIfNestedElse", New TypeNode() { intTypeNode, intTypeNode, intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InvalidCallInsideIfCondition_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of IfSample)("InvalidCallInsideIfCondition", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
