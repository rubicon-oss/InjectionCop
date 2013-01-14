Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.[Goto]
	<TestFixture()>
	Public Class Goto_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_SimpleGoto_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of GotoSample)("SimpleGoto", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_GotoJumpsOverUnsafeAssignment_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of GotoSample)("GotoJumpsOverUnsafeAssignment", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InvalidCallInsideWhileWithGoto_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of GotoSample)("InvalidCallInsideWhileWithGoto", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InvalidCallInsideIfWithGoto_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of GotoSample)("InvalidCallInsideIfWithGoto", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InvalidCallInsideIfWithGotoAndBreak_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of GotoSample)("InvalidCallInsideIfWithGotoAndBreak", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
