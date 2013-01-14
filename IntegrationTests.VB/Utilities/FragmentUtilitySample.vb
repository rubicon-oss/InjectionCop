Imports InjectionCop.Fragment
Imports InjectionCop.IntegrationTests.VB.Parser
Imports System

Namespace Utilities
  Friend Class FragmentUtilitySample
    Public Sub ContainsFragmentParameter(<Fragment("FragmentType")> parameter As String)
    End Sub

    Public Sub NoFragmentParameter(parameter As String)
    End Sub

    Public Sub ContainsNonFragmentParameter(<NonFragment("FragmentType")> parameter As String)
    End Sub

    Public Sub ContainsSqlFragmentParameter(<Fragment("SqlFragment")> parameter As String)
    End Sub

    Public Sub ContainsStronglyTypedSqlFragmentParameter(<SqlFragment()> parameter As String)
    End Sub

    Public Function NoReturnFragment() As Integer
      Return 3
    End Function

    Public Function ReturnFragment() As <Fragment("ReturnFragmentType")>
    Integer
      Return 3
    End Function
  End Class
End Namespace
