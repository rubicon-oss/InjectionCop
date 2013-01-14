Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.[Return]
	<TestFixture()>
	Public Class Return_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_SafeSource_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of ParserSampleBase)("SafeSource", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeSource_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of ReturnSample)("ReturnFragmentMismatch", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_NoReturnAnnotation_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of ReturnSample)("NoReturnAnnotation", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_DeclarationWithReturn_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of ReturnSample)("DeclarationWithReturn", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_ReturnsDummyType_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of ReturnSample)("ReturnsDummyType", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_ReturnsFieldWithWrongType_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of ReturnSample)("ReturnsFieldWithWrongType", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
