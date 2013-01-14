Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.FragmentUsageTests
	Public Class BaseClass
		Public Overridable Sub Foo(<Fragment("ValidFragmentType")> parameter1 As Integer, parameter2 As String)
		End Sub

		Public Overridable Sub MethodWithFragmentOnSecondParameter(parameter1 As Integer, <Fragment("ValidFragmentType")> parameter2 As Integer)
		End Sub
	End Class
End Namespace
