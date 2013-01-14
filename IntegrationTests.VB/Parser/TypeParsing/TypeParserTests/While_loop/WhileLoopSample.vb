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

Namespace Parser.TypeParsing.TypeParserTests.While_loop
	Public Class WhileLoopSample
		Inherits ParserSampleBase

		Public Sub ValidCallInsideWhile()
      For i As Integer = 10 To 0 + 1 Step -1
        MyBase.RequiresSqlFragment("safe")
      Next
		End Sub

		Public Sub InValidCallInsideWhile()
      For i As Integer = 10 To 0 + 1 Step -1
        MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
      Next
		End Sub

		Public Sub InValidCallInsideWhileReprocessingRequired()
			Dim i As Integer = 10
			Dim parameter As String = MyBase.SafeSource()
			While i > 0
				MyBase.RequiresSqlFragment(parameter)
				parameter = MyBase.UnsafeSource()
				i -= 1
			End While
		End Sub

		Public Sub InValidAssignmentInsideWhileReprocessingRequired()
			Dim i As Integer = 10
			Dim parameter As String = MyBase.SafeSource()
			While i > 0
				Me._fragmentField = parameter
				parameter = MyBase.UnsafeSource()
				i -= 1
			End While
		End Sub

		Public Sub InvalidCallInsideNestedWhile()
      For i As Integer = 10 To 0 + 1 Step -1
        While i > 5
          MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
          i -= 1
        End While
      Next
		End Sub

		Public Sub InValidCallInsideNestedWhileReprocessingRequired()
			Dim i As Integer = 10
			Dim parameter As String = MyBase.SafeSource()
			While i > 0
				While i > 5
					MyBase.RequiresSqlFragment(parameter)
					i -= 1
				End While
				parameter = MyBase.UnsafeSource()
				i -= 1
			End While
		End Sub

		Public Sub InValidCallInsideDeeperNestedWhileReprocessingRequired()
			Dim i As Integer = 10
			Dim parameter As String = MyBase.SafeSource()
			While i > 0
				While i > 5
					While i > 7
						MyBase.RequiresSqlFragment(parameter)
						i -= 1
					End While
					i -= 1
				End While
				parameter = MyBase.UnsafeSource()
				i -= 1
			End While
		End Sub

		Public Sub ValidCallInsideWhileWithContinue()
			Dim i As Integer = 10
			Dim x As String = "safe"
			While i > 0
				MyBase.RequiresSqlFragment(x)
				If i <> 3 Then
					i -= 1
				Else
					i -= 1
				End If
			End While
		End Sub

		Public Sub InvalidCallInsideWhileWithContinue()
			Dim i As Integer = 10
			Dim x As String = "safe"
			While i > 0
				MyBase.RequiresSqlFragment(x)
				If i <> 3 Then
					i -= 1
				Else
					x = MyBase.UnsafeSource()
					i -= 1
				End If
			End While
		End Sub

		Public Sub InvalidCallInsideIfWithContinue()
			Dim i As Integer = 10
			Dim x As String = "safe"
			While i > 0
				MyBase.RequiresSqlFragment(x)
				If i <> 3 Then
					x = MyBase.UnsafeSource()
					i -= 1
				Else
					x = MyBase.SafeSource()
					i -= 1
				End If
			End While
		End Sub

		Public Sub ValidCallInsideWhileWithBreak()
			Dim i As Integer = 10
			Dim x As String = "safe"
			While i > 0
				If i <> 3 Then
					Exit While
				End If
				i -= 1
			End While
			MyBase.RequiresSqlFragment(x)
		End Sub

		Public Sub InvalidCallInsideWhileWithBreak()
			Dim i As Integer = 10
			Dim x As String = "safe"
			While i > 0
				If i <> 3 Then
					Exit While
				End If
				x = MyBase.UnsafeSource()
				i -= 1
			End While
			MyBase.RequiresSqlFragment(x)
		End Sub

		Public Sub InvalidCallInsideIfWithBreak()
			Dim i As Integer = 10
			Dim x As String = "safe"
			While i > 0
				If i <> 3 Then
					x = MyBase.UnsafeSource()
					Exit While
				End If
				x = MyBase.SafeSource()
				i -= 1
			End While
			MyBase.RequiresSqlFragment(x)
		End Sub

		Public Sub InValidCallInsideWhileCondition()
			While MyBase.RequiresSqlFragmentReturnsBool(MyBase.UnsafeSource())
				MyBase.RequiresSqlFragment("safe")
			End While
		End Sub
	End Class
End Namespace
