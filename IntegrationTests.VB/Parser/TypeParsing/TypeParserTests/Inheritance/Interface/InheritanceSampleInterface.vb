Imports System

Namespace Parser.TypeParsing.TypeParserTests.Inheritance.[Interface]
	Public Class InheritanceSampleInterface
		Inherits ParserSampleBase

		Public Sub SafeCallOnInterfaceMethodWithFragmentParameter()
			Dim sample As IInheritanceSample = New InterfaceSampleImplicitDeclarations()
			sample.MethodWithFragmentParameter("safe", "safe")
		End Sub

		Public Sub UnsafeCallOnInterfaceMethodWithFragmentParameter()
			Dim sample As IInheritanceSample = New InterfaceSampleImplicitDeclarations()
			sample.MethodWithFragmentParameter(MyBase.UnsafeSource(), "safe")
		End Sub

		Public Sub InterfaceReturnFragmentsAreConsidered()
			Dim sample As IInheritanceSample = New InterfaceSampleImplicitDeclarations()
			sample.MethodWithFragmentParameter(sample.MethodWithReturnFragment(), "safe")
		End Sub

		Public Sub SafeCallOnExplicitInterfaceMethodWithFragmentParameter()
			Dim sample As IInheritanceSample = New InterfaceSampleExplicitDeclarations()
			sample.MethodWithFragmentParameter("safe", "safe")
		End Sub

		Public Sub UnsafeCallOnExplicitInterfaceMethodWithFragmentParameter()
			Dim sample As IInheritanceSample = New InterfaceSampleExplicitDeclarations()
			sample.MethodWithFragmentParameter(MyBase.UnsafeSource(), "safe")
		End Sub

		Public Sub InterfaceReturnFragmentsOfExplicitlyDeclaredMethodAreConsidered()
			Dim sample As IInheritanceSample = New InterfaceSampleExplicitDeclarations()
			sample.MethodWithFragmentParameter(sample.MethodWithReturnFragment(), "safe")
		End Sub

		Public Sub SafeCallOnClassImplementingInterfaceMethodWithFragmentParameter()
			Dim sample As InterfaceSampleImplicitDeclarations = New InterfaceSampleImplicitDeclarations()
			sample.MethodWithFragmentParameter("safe", "safe")
		End Sub

		Public Sub UnsafeCallOnClassImplementingInterfaceMethodWithFragmentParameter()
			Dim sample As InterfaceSampleImplicitDeclarations = New InterfaceSampleImplicitDeclarations()
			sample.MethodWithFragmentParameter(MyBase.UnsafeSource(), "safe")
		End Sub

		Public Sub InterfaceReturnFragmentsOfClassImplementingInterfaceMethodAreConsidered()
			Dim sample As InterfaceSampleImplicitDeclarations = New InterfaceSampleImplicitDeclarations()
			sample.MethodWithFragmentParameter(sample.MethodWithReturnFragment(), "safe")
		End Sub

		Public Sub SafeCallOnClassImplementingMethodWithFragmentParameterFromMultipleInterfaces()
			Dim sample As InterfaceSampleImplicitDeclarationsMultipleInheritance = New InterfaceSampleImplicitDeclarationsMultipleInheritance()
			sample.MethodWithFragmentParameter("safe", "safe")
		End Sub

		Public Sub UnsafeCallOnClassImplementingMethodWithFragmentParameterFromMultipleInterfaces()
			Dim sample As InterfaceSampleImplicitDeclarationsMultipleInheritance = New InterfaceSampleImplicitDeclarationsMultipleInheritance()
			sample.MethodWithFragmentParameter(MyBase.UnsafeSource(), "safe")
		End Sub

		Public Sub InterfaceReturnFragmentsOfClassImplementingInterfaceMethodFromMultipleInterfacesAreConsidered()
			Dim sample As InterfaceSampleImplicitDeclarationsMultipleInheritance = New InterfaceSampleImplicitDeclarationsMultipleInheritance()
			sample.MethodWithFragmentParameter(sample.MethodWithReturnFragment(), "safe")
		End Sub

		Public Sub SafeCallOnExtendedInterfaceMethodWithFragmentParameter()
			Dim sample As IInheritanceSampleDuplicate = New ExtendedInterfaceSampleImplicitDeclarations()
			sample.MethodWithFragmentParameter("safe", "safe")
		End Sub

		Public Sub UnsafeCallOnExtendedInterfaceMethodWithFragmentParameter()
			Dim sample As IInheritanceSample = New ExtendedInterfaceSampleImplicitDeclarations()
			sample.MethodWithFragmentParameter(MyBase.UnsafeSource(), "safe")
		End Sub

		Public Sub ExtendedInterfaceReturnFragmentsAreConsidered()
			Dim sample As IInheritanceSampleDuplicate = New ExtendedInterfaceSampleImplicitDeclarations()
			sample.MethodWithFragmentParameter(sample.MethodWithReturnFragment(), "safe")
		End Sub
	End Class
End Namespace
