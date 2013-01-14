Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Lambda
	<TestFixture()>
	Public Class Lambda_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_SafeLambdaCall_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of LambdaSample)("SafeLambdaCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeLambdaCall_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of LambdaSample)("UnsafeLambdaCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeLambdaCallUsingReturn_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of LambdaSample)("SafeLambdaCallUsingReturn", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SafeMethodCallInsideLambda_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of LambdaSample)("SafeMethodCallInsideLambda", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeMethodCallInsideLambda_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of LambdaSample)("UnsafeMethodCallInsideLambda", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeReturnInsideLambda_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of LambdaSample)("SafeReturnInsideLambda", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeReturnInsideLambda_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of LambdaSample)("UnsafeReturnInsideLambda", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
