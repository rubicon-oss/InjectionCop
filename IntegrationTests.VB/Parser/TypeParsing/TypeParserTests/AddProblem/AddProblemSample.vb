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
