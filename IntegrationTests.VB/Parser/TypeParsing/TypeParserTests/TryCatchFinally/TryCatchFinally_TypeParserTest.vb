Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.TryCatchFinally
	<TestFixture()>
	Public Class TryCatchFinally_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_SafeCallInsideTry_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("SafeCallInsideTry", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallInsideTry_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("UnsafeCallInsideTry", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallInsideCatch_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("SafeCallInsideCatch", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallInsideCatch_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("UnsafeCallInsideCatch", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallInsideFinally_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("SafeCallInsideFinally", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallInsideFinally_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("UnsafeCallInsideFinally", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallNestedTry_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("UnsafeCallNestedTry", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallNestedCatch_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("UnsafeCallNestedCatch", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallNestedFinally_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("UnsafeCallNestedFinally", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
