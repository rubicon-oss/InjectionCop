Imports System
Imports System.Data
Imports System.Data.SqlClient

Namespace Parser.TypeParsing.TypeParserTests.BlackMethod
	Friend Class BlackMethodSample
		Inherits ParserSampleBase

		Public Sub BlackMethodCallLiteral()
			Dim command As IDbCommand = New SqlCommand()
			command.CommandText = "select * from users"
		End Sub

		Public Sub BlackMethodCallSafeSource()
			Dim command As IDbCommand = New SqlCommand()
			command.CommandText = MyBase.SafeSource()
		End Sub

		Public Sub BlackMethodCallUnsafeSourceNoParameter()
			Dim command As IDbCommand = New SqlCommand()
			command.CommandText = MyBase.UnsafeSource()
		End Sub

		Public Sub BlackMethodCallUnsafeSourceWithSafeParameter()
			Dim command As IDbCommand = New SqlCommand()
			command.CommandText = MyBase.UnsafeSource("")
		End Sub

		Public Sub WhiteMethodCall()
      CType(New SqlCommand() With {.CommandTimeout = 0}, System.IDisposable).Dispose()
      Dim sqlCommand As New SqlCommand() With {.CommandTimeout = 0}
      sqlCommand.Dispose()
			Dim str As String = "ab"
			If str = "ab" Then
			End If
		End Sub
	End Class
End Namespace
