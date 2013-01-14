Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Concatenation
  Public Class ConcatenationSample
    Inherits ParserSampleBase

    Public Sub CompilerOptimizationConcatenation(nonFragmentParameter As String, <Fragment("ConstructorFragment")> fragmentParameter As String, dummy1 As String, dummy2 As String, dummy3 As String)
      MyBase.DummyMethod(String.Concat(New String() {nonFragmentParameter, fragmentParameter, dummy1, dummy2, dummy3}))
    End Sub
  End Class
End Namespace
