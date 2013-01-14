Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.AttributePropagation
	<TestFixture()>
	Public Class AttributePropagation_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_SafeCallOfSqlFragmentCallee_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of AttributePropagationSample)("SafeCallOfSqlFragmentCallee", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallOfSqlFragmentCallee_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of AttributePropagationSample)("UnsafeCallOfSqlFragmentCallee", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallOfMixedCallee_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of AttributePropagationSample)("SafeCallOfMixedCallee", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallOfMixedCallee_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of AttributePropagationSample)("UnsafeCallOfMixedCallee", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
