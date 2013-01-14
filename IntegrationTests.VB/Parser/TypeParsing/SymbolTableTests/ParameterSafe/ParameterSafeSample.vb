Imports System

Namespace Parser.TypeParsing.SymbolTableTests.ParameterSafe
	Friend Class ParameterSafeSample
		Inherits ParserSampleBase

		Public Function DeliverFragmentWhenNotExpected() As Boolean
			Return"dummy" = Me.doSomething(MyBase.SafeSource())
		End Function

		Public Sub CallWithoutFragments()
			Me.doSomething(MyBase.UnsafeSource())
		End Sub

		Private Function doSomething(parameter As String) As String
			Return parameter
		End Function
	End Class
End Namespace
