Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.FragmentUsageTests
	Public Interface IBaseInterface
		Sub Foo(<Fragment("ValidFragmentType")> parameter1 As Integer, parameter2 As String)
	End Interface
End Namespace
