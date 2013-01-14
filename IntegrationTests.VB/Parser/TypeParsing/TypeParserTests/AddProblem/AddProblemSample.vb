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
Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.AddProblem
  Friend Class AddProblemSample
    Inherits ParserSampleBase

    Public Sub NoViolation()
      MyBase.RequiresSqlFragment(MyBase.SafeSource())
    End Sub

    Public Sub OneViolation()
      MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
    End Sub

    Public Sub TwoViolations()
      MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
      MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
    End Sub

    Public Sub OneViolationInWhile()
      Dim i As Integer = 0
      Dim source As String = MyBase.UnsafeSource()
      While i < 10
        MyBase.RequiresSqlFragment(source)
        i += 1
      End While
    End Sub

    Public Sub OneViolationInWhileAssignmentAfterCall()
      Dim i As Integer = 0
      Dim source As String = "safe"
      While i < 10
        MyBase.RequiresSqlFragment(source)
        source = MyBase.UnsafeSource()
        i += 1
      End While
    End Sub

    Public Sub TwoViolationsInNestedWhile()
      Dim i As Integer = 0
      Dim sqlSource As String = MyBase.UnsafeSource()
      While i < 10
        Dim source As Integer = 0
        While i < 5
          MyBase.RequiresSqlFragment(sqlSource)
          Me.RequiresValidatedFragment(source)
          source = Me.UnsafeIntSource()
          i += 1
        End While
        i += 1
      End While
    End Sub

    Public Function RequiresValidatedFragment(<Fragment("Validated")> source As Integer) As Integer
      Return source
    End Function

    Public Function UnsafeIntSource() As Integer
      Return 0
    End Function
  End Class
End Namespace
