Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Inheritance.[Interface]
	Public Interface IInheritanceSampleDuplicate
		Function MethodWithFragmentParameter(<Fragment("InterfaceFragment")> fragmentParameter As String, nonFragmentParameter As String) As String

		Function MethodWithReturnFragment() As<Fragment("InterfaceFragment")>
		String
	End Interface
End Namespace
