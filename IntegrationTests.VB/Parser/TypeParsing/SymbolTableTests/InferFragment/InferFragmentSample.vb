Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.SymbolTableTests.InferFragment
  Friend Class InferFragmentSample
    Inherits ParserSampleBase

    <Fragment("SampleFragment")>
    Private _fragmentField As String = "dummy"

    Private _nonFragmentField As String = "dummy"

    <Fragment("PropertyFragmentType")>
    Public Property PropertyWithFragment() As Object

    Public Property PropertyWithoutFragment() As Integer

    Public Function AssignmentWithLiteral() As Integer
      Return 3
    End Function

    Public Function AssignmentWithLocal() As String
      Dim x As String = "safe"
      MyBase.DummyMethod(x)
      Return x
    End Function

    Public Function AssignmentWithParameter(parameter As Integer) As Integer
      Return parameter
    End Function

    Public Function AssignmentWithSafeMethodCall() As String
      Return MyBase.SafeSource()
    End Function

    Public Function AssignmentWithUnsafeMethodCall() As String
      Return MyBase.UnsafeSource()
    End Function

    Public Function AssignmentWithFragmentField() As String
      Return Me._fragmentField
    End Function

    Public Function AssignmentWithNonFragmentField() As String
      Return Me._nonFragmentField
    End Function

    Public Function AssignmentWithFragmentProperty() As Object
      Return Me.PropertyWithFragment
    End Function
  End Class
End Namespace
