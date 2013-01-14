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

Namespace Parser.TypeParsing.TypeParserTests.AttributePropagation
	Friend Class AttributePropagationSample
		Inherits ParserSampleBase

		Public Sub SafeCallOfSqlFragmentCallee()
			MyBase.RequiresSqlFragment(MyBase.SafeSource())
		End Sub

		Public Function UnsafeCallOfSqlFragmentCallee() As String
			Return MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
		End Function

		Public Function SafeCallOfMixedCallee() As String
			Return MyBase.RequiresSqlFragment("literal", MyBase.UnsafeSource(), MyBase.SafeSource())
		End Function

		Public Sub UnsafeCallOfMixedCallee()
			MyBase.RequiresSqlFragment("literal", MyBase.SafeSource(), MyBase.UnsafeSource())
		End Sub
	End Class
End Namespace
