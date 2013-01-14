Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.FragmentUsageTests
	Public Class DerivedClassWithValidFragmentUsage
		Inherits BaseClass

    Public Overrides Sub Foo(<Fragment("ValidFragmentType")> parameter1 As Integer, parameter2 As String)
    End Sub
	End Class
End Namespace
