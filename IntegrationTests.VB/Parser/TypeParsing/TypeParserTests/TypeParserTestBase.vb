Imports InjectionCop.Parser.TypeParsing
Imports InjectionCop.Utilities
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests
	Public Class TypeParserTestBase
		Protected _typeParser As TypeParser

		Protected c_InjectionCopRuleId As String = "IC0001"

		<SetUp()>
		Public Sub SetUp()
			Me._typeParser = New TypeParser()
			Me._typeParser.InitializeBlacklistManager(IntrospectionUtility.TypeNodeFactory(Of TypeParserTestBase)())
		End Sub
	End Class
End Namespace
