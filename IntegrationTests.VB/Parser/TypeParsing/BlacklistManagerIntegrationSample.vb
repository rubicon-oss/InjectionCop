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
Imports System.Data
Imports System.Data.SqlClient

Namespace Parser.TypeParsing
	Public Class BlacklistManagerIntegrationSample
		Inherits ParserSampleBase

		Public Sub UnsafeBlacklistedCall()
			Dim command As IDbCommand = New SqlCommand()
			command.CommandText = MyBase.UnsafeSource("")
		End Sub

		Public Sub SafeBlacklistedCall()
			Dim command As IDbCommand = New SqlCommand()
			command.CommandText = MyBase.SafeSource()
		End Sub

		Public Sub ListedAndUnlistedViolation()
			MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
			Dim command As IDbCommand = New SqlCommand()
			command.CommandText = MyBase.UnsafeSource("")
		End Sub

		Public Sub FragmentDefinedInXmlSafeCall()
			Me.FragmentDefinedInXml(Me.ReturnsExternal())
		End Sub

		Public Sub FragmentDefinedInXmlUnsafeCall()
			Me.FragmentDefinedInXml(Me.UnsafeString())
		End Sub

		Public Sub MixedViolations(x As Integer)
			Me.RequiresDummyFragment(3)
			Me.RequiresDummyFragment(Me.SafeDummySource())
			Me.RequiresDummyFragment(Me.UnsafeDummySource())
			Me.RequiresDummyFragment(Me.OtherUnsafeDummySource())
			Dim command As IDbCommand = New SqlCommand()
			command.CommandText = Me.UnsafeString()
			Me.FragmentDefinedInXml(Me.UnsafeString())
			Me.FragmentDefinedInXml(Me.ReturnsExternal())
			MyBase.RequiresSqlFragment("safe")
      If 1 = x Then
        If 2 = x Then
        End If
      End If
		End Sub

		Public Sub RequiresDummyFragment(<Fragment("dummy")> i As Integer)
		End Sub

		Public Function UnsafeDummySource() As Integer
      Return 3
		End Function

		Public Function OtherUnsafeDummySource() As<Fragment("wrongtype")>
		Integer
      Return 3
		End Function

		Public Function SafeDummySource() As<Fragment("dummy")>
		Integer
      Return 3
		End Function

		Public Function UnsafeString() As String
			Return"unsafe command"
		End Function

		Public Function ReturnsExternal() As<Fragment("external")>
		String
			Return"safe"
		End Function

		Public Sub FragmentDefinedInXml(needsExternalFragment As String)
		End Sub
	End Class
End Namespace
