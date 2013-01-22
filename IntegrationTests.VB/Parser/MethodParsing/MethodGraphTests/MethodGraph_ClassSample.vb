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
Imports System
Imports System.Globalization

Namespace Parser.MethodParsing.MethodGraphTests
  Public Class MethodGraph_ClassSample
    Inherits ParserSampleBase

    Public Function DeclarationWithReturn() As <Fragment("ReturnFragmentType")>
    Integer
      Return 3
    End Function

    Public Function IfStatementTrueBlockOnly(param As String) As String
      If param = "dummy" Then
        param = "changed"
      End If
      Return param
    End Function

    Public Function ForLoop() As Integer
      Dim result As Integer = 0
      For i As Integer = 10 To 1 Step -1
        result += i
      Next
      Return result
    End Function

    Public Function ValidReturnWithIf(<Fragment("DummyFragment")> parameter As String) As <Fragment("DummyFragment")>
    String
      Dim returnValue As String = ""
      If parameter = "Dummy" Then
        returnValue = "safe"
      End If
      Return returnValue
    End Function

    Public Sub FragmentOutParameterSafeReturn(<Fragment("SqlFragment")> ByRef safe As String)
      Dim temp As String = "safe"
      safe = "safe"
      If MyBase.SafeSource() = "dummy" Then
        safe = temp
      End If
    End Sub

    Public Sub FragmentRefParameterSafeReturn(<Fragment("SqlFragment")> ByRef safe As String)
      MyBase.DummyMethod(safe)
      Dim temp As String = "safe"
      safe = "safe"
      If MyBase.SafeSource() = "dummy" Then
        safe = temp
      End If
    End Sub

    Public Sub TryCatchFinally()
      Try
        MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
        Me.ThrowsException(0)
      Catch ex As System.ArgumentNullException
        MyBase.DummyMethod(ex.Message)
      Finally
        MyBase.DummyMethod("")
      End Try
      MyBase.DummyMethod("dummy")
    End Sub

    Private Sub ThrowsException(parameter As Integer)
      If parameter = 1 Then
        Throw New System.ArgumentNullException()
      End If
      MyBase.DummyMethod(parameter.ToString(System.Globalization.CultureInfo.InvariantCulture))
    End Sub
  End Class
End Namespace
