Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Parameter.CallByShare
	<TestFixture()>
	Public Class CallByShare_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_UnsafeMethodParameter_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of CallByShareSample)("UnsafeMethodParameter", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeMethodParameter_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of CallByShareSample)("SafeMethodParameter", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub
	End Class
End Namespace
