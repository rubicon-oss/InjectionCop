Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.[Interface]
	Public Interface InterfaceSample
		Sub ParameterLess()

		Sub NonFragmentParameter(parameter As Object)

		Sub FragmentParameter(parameter1 As Integer, <Fragment("Type")> parameter2 As Object, <SqlFragment()> parameter3 As String)
	End Interface
End Namespace
