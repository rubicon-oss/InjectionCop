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
