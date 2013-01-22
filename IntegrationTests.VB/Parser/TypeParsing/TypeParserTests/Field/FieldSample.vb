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

Namespace Parser.TypeParsing.TypeParserTests.Field
  Friend Class FieldSample
    Inherits ParserSampleBase

    <SqlFragment()>
    Private _safeField As String = "safe"

    <Fragment("SqlFragment")>
    Private _safeFieldDifferentAnnotation As String = "safe_too"

    Private _unsafeField As String = "dummy"

    <Fragment("AnyType")>
    Private _otherSafeField As String = FieldSample.StaticUnsafeSource()

    Public Sub CallWithSafeField()
      MyBase.RequiresSqlFragment(Me._safeField)
    End Sub

    Public Sub CallWithUnsafeField()
      MyBase.RequiresSqlFragment(Me._unsafeField)
    End Sub

    Public Sub CallWithWrongFragmentType()
      MyBase.RequiresSqlFragment(MyBase.ReturnsHtmlFragment())
    End Sub

    Public Sub NestedUnsafeCall()
      Dim i As Integer = 0
      While __PostIncrement(i) < 5
        MyBase.RequiresSqlFragment(MyBase.SafeSourceRequiresSqlFragment(Me._unsafeField))
      End While
    End Sub

    Public Sub NestedSafeCall()
      Dim i As Integer = 0
      While __PostIncrement(i) < 5
        MyBase.RequiresSqlFragment(MyBase.SafeSourceRequiresSqlFragment("safe"))
      End While
    End Sub

    Public Sub NestedCallWithWrongFragmentType()
      Dim i As Integer = 0
      While __PostIncrement(i) < 5
        MyBase.RequiresSqlFragment(MyBase.SafeSourceRequiresSqlFragment(MyBase.ReturnsHtmlFragment()))
      End While
    End Sub

    Private Function __PostIncrement(i As Integer) As Integer
      Return i + 1
    End Function

    Public Sub UnsafeFieldAssignment()
      Me._safeField = MyBase.UnsafeSource()
    End Sub

    Public Sub SafeFieldAssignment()
      Me._safeField = MyBase.SafeSource()
    End Sub

    Public Sub SafeFieldAssignmentWithLiteral()
      Me._safeField = "safe"
    End Sub

    Public Sub UnsafeFieldAssignmentWithField()
      Me._safeField = Me._unsafeField
    End Sub

    Public Sub SafeFieldAssignmentWithField()
      Me._safeField = Me._safeFieldDifferentAnnotation
    End Sub

    Public Sub WrongFragmentTypeFieldAssignment()
      Me._safeField = MyBase.ReturnsHtmlFragment()
    End Sub

    Public Shared Function StaticUnsafeSource() As String
      Return "unsafe"
    End Function

    Public Function Dummy() As String
      Return Me._unsafeField + Me._otherSafeField
    End Function
  End Class
End Namespace
