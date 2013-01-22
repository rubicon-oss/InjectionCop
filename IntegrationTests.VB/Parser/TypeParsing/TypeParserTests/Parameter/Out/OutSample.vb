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

Namespace Parser.TypeParsing.TypeParserTests.Parameter.Out
	Friend Class OutSample
		Inherits ParserSampleBase

		Public Sub FragmentOutParameterSafeCall()
			Dim staySafe As String = MyBase.SafeSource()
			Me.FragmentOutParameterSafeReturn(staySafe)
		End Sub

		Public Sub FragmentOutParameterUnsafeCall()
			Dim unsafeVariable As String = MyBase.UnsafeSource()
			Me.FragmentOutParameterSafeReturn(unsafeVariable)
		End Sub

		Public Sub OutParameterSafeOperand()
			Dim operand As String = MyBase.SafeSource()
			Me.FragmentOutParameterSafeReturn(operand)
			MyBase.RequiresSqlFragment(operand)
		End Sub

		Public Sub OutParameterUnsafeOperand()
			Dim unSafe As String = MyBase.UnsafeSource()
			Me.NonFragmentOutParameter(unSafe)
			MyBase.RequiresSqlFragment(unSafe)
		End Sub

		Public Sub OutParameterSafeVariableTurningUnsafe()
			Dim turnsUnsafe As String = MyBase.SafeSource()
			Me.NonFragmentOutParameter(turnsUnsafe)
			MyBase.RequiresSqlFragment(turnsUnsafe)
		End Sub

    Public Sub FragmentOutParameterSafeReturn(<Fragment("SqlFragment")> ByRef safe As String)
      safe = "safe"
    End Sub

    Public Sub FragmentOutParameterUnsafeReturn(<Fragment("SqlFragment")> ByRef unSafe As String)
      unSafe = MyBase.UnsafeSource()
    End Sub

    Public Sub FragmentOutParameterSafeReturnWithAssignment(<Fragment("SqlFragment")> ByRef framentOutParameter As String)
      framentOutParameter = MyBase.UnsafeSource()
      MyBase.DummyMethod(framentOutParameter)
      Dim temp As String = MyBase.SafeSource()
      framentOutParameter = temp
    End Sub

    Public Sub FragmentOutParameterUnsafeReturnWithAssignment(<Fragment("SqlFragment")> ByRef fragmentOutParameter As String)
      fragmentOutParameter = MyBase.SafeSource()
      MyBase.DummyMethod(fragmentOutParameter)
      Dim temp As String = MyBase.UnsafeSource()
      fragmentOutParameter = temp
    End Sub

    Public Sub SafeFragmentOutParameterInsideCondition(<Fragment("SqlFragment")> ByRef fragmentOutParameter As String)
      fragmentOutParameter = "safe"
      Dim temp As String = MyBase.SafeSource()
      If MyBase.SafeSource() = "dummy" Then
        fragmentOutParameter = temp
      End If
    End Sub

    Public Sub UnsafeFragmentOutParameterInsideCondition(<Fragment("SqlFragment")> ByRef fragmentOutParameter As String)
      fragmentOutParameter = "safe"
      Dim temp As String = MyBase.UnsafeSource()
      If MyBase.SafeSource() = "dummy" Then
        fragmentOutParameter = temp
      End If
    End Sub

    Public Sub SafeFragmentOutParameterInsideWhile(<Fragment("SqlFragment")> ByRef fragmentOutParameter As String)
      fragmentOutParameter = "safe"
      Dim temp As String = MyBase.SafeSource()
      For i As Integer = 0 To 5 - 1
        fragmentOutParameter = temp
        temp = MyBase.SafeSource()
      Next
    End Sub

    Public Sub UnsafeFragmentOutParameterInsideWhile(<Fragment("SqlFragment")> ByRef fragmentOutParameter As String)
      fragmentOutParameter = "safe"
      Dim temp As String = MyBase.SafeSource()
      For i As Integer = 0 To 5 - 1
        fragmentOutParameter = temp
        temp = MyBase.UnsafeSource()
      Next
    End Sub

    Public Function UnsafeReturnWithAssignment(<Fragment("SqlFragment")> ByRef unSafe As String) As <SqlFragment()>
    String
      unSafe = MyBase.UnsafeSource()
      Dim temp As String = unSafe
      unSafe = MyBase.SafeSource()
      Return temp
    End Function

    Private Sub NonFragmentOutParameter(ByRef unSafe As String)
      unSafe = "unsafe"
    End Sub
	End Class
End Namespace
