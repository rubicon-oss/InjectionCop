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
Imports InjectionCop.Attributes
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
