Imports InjectionCop.Fragment
Imports System

Namespace Parser.MethodParsing.MethodGraphTests
	Public Interface InterfaceSample
		Sub MethodNonAnnotated(param1 As Object, param2 As String)

		Sub MethodAnnotated(<Fragment("fragmentType")> param1 As Object, <SqlFragment()> fragment2 As Object)
	End Interface
End Namespace
