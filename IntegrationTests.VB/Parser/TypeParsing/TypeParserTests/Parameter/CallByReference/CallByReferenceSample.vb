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

Namespace Parser.TypeParsing.TypeParserTests.Parameter.CallByReference
	Friend Class CallByReferenceSample
		Inherits ParserSampleBase

		Public Sub FragmentRefParameterSafeCall()
			Dim safe As String = "safe"
			Me.FragmentRefParameter(safe, 0)
		End Sub

		Public Sub FragmentRefParameterUnsafeCall()
			Dim unSafe As String = MyBase.UnsafeSource()
			Me.FragmentRefParameter(unSafe, 0)
		End Sub

		Public Sub RefParameterSafeOperand()
			Dim safe As String = MyBase.SafeSource()
			Me.FragmentRefParameter(safe, 0)
			MyBase.RequiresSqlFragment(safe)
		End Sub

		Public Sub RefParameterUnsafeOperand()
			Dim unSafe As String = MyBase.UnsafeSource()
			Me.NonFragmentRefParameter(unSafe, 0)
			MyBase.RequiresSqlFragment(unSafe)
		End Sub

		Public Sub RefParameterSafeVariableTurningUnsafe()
			Dim turnUnsafe As String = MyBase.SafeSource()
			Me.NonFragmentRefParameter(turnUnsafe, 0)
			MyBase.RequiresSqlFragment(turnUnsafe)
		End Sub

		Public Sub FragmentRefParameterSafeReturn(<Fragment("SqlFragment")> ByRef fragmentParameter As String)
			MyBase.DummyMethod(fragmentParameter)
			fragmentParameter = "safe"
		End Sub

		Public Sub FragmentRefParameterUnsafeReturn(<Fragment("SqlFragment")> ByRef fragmentParameter As String)
			MyBase.DummyMethod(fragmentParameter)
			fragmentParameter = MyBase.UnsafeSource()
		End Sub

		Public Sub FragmentRefParameterSafeReturnWithAssignment(<Fragment("SqlFragment")> ByRef fragmentRefParameter As String)
			MyBase.DummyMethod(fragmentRefParameter)
			fragmentRefParameter = MyBase.UnsafeSource()
			MyBase.DummyMethod(fragmentRefParameter)
			Dim temp As String = MyBase.SafeSource()
			fragmentRefParameter = temp
		End Sub

		Public Sub FragmentRefParameterUnsafeReturnWithAssignment(<Fragment("SqlFragment")> ByRef fragmentRefParameter As String)
			MyBase.DummyMethod(fragmentRefParameter)
			fragmentRefParameter = MyBase.SafeSource()
			MyBase.DummyMethod(fragmentRefParameter)
			Dim temp As String = MyBase.UnsafeSource()
			fragmentRefParameter = temp
		End Sub

		Public Sub SafeFragmentRefParameterInsideCondition(<Fragment("SqlFragment")> ByRef fragmentRefParameter As String)
			MyBase.DummyMethod(fragmentRefParameter)
			fragmentRefParameter = "safe"
			Dim temp As String = MyBase.SafeSource()
			If MyBase.SafeSource() = "dummy" Then
				fragmentRefParameter = temp
			End If
		End Sub

		Public Sub UnsafeFragmentRefParameterInsideCondition(<Fragment("SqlFragment")> ByRef fragmentRefParameter As String)
			MyBase.DummyMethod(fragmentRefParameter)
			fragmentRefParameter = "safe"
			Dim temp As String = MyBase.UnsafeSource()
			If MyBase.SafeSource() = "dummy" Then
				fragmentRefParameter = temp
			End If
		End Sub

		Public Sub SafeFragmentRefParameterInsideWhile(<Fragment("SqlFragment")> ByRef fragmentRefParameter As String)
			MyBase.DummyMethod(fragmentRefParameter)
			fragmentRefParameter = "safe"
			Dim temp As String = MyBase.SafeSource()
      For i As Integer = 0 To 5 - 1
        fragmentRefParameter = temp
        temp = MyBase.SafeSource()
      Next
		End Sub

		Public Sub UnsafeFragmentRefParameterInsideWhile(<Fragment("SqlFragment")> ByRef fragmentRefParameter As String)
			MyBase.DummyMethod(fragmentRefParameter)
			fragmentRefParameter = "safe"
			Dim temp As String = MyBase.SafeSource()
      For i As Integer = 0 To 5 - 1
        fragmentRefParameter = temp
        temp = MyBase.UnsafeSource()
      Next
		End Sub

		Public Function UnsafeReturnWithAssignment(<Fragment("SqlFragment")> ByRef unSafe As String) As<SqlFragment()>
		String
			MyBase.DummyMethod(unSafe)
			unSafe = MyBase.UnsafeSource()
			Dim temp As String = unSafe
			unSafe = MyBase.SafeSource()
			Return temp
		End Function

		Private Sub FragmentRefParameter(<Fragment("SqlFragment")> ByRef safe As String, dummy As Integer)
			safe = "safe" + safe + dummy
		End Sub

		Private Sub NonFragmentRefParameter(ByRef unSafe As String, dummy As Integer)
			unSafe += dummy
		End Sub
	End Class
End Namespace
