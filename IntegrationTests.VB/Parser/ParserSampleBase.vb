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

Namespace Parser
	Public Class ParserSampleBase
		<SqlFragment()>
		Protected _fragmentField As String = "safe"

		Protected Function UnsafeSource() As String
      Return "unsafe command"
		End Function

		Protected Function UnsafeSource(param As String) As String
			Return param + "unsafe"
		End Function

		Protected Function SafeSource() As<Fragment("SqlFragment")>
		String
			Return"safe command"
		End Function

		Protected Function SafeSourceRequiresSqlFragment(<SqlFragment()> param As String) As<Fragment("SqlFragment")>
		String
			Return Me._fragmentField
		End Function

		Protected Sub DummyMethod(parameter As String)
		End Sub

		Protected Function RequiresSqlFragment(<Fragment("SqlFragment")> param As String) As String
			Return param
		End Function

		Protected Function RequiresSqlFragment(<Fragment("SqlFragment")> a As String, b As String, <Fragment("SqlFragment")> c As String) As String
			Return a + b + c
		End Function

		Protected Function RequiresSqlFragmentReturnsBool(<SqlFragment()> param As String) As Boolean
			Return True
		End Function

		Public Function ReturnsHtmlFragment() As<Fragment("HtmlFragment")>
		String
			Return"HtmlFragment"
		End Function
	End Class
End Namespace
