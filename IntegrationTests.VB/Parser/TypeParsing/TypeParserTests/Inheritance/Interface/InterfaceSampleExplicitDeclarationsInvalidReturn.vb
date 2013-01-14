Imports System

Namespace Parser.TypeParsing.TypeParserTests.Inheritance.[Interface]
	Public Class InterfaceSampleExplicitDeclarationsInvalidReturn
		Inherits ParserSampleBase
		Implements IInheritanceSample

		Function MethodWithFragmentParameter(fragmentParameter As String, nonFragmentParameter As String) As String Implements IInheritanceSample.MethodWithFragmentParameter
			Return"dummy"
		End Function

		Function MethodWithReturnFragment() As String Implements IInheritanceSample.MethodWithReturnFragment
			Return MyBase.UnsafeSource()
		End Function
	End Class
End Namespace
