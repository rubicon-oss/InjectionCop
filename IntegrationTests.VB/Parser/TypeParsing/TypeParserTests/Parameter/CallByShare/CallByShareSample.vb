Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Parameter.CallByShare
	Friend Class CallByShareSample
		Inherits ParserSampleBase

		Public Sub UnsafeMethodParameter(unsafeParam As String)
			MyBase.RequiresSqlFragment(unsafeParam)
		End Sub

		Public Sub SafeMethodParameter(<Fragment("SqlFragment")> safeParam As String)
			MyBase.RequiresSqlFragment(safeParam)
		End Sub
	End Class
End Namespace
