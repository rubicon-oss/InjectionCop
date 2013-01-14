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

Namespace Parser.TypeParsing.TypeParserTests.Switch
	Friend Class SwitchSample
		Inherits ParserSampleBase

		Public Sub ValidSwitch(i As Integer)
			Select Case i
				Case 1
					MyBase.RequiresSqlFragment("safe")
				Case 2
					MyBase.RequiresSqlFragment(MyBase.SafeSource())
				Case Else
					MyBase.RequiresSqlFragment("safe")
			End Select
		End Sub

		Public Sub UnsafeCallInsideSwitch(i As Integer)
			Select Case i
				Case 1
					MyBase.RequiresSqlFragment("safe")
				Case 2
					MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
				Case Else
					MyBase.RequiresSqlFragment("safe")
			End Select
		End Sub

		Public Sub UnsafeCallAfterSwitch(i As Integer)
			Dim param As String
			Select Case i
				Case 1
					param = "safe"
				Case 2
					param = MyBase.UnsafeSource()
				Case Else
					param = "safe"
			End Select
			MyBase.RequiresSqlFragment(param)
		End Sub

		Public Sub UnsafeCallAfterNestedSwitch(i As Integer, j As Integer)
			Dim param As String
			Select Case i
				Case 1
					param = "safe"
				Case 2
					If j <> 1 Then
						param = MyBase.UnsafeSource()
					Else
						param = "safe"
					End If
				Case Else
					param = "safe"
			End Select
			MyBase.RequiresSqlFragment(param)
		End Sub

		Public Sub SafeCallAfterNestedSwitch(i As Integer, j As Integer)
			Dim param As String
			Select Case i
				Case 1
					param = "safe"
				Case 2
					If j <> 1 Then
						param = MyBase.UnsafeSource()
					Else
						param = "safe"
					End If
					MyBase.DummyMethod(param)
					param = MyBase.SafeSource()
				Case Else
					param = "safe"
			End Select
			MyBase.RequiresSqlFragment(param)
		End Sub

		Public Sub UnsafeCallInsideNestedSwitch(i As Integer, j As Integer)
			Dim param As String
			Select Case i
				Case 1
					param = "safe"
				Case 2
					If j <> 1 Then
						param = MyBase.UnsafeSource()
					Else
						param = "safe"
					End If
					MyBase.RequiresSqlFragment(param)
				Case Else
					param = "safe"
			End Select
			MyBase.DummyMethod(param)
		End Sub

		Public Sub ValidFallThrough(i As Integer)
			Select Case i
				Case 1, 2
					MyBase.RequiresSqlFragment(MyBase.SafeSource())
				Case Else
					MyBase.RequiresSqlFragment("safe")
			End Select
		End Sub

		Public Sub ValidFallThroughGoto(i As Integer)
			Select Case i
				Case 1
					MyBase.RequiresSqlFragment("safe")
				Case 2
				Case Else
					MyBase.RequiresSqlFragment("safe")
					Return
			End Select
			MyBase.RequiresSqlFragment(MyBase.SafeSource())
		End Sub

		Public Sub InvalidFallThrough(i As Integer)
			Select Case i
				Case 1, 2
					MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
				Case Else
					MyBase.RequiresSqlFragment("safe")
			End Select
		End Sub

		Public Sub InvalidFallThroughGoto(i As Integer)
			Select Case i
				Case 1
					MyBase.RequiresSqlFragment("safe")
				Case 2
				Case Else
					MyBase.RequiresSqlFragment("safe")
					Return
			End Select
			MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
		End Sub
	End Class
End Namespace
