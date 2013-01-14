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
Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Inheritance.[Property]
	Friend Class InheritanceSampleProperty
		Inherits InheritanceSampleBase

    <Fragment("InheritanceFragment")>
    Public Shadows Property InitialNonFragmentProperty() As String

    Public Shadows Property InitialFragmentProperty() As String

    <Fragment("InheritanceFragment")>
    Public Overrides Property VirtualProperty() As String

		Public Sub New()
      MyBase.New("safe", "safe")
		End Sub

		Protected Sub UnsafeAssignmentOnInheritedProperty()
			MyBase.FragmentProperty = InheritanceSampleBase.UnsafeInheritanceFragmentSource()
		End Sub

		Protected Sub SafeAssignmentOnInheritedPropertyWithLiteral()
			MyBase.FragmentProperty = "safe"
		End Sub

		Protected Sub SafeAssignmentOnInheritedProperty()
			MyBase.FragmentProperty = MyBase.SafeInheritanceFragmentSource()
		End Sub

		Protected Sub SafeAssignmentOnPropertyHidingParent()
			Me.InitialNonFragmentProperty = MyBase.SafeInheritanceFragmentSource()
		End Sub

		Protected Sub UnsafeAssignmentOnPropertyHidingParent()
			Me.InitialFragmentProperty = InheritanceSampleBase.UnsafeInheritanceFragmentSource()
		End Sub

		Protected Sub SafeCallWithInheritedProperty()
			MyBase.RequiresInheritanceFragment(MyBase.FragmentProperty)
		End Sub

		Protected Sub UnsafeCallWithInheritedProperty()
			MyBase.RequiresInheritanceFragment(MyBase.NonFragmentProperty)
		End Sub

		Protected Sub SafeCallWithPropertyHidingParent()
			MyBase.RequiresInheritanceFragment(Me.InitialNonFragmentProperty)
		End Sub

		Protected Sub UnsafeCallWithPropertyHidingParent()
			MyBase.RequiresInheritanceFragment(Me.InitialFragmentProperty)
		End Sub

		Public Sub SafeStaticBindingOnNewProperty()
			Dim sample As InheritanceSampleBase = New InheritanceSampleProperty()
			sample.FragmentProperty = MyBase.SafeInheritanceFragmentSource()
		End Sub

		Public Sub UnsafeStaticBindingOnNewProperty()
			Dim sample As InheritanceSampleBase = New InheritanceSampleProperty()
			sample.FragmentProperty = InheritanceSampleBase.UnsafeInheritanceFragmentSource()
		End Sub

		Public Sub SafeSetOnOverriddenProperty()
			Me.VirtualProperty = MyBase.SafeInheritanceFragmentSource()
		End Sub

		Public Sub AnotherSafeSetOfOverriddenProperty()
			Me.VirtualProperty = "safe"
		End Sub

		Public Sub UnsafeSetOnOverriddenProperty()
			Me.VirtualProperty = InheritanceSampleBase.UnsafeInheritanceFragmentSource()
		End Sub

		Public Sub SafeDynamicBindingOnProperty()
			Dim sample As InheritanceSampleBase = New InheritanceSampleProperty()
			sample.VirtualProperty = MyBase.SafeInheritanceFragmentSource()
		End Sub

		Public Sub UnsafeDynamicBindingOnProperty()
			Dim sample As InheritanceSampleBase = New InheritanceSampleProperty()
			sample.VirtualProperty = InheritanceSampleBase.UnsafeInheritanceFragmentSource()
		End Sub

		Protected Sub SafeAssignmentOnBaseProperty()
      MyBase.InitialFragmentProperty = SafeInheritanceFragmentSource()
		End Sub

		Protected Sub UnsafeAssignmentOnBaseProperty()
      MyBase.InitialFragmentProperty = UnsafeInheritanceFragmentSource()
		End Sub

		Protected Sub SafeAssignmentOfBaseProperty()
      FragmentProperty = MyBase.InitialFragmentProperty
		End Sub

		Protected Sub UnsafeAssignmentOfBaseProperty()
      FragmentProperty = MyBase.InitialNonFragmentProperty
		End Sub

		Public Sub SafeMethodCallUsingBaseProperty()
      RequiresInheritanceFragment(MyBase.InitialFragmentProperty)
		End Sub

		Public Sub UnsafeMethodCallUsingBaseProperty()
      RequiresInheritanceFragment(MyBase.InitialNonFragmentProperty)
		End Sub
	End Class
End Namespace
