' Copyright 2013 rubicon informationstechnologie gmbh
'
' Licensed under the Apache License, Version 2.0 (the "License");
' you may not use this file except in compliance with the License.
' You may obtain a copy of the License at
'
' http://www.apache.org/licenses/LICENSE-2.0
'
' Unless required by applicable law or agreed to in writing, software
' distributed under the License is distributed on an "AS IS" BASIS,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the License for the specific language governing permissions and
' limitations under the License.
Imports InjectionCop.Attributes
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
