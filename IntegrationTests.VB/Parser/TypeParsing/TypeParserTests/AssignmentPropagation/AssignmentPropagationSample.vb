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

Namespace Parser.TypeParsing.TypeParserTests.AssignmentPropagation
	Friend Class AssignmentPropagationSample
		Inherits ParserSampleBase

		<Fragment("DummyFragment")>
		Private _dummyFragmentField As String = ""

		Public Sub ValidSafenessPropagation()
			Dim temp As String = "select * from users"
			MyBase.RequiresSqlFragment(temp)
		End Sub

		Public Sub InvalidSafenessPropagationParameter(<Fragment("SqlFragment")> temp As String)
			temp = MyBase.UnsafeSource(temp)
			MyBase.RequiresSqlFragment(temp)
		End Sub

		Public Sub ValidSafenessPropagationParameter(temp As String)
			MyBase.DummyMethod(temp)
			temp = MyBase.SafeSource()
			MyBase.RequiresSqlFragment(temp)
		End Sub

		Public Sub InvalidSafenessPropagationVariable()
			Dim temp As String = MyBase.SafeSource()
			temp = MyBase.UnsafeSource(temp)
			MyBase.RequiresSqlFragment(temp)
		End Sub

		Public Sub ValidSafenessPropagationVariable()
			Dim temp As String = MyBase.UnsafeSource()
			temp = MyBase.SafeSource()
			MyBase.RequiresSqlFragment(temp)
		End Sub

		Public Function ValidReturnWithIf(<Fragment("DummyFragment")> parameter As String) As<Fragment("DummyFragment")>
		String
			Dim returnValue As String
			If MyBase.SafeSource() = "Dummy" Then
				returnValue = "safe"
			Else
				returnValue = parameter
			End If
			Return returnValue
		End Function

		Public Function InvalidReturnWithIf(<Fragment("DummyFragment")> parameter As String) As<Fragment("DummyFragment")>
		String
			Dim returnValue As String
			If MyBase.SafeSource() = "Dummy" Then
				returnValue = "safe"
			Else
				returnValue = MyBase.UnsafeSource()
			End If
			Return returnValue
		End Function

		Public Function InvalidReturnWithIfFragmentTypeConsidered(<Fragment("DummyFragment")> parameter As String) As<Fragment("DummyFragment")>
		String
			Dim returnValue As String = parameter
			If MyBase.SafeSource() = "Dummy" Then
				MyBase.DummyMethod("dummy")
			Else
				returnValue = MyBase.UnsafeSource()
			End If
			Return returnValue
		End Function

		Public Function InvalidReturnWithTempVariable(<Fragment("DummyFragment")> parameter As String) As<Fragment("DummyFragment")>
		String
			Dim returnValue As String = parameter
			If MyBase.SafeSource() = "Dummy" Then
				MyBase.DummyMethod("dummy")
			Else
				Dim temp As String = MyBase.UnsafeSource()
				MyBase.DummyMethod(temp)
				returnValue = temp
			End If
			Return returnValue
		End Function

		Public Function InvalidReturnWithParameterReset(<Fragment("DummyFragment")> parameter As String) As<Fragment("DummyFragment")>
		String
			Dim returnValue As String = parameter
			If MyBase.SafeSource() = "Dummy" Then
				MyBase.DummyMethod("dummy")
			Else
				parameter = MyBase.UnsafeSource()
				returnValue = parameter
			End If
			Return returnValue
		End Function

		Private Function SafeDummyFragmentSource() As<Fragment("DummyFragment")>
		String
			Return"safe"
		End Function

		Public Function ValidReturnWithParameterReset(<Fragment("DummyFragment")> parameter As String) As<Fragment("DummyFragment")>
		String
			Dim returnValue As String = parameter
			If MyBase.SafeSource() = "Dummy" Then
				MyBase.DummyMethod("dummy")
			Else
				parameter = Me.SafeDummyFragmentSource()
				returnValue = parameter
			End If
			Return returnValue
		End Function

		Public Function InvalidReturnWithFieldReset(<Fragment("DummyFragment")> parameter As String) As<Fragment("DummyFragment")>
		String
			Dim returnValue As String = parameter
			If MyBase.SafeSource() = "Dummy" Then
				MyBase.DummyMethod("dummy")
			Else
				Me._dummyFragmentField = MyBase.UnsafeSource()
				returnValue = Me._dummyFragmentField
			End If
			Return returnValue
		End Function

		Public Function ValidReturnWithFieldReset(<Fragment("DummyFragment")> parameter As String) As<Fragment("DummyFragment")>
		String
			Dim returnValue As String = parameter
			If MyBase.SafeSource() = "Dummy" Then
				MyBase.DummyMethod("dummy")
			Else
				Me._dummyFragmentField = Me.SafeDummyFragmentSource()
				returnValue = Me._dummyFragmentField
			End If
			Return returnValue
		End Function

		Public Function InvalidReturnWithField(<Fragment("DummyFragment")> parameter As String) As<Fragment("DummyFragment")>
		String
			Me._dummyFragmentField = parameter
			If MyBase.SafeSource() = "Dummy" Then
				MyBase.DummyMethod("dummy")
			Else
				Dim temp As String = MyBase.UnsafeSource()
				MyBase.DummyMethod(temp)
				Me._dummyFragmentField = temp
			End If
			Return Me._dummyFragmentField
		End Function

		Public Function ValidReturnWithField(<Fragment("DummyFragment")> parameter As String) As<Fragment("DummyFragment")>
		String
			Me._dummyFragmentField = parameter
			If MyBase.SafeSource() = "Dummy" Then
				MyBase.DummyMethod("dummy")
			Else
				Dim temp As String = Me.SafeDummyFragmentSource()
				MyBase.DummyMethod(temp)
				Me._dummyFragmentField = temp
			End If
			Return Me._dummyFragmentField
		End Function

		Public Function InvalidReturnWithFieldAndLoops(<Fragment("DummyFragment")> parameter As String) As<Fragment("DummyFragment")>
		String
			Me._dummyFragmentField = parameter
			If MyBase.SafeSource() = "Dummy" Then
				MyBase.DummyMethod("dummy")
			Else
				Dim temp As String = Me.SafeDummyFragmentSource()
				MyBase.DummyMethod(temp)
        For i As Integer = 0 To 10 - 1
          While i < 5
            MyBase.DummyMethod(temp)
            Me._dummyFragmentField = temp
            temp = Me.SafeDummyFragmentSource()
            i += 1
          End While
          temp = MyBase.UnsafeSource()
        Next
			End If
			Return Me._dummyFragmentField
		End Function

		Public Function ValidReturnWithFieldAndLoops(<Fragment("DummyFragment")> parameter As String) As<Fragment("DummyFragment")>
		String
			Me._dummyFragmentField = parameter
			If MyBase.SafeSource() = "Dummy" Then
				MyBase.DummyMethod("dummy")
			Else
				Dim temp As String = Me.SafeDummyFragmentSource()
				MyBase.DummyMethod(temp)
        For i As Integer = 0 To 10 - 1
          While i < 5
            MyBase.DummyMethod(temp)
            Me._dummyFragmentField = temp
            temp = Me.SafeDummyFragmentSource()
            i += 1
          End While
          temp = "safe"
        Next
			End If
			Return Me._dummyFragmentField
		End Function
	End Class
End Namespace
