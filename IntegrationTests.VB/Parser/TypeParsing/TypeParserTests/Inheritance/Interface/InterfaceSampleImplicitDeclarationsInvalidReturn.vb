Imports System

Namespace Parser.TypeParsing.TypeParserTests.Inheritance.[Interface]
	Public Class InterfaceSampleImplicitDeclarationsInvalidReturn
		Inherits ParserSampleBase
		Implements IInheritanceSample

    Public Function MethodWithFragmentParameter(fragmentParameter As String, nonFragmentParameter As String) As String _
      Implements IInheritanceSample.MethodWithFragmentParameter

      Return "dummy"
    End Function

    Public Function MethodWithReturnFragment() As String _
      Implements IInheritanceSample.MethodWithReturnFragment

      Return MyBase.UnsafeSource()
    End Function
  End Class
End Namespace
