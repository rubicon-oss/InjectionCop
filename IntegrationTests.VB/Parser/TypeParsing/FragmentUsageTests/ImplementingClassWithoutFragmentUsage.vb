Imports System

Namespace Parser.TypeParsing.FragmentUsageTests
	Public Class ImplementingClassWithoutFragmentUsage
		Implements IBaseInterface

    Public Sub Foo(parameter1 As Integer, parameter2 As String) Implements IBaseInterface.Foo
    End Sub
	End Class
End Namespace
