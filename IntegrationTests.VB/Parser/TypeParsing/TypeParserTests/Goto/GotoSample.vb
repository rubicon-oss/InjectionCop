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
