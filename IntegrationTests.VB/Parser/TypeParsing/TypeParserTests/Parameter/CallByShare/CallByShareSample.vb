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

Namespace Parser.TypeParsing.TypeParserTests.Parameter.CallByShare
	Friend Class CallByShareSample
		Inherits ParserSampleBase

		Public Sub UnsafeMethodParameter(unsafeParam As String)
			MyBase.RequiresSqlFragment(unsafeParam)
		End Sub

		Public Sub SafeMethodParameter(<Fragment("SqlFragment")> safeParam As String)
			MyBase.RequiresSqlFragment(safeParam)
		End Sub
	End Class
End Namespace
