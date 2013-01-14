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

Namespace Parser.TypeParsing.TypeParserTests.[If]
	Friend Class IfSample
		Inherits ParserSampleBase

		Public Sub ValidExampleInsideIf(x As Integer, y As Integer)
			If x = y Then
				MyBase.RequiresSqlFragment("safe")
			End If
		End Sub

		Public Sub InvalidExampleInsideIf(x As Integer, y As Integer)
			If x = y Then
				MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
			End If
		End Sub

		Public Sub InvalidExampleInsideElse(x As Integer, y As Integer)
			If x = y Then
				Dim temp As String = ""
				MyBase.RequiresSqlFragment(temp)
			Else
				MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
			End If
		End Sub

		Public Sub UnsafeAssignmentInsideIf(x As Integer, y As Integer)
			Dim temp As String
			If x = y Then
				temp = MyBase.UnsafeSource()
			Else
				temp = "safe"
			End If
			MyBase.RequiresSqlFragment(temp)
		End Sub

		Public Sub UnsafeAssignmentInsideIfTwisted(x As Integer, y As Integer)
			Dim temp As String
			If x = y Then
				temp = "safe"
			Else
				temp = MyBase.UnsafeSource()
			End If
			MyBase.RequiresSqlFragment(temp)
		End Sub

		Public Sub UnsafeAssignmentInsideIfNested(x As Integer, y As Integer, z As Integer)
			Dim temp As String
			If x = z Then
				temp = "safe"
				If x = y Then
					temp = MyBase.UnsafeSource()
				End If
			Else
				temp = MyBase.SafeSource()
			End If
			MyBase.RequiresSqlFragment(temp)
		End Sub

		Public Sub SafeAssignmentInsideIfNested(x As Integer, y As Integer, z As Integer)
			Dim temp As String
			If x = z Then
				temp = "safe"
				If x = y Then
					temp = MyBase.SafeSource()
				End If
			Else
				temp = MyBase.SafeSource()
			End If
			MyBase.RequiresSqlFragment(temp)
		End Sub

		Public Sub UnsafeAssignmentInsideIfNestedDeeper(x As Integer, y As Integer, z As Integer)
			Dim temp As String = "safe"
			If x = z Then
				temp = "safe"
			Else
				If x = y Then
					If y = z Then
						temp = MyBase.SafeSource()
					Else
						temp = MyBase.UnsafeSource()
					End If
				End If
			End If
			MyBase.RequiresSqlFragment(temp)
		End Sub

		Public Sub UnsafeAssignmentInsideIfNestedElse(x As Integer, y As Integer, z As Integer)
			Dim temp As String
			If x = z Then
				temp = "safe"
			Else
				If z = y Then
					temp = MyBase.UnsafeSource()
				Else
					temp = MyBase.SafeSource()
				End If
			End If
			MyBase.RequiresSqlFragment(temp)
		End Sub

		Public Sub UnsafeAssignmentInsideIfReversed(x As Integer, y As Integer, z As Integer)
			Dim temp As String
			If x = z Then
				If x = y Then
					temp = MyBase.UnsafeSource()
				End If
				temp = "safe again"
			Else
				If z = y Then
					temp = MyBase.UnsafeSource()
				Else
					temp = MyBase.SafeSource()
				End If
			End If
			MyBase.RequiresSqlFragment(temp)
		End Sub

		Public Sub InvalidCallInsideIfCondition()
			If MyBase.RequiresSqlFragmentReturnsBool(MyBase.UnsafeSource()) Then
				MyBase.RequiresSqlFragment("Safe")
			End If
		End Sub
	End Class
End Namespace
