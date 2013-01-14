Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Inheritance.Field
	Public Class InheritanceSampleField
		Inherits InheritanceSampleBase

    <Fragment("InheritanceFragment")>
    Protected Shadows _initialNonFragmentField As String

    Protected Shadows _initialFragmentField As String

		Public Sub New()
      MyBase.New("safe", "safe")
		End Sub

		Protected Sub UnsafeAssignmentOnInheritedField()
			Me._fragmentField = InheritanceSampleBase.UnsafeInheritanceFragmentSource()
		End Sub

		Protected Sub SafeAssignmentOnInheritedFieldWithLiteral()
			Me._fragmentField = "safe"
		End Sub

		Protected Sub SafeAssignmentOnInheritedField()
			Me._fragmentField = MyBase.SafeInheritanceFragmentSource()
		End Sub

		Protected Sub SafeAssignmentOnFieldSetByHidingParent()
			Me._initialNonFragmentField = MyBase.SafeInheritanceFragmentSource()
		End Sub

		Protected Sub UnsafeAssignmentOnFieldResetByHidingParent()
			Me._initialFragmentField = InheritanceSampleBase.UnsafeInheritanceFragmentSource()
		End Sub

		Protected Sub SafeCallWithInheritedField()
			MyBase.RequiresInheritanceFragment(Me._fragmentField)
		End Sub

		Protected Sub UnsafeCallWithInheritedField()
			MyBase.RequiresInheritanceFragment(Me._nonFragmentField)
		End Sub

		Protected Sub SafeCallWithFieldSetByHidingParentField()
			MyBase.RequiresInheritanceFragment(Me._initialNonFragmentField)
		End Sub

		Protected Sub UnsafeCallWithFieldSetByHidingParentField()
			MyBase.RequiresInheritanceFragment(Me._initialFragmentField)
		End Sub

		Protected Sub SafeAssignmentOnBaseField()
      MyBase._initialFragmentField = MyBase.SafeInheritanceFragmentSource()
		End Sub

		Protected Sub UnsafeAssignmentOnBaseField()
      MyBase._initialFragmentField = InheritanceSampleBase.UnsafeInheritanceFragmentSource()
		End Sub

		Protected Sub SafeAssignmentOfBaseField()
      Me._fragmentField = MyBase._initialFragmentField
		End Sub

		Protected Sub UnsafeAssignmentOfBaseField()
      Me._fragmentField = MyBase._initialNonFragmentField
		End Sub

		Public Sub SafeMethodCallUsingBaseField()
      MyBase.RequiresInheritanceFragment(MyBase._initialFragmentField)
		End Sub

		Public Sub UnsafeMethodCallUsingBaseField()
      MyBase.RequiresInheritanceFragment(MyBase._initialNonFragmentField)
		End Sub
	End Class
End Namespace
