Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.AnonymousMethod
	<TestFixture()>
	Public Class AnonymousMethod_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_SafeDelegateCall_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of AnonymousMethodSample)("SafeAnonymousMethodCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeAnonymousMethodCall_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of AnonymousMethodSample)("UnsafeAnonymousMethodCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeAnonymousMethodCallUsingReturn_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of AnonymousMethodSample)("SafeAnonymousMethodCallUsingReturn", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SafeMethodCallInsideAnonymousMethod_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of AnonymousMethodSample)("SafeMethodCallInsideAnonymousMethod", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeMethodCallInsideAnonymousMethod_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of AnonymousMethodSample)("UnsafeMethodCallInsideAnonymousMethod", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeReturnInsideAnonymousMethod_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of AnonymousMethodSample)("SafeReturnInsideAnonymousMethod", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeReturnInsideAnonymousMethod_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of AnonymousMethodSample)("UnsafeReturnInsideAnonymousMethod", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
