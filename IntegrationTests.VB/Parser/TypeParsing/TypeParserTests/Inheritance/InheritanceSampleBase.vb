Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Inheritance
  Public Class InheritanceSampleBase
    Inherits ParserSampleBase

    <Fragment("InheritanceFragment")>
    Protected _fragmentField As String

    <Fragment("InheritanceFragment")>
    Protected _initialFragmentField As String

    Protected _nonFragmentField As String

    Protected _initialNonFragmentField As String

    Private _dummy As String

    <Fragment("InheritanceFragment")>
    Public Property FragmentProperty() As String

    <Fragment("InheritanceFragment")>
    Public Property InitialFragmentProperty() As String

    Public Property NonFragmentProperty() As String

    Public Property InitialNonFragmentProperty() As String

    <Fragment("InheritanceFragment")>
    Public Overridable Property VirtualProperty() As String

    Public Sub New(parameter As String)
      Me._fragmentField = "safe"
      Me._nonFragmentField = parameter
    End Sub

    Public Sub New(<Fragment("InheritanceFragment")> fragmentField As String, nonFragmentField As String)
      Me._fragmentField = fragmentField
      Me._nonFragmentField = nonFragmentField
    End Sub

    Public Overridable Function VirtualMethod(<Fragment("InheritanceFragment")> annotatedParameter As String, nonAnnotatedParameter As String) As String
      Return Nothing
    End Function

    Public Function NonVirtualMethod(<Fragment("InheritanceFragment")> annotatedParameter As String, nonAnnotatedParameter As String) As String
      Return Nothing
    End Function

    Public Function InvariantMethod(<Fragment("InheritanceFragment")> annotatedParameter As String, nonAnnotatedParameter As String) As String
      Return Nothing
    End Function

    Protected Shared Function UnsafeInheritanceFragmentSource() As String
      Return "unsafe"
    End Function

    Protected Function SafeInheritanceFragmentSource() As <Fragment("InheritanceFragment")>
    String
      Return "safe" + Me._dummy
    End Function

    Protected Sub RequiresInheritanceFragment(<Fragment("InheritanceFragment")> parameter As String)
      Me._dummy = parameter
    End Sub

    Protected Sub [New](p1 As String, p2 As String)
      Throw New NotImplementedException
    End Sub

  End Class
End Namespace
