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

Namespace Parser.TypeParsing.TypeParserTests.[Goto]
	Friend Class GotoSample
		Inherits ParserSampleBase

		Public Sub SimpleGoto()
			Dim x As String = "safe"
			MyBase.DummyMethod(x)
			If Not("dummy" = MyBase.SafeSource()) Then
				x = MyBase.SafeSource()
			End If
			MyBase.RequiresSqlFragment(x)
		End Sub

		Public Sub GotoJumpsOverUnsafeAssignment()
			Dim x As String = "safe"
			MyBase.DummyMethod(x)
			If Not("dummy" = MyBase.SafeSource()) Then
				x = MyBase.UnsafeSource()
			End If
			MyBase.RequiresSqlFragment(x)
		End Sub

		Public Sub InvalidCallInsideWhileWithGoto()
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

		Public Sub InvalidCallInsideIfWithGoto()
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

		Public Sub InvalidCallInsideIfWithGotoAndBreak()
			Dim i As Integer = 10
			Dim x As String = "safe"
			While i > 0
				If i = 3 Then
					x = MyBase.SafeSource()
					Exit While
				End If
				x = MyBase.UnsafeSource()
			End While
			MyBase.RequiresSqlFragment(x)
		End Sub
	End Class
End Namespace
