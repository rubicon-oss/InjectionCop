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
