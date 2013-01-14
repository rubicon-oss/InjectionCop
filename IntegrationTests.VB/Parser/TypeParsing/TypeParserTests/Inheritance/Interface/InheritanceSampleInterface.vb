' Copyright 2013 rubicon informationstechnologie gmbh
'
' Licensed under the Apache License, Version 2.0 (the "License");
' you may not use this file except in compliance with the License.
' You may obtain a copy of the License at
'
' http://www.apache.org/licenses/LICENSE-2.0
'
' Unless required by applicable law or agreed to in writing, software
' distributed under the License is distributed on an "AS IS" BASIS,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the License for the specific language governing permissions and
' limitations under the License.
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
