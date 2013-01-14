Imports System

Namespace Parser.TypeParsing.TypeParserTests.Inheritance.[Interface]
	Public Class InterfaceSampleExplicitDeclarations
		Implements IInheritanceSample

		Function MethodWithFragmentParameter(fragmentParameter As String, nonFragmentParameter As String) As String Implements IInheritanceSample.MethodWithFragmentParameter
			Return"dummy"
		End Function

		Function MethodWithReturnFragment() As String Implements IInheritanceSample.MethodWithReturnFragment
			Return"safe"
		End Function
	End Class
End Namespace
