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

Namespace Utilities
	Friend Class IntrospectionUtility_ClassSample
		Private Class NestedClass
		End Class

		Public Delegate Function Closure() As String

		Private _field As String = "dummy"

		Public Property AnyProperty() As Object

		Public Sub UsingField()
			Me.Dummy(Me._field)
		End Sub

		Public Sub FieldAssignment()
			Me._field = "dummy"
		End Sub

		Public Function NonFieldAssignment() As String
			Return Me.Dummy("dummy")
		End Function

		Public Function Dummy(parameter As String) As String
			Return parameter
		End Function

		Public Function get_NonExistingProperty() As String
			Dim nested As IntrospectionUtility_ClassSample.NestedClass = New IntrospectionUtility_ClassSample.NestedClass()
      Return "dummy" & nested.ToString()
		End Function

		Public Function get_NonExistingProperty(parameter As String) As String
			Return parameter
		End Function

		Public Sub set_NonExistingProperty()
		End Sub

		Public Function ArrayVariableAndIndexer() As Object()
      Dim objectArray As Object() = {5}
			objectArray(0) = New Object()
			Return objectArray
		End Function

		Public Function UsingClosure() As String
			Dim environmentVariable As String = "environment"
			Dim closure As IntrospectionUtility_ClassSample.Closure = Function() environmentVariable
			Return closure()
		End Function
	End Class
End Namespace
