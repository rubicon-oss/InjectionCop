Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.[Return]
  Friend Class ReturnSample
    Inherits ParserSampleBase

    <Fragment("DummyType")>
    Private _dummyTypeFragment As String = "safe"

    Public Function ReturnFragmentMismatch() As <Fragment("DummyType")>
    String
      Return MyBase.UnsafeSource()
    End Function

    Public Function NoReturnAnnotation() As String
      Return MyBase.UnsafeSource()
    End Function

    Public Function DeclarationWithReturn() As <Fragment("ReturnFragmentType")>
    Integer
      Return 3
    End Function

    Public Function ReturnsDummyType() As <Fragment("DummyType")>
    String
      Return Me._dummyTypeFragment
    End Function

    Public Function ReturnsFieldWithWrongType() As <Fragment("DummyType")>
    String
      Return Me._fragmentField
    End Function
  End Class
End Namespace
