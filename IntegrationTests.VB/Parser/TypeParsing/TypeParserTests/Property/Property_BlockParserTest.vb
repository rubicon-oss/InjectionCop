Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.[Property]
	<TestFixture()>
	Public Class Property_BlockParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_CallWithUnsafeProperty_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of PropertySample)("CallWithUnsafeProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_CallWithSafePropertyVerboseAnnotation_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of PropertySample)("CallWithSafePropertyVerboseAnnotation", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SetSafePropertyVerboseAnnotationWithSafeValue_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of PropertySample)("SetSafePropertyVerboseAnnotationWithSafeValue", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SetSafePropertyVerboseAnnotationWithUnsafeValue_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of PropertySample)("SetSafePropertyVerboseAnnotationWithUnsafeValue", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_CallWithSafeProperty_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of PropertySample)("CallWithSafeProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SetSafePropertyWithSafeValue_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of PropertySample)("SetSafePropertyWithSafeValue", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SetSafePropertyWithUnsafeValue_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of PropertySample)("SetSafePropertyWithUnsafeValue", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
