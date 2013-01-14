Imports System

Namespace Parser.TypeParsing.TypeParserTests.AttributePropagation
	Friend Class AttributePropagationSample
		Inherits ParserSampleBase

		Public Sub SafeCallOfSqlFragmentCallee()
			MyBase.RequiresSqlFragment(MyBase.SafeSource())
		End Sub

		Public Function UnsafeCallOfSqlFragmentCallee() As String
			Return MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
		End Function

		Public Function SafeCallOfMixedCallee() As String
			Return MyBase.RequiresSqlFragment("literal", MyBase.UnsafeSource(), MyBase.SafeSource())
		End Function

		Public Sub UnsafeCallOfMixedCallee()
			MyBase.RequiresSqlFragment("literal", MyBase.SafeSource(), MyBase.UnsafeSource())
		End Sub
	End Class
End Namespace
