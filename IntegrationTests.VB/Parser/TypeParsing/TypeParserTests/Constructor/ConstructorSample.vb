Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Constructor
  Public Class ConstructorSample
    Inherits ParserSampleBase

    Public Sub New()
    End Sub

    Public Sub New(nonFragmentParameter As String)
      MyBase.DummyMethod(nonFragmentParameter)
    End Sub

    Public Sub New(nonFragmentParameter As String, <Fragment("ConstructorFragment")> fragmentParameter As String)
      MyBase.DummyMethod(nonFragmentParameter)
      Me.RequiresConstructorFragment(fragmentParameter)
    End Sub

    Public Sub New(nonFragmentParameter As String, <Fragment("ConstructorFragment")> fragmentParameter As String, dummy As String)
      MyBase.DummyMethod(nonFragmentParameter + fragmentParameter + dummy)
      Me.RequiresConstructorFragment(nonFragmentParameter)
    End Sub

    Public Sub New(nonFragmentParameter As String, <Fragment("ConstructorFragment")> fragmentParameter As String, dummy1 As String, dummy2 As String)
      Me.New(nonFragmentParameter, fragmentParameter)
      MyBase.DummyMethod(nonFragmentParameter + fragmentParameter + dummy1 + dummy2)
    End Sub

    Public Sub New(nonFragmentParameter As String, <Fragment("ConstructorFragment")> fragmentParameter As String, dummy1 As String, dummy2 As String, dummy3 As String)
      Me.New(nonFragmentParameter, nonFragmentParameter, dummy1)
      MyBase.DummyMethod(nonFragmentParameter)
      MyBase.DummyMethod(fragmentParameter)
      MyBase.DummyMethod(dummy1)
      MyBase.DummyMethod(dummy2)
      MyBase.DummyMethod(dummy3)
    End Sub

    Public Sub RequiresConstructorFragment(<Fragment("ConstructorFragment")> fragmentParameter As String)
      MyBase.DummyMethod(fragmentParameter)
    End Sub
  End Class
End Namespace
