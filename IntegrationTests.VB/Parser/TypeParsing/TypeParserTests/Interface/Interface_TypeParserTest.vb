Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.[Interface]
	<TestFixture()>
	Public Class Interface_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Check_InterfaceSample_NoProblem()
			Dim sample As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InterfaceSample)()
			Me._typeParser.Check(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub
	End Class
End Namespace
