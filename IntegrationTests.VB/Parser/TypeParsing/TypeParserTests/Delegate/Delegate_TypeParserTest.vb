Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.[Delegate]
	<TestFixture()>
	Public Class Delegate_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_SafeDelegateCall_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of DelegateSample)("SafeDelegateCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeDelegateCall_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of DelegateSample)("UnsafeDelegateCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeDelegateCallUsingReturn_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of DelegateSample)("SafeDelegateCallUsingReturn", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SafeDelegateFieldCall_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of DelegateSample)("SafeDelegateFieldCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeDelegateFieldCall_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of DelegateSample)("UnsafeDelegateFieldCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_DelegateWithSafeReturn_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of DelegateSample)("DelegateWithSafeReturn", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_DelegateWithUnsafeReturn_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of DelegateSample)("DelegateWithUnsafeReturn", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
