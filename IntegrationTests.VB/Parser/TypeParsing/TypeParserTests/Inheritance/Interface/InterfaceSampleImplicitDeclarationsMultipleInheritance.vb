Imports System

Namespace Parser.TypeParsing.TypeParserTests.Inheritance.[Interface]
	Public Class InterfaceSampleImplicitDeclarationsMultipleInheritance
		Implements IInheritanceSample, IInheritanceSampleDuplicate

    Public Function MethodWithFragmentParameter(fragmentParameter As String, nonFragmentParameter As String) As String _
      Implements IInheritanceSample.MethodWithFragmentParameter, IInheritanceSampleDuplicate.MethodWithFragmentParameter

      Return "dummy"
    End Function

    Public Function MethodWithReturnFragment() As String _
      Implements IInheritanceSample.MethodWithReturnFragment, IInheritanceSampleDuplicate.MethodWithReturnFragment

      Return "safe"
    End Function
  End Class
End Namespace
