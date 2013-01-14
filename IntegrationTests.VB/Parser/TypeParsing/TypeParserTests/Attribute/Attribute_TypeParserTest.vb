Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Attribute
	<TestFixture()>
	Public Class Attribute_TypeParserTest
		Inherits TypeParserTestBase

		<Category("Attribute"), Test()>
		Public Sub Parse_ParameterSampleType_NoProblem()
			Dim sample As TypeNode = IntrospectionUtility.TypeNodeFactory(Of SampleAttribute)()
			Dim result As ProblemCollection = Me._typeParser.Check(sample)
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub
	End Class
End Namespace
