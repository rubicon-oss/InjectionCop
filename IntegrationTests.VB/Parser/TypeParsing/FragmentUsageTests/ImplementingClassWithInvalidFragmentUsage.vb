Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.FragmentUsageTests
	Public Class ImplementingClassWithInvalidFragmentUsage
		Implements IBaseInterface

    Public Sub Foo(<Fragment("InvalidFragmentType")> parameter1 As Integer, parameter2 As String) Implements IBaseInterface.Foo
    End Sub
	End Class
End Namespace
