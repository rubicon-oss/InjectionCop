Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.FragmentUsageTests
  Public Class DerivedClassWithInvalidFragmentUsage
    Inherits BaseClass

    Public Overrides Sub Foo(<Fragment("InvalidFragmentType")> parameter1 As Integer, parameter2 As String)
    End Sub

    Public Overrides Sub MethodWithFragmentOnSecondParameter(parameter1 As Integer, <Fragment("InvalidFragmentTypeOnSecondParameter")> parameter2 As Integer)
    End Sub
  End Class
End Namespace
