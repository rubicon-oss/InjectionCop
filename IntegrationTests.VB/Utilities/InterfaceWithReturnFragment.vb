Imports InjectionCop.Fragment
Imports System

Namespace Utilities
	Public Interface InterfaceWithReturnFragment
		Function MethodWithReturnFragment() As<Fragment("ReturnFragmentType")>
		Integer
	End Interface
End Namespace
