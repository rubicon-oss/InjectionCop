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
Imports InjectionCop.IntegrationTests.VB.Parser.TypeParsing.TypeParserTests.AnonymousMethod
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Closure
	Public Class ClosureSample
		Inherits ParserSampleBase

		Public Delegate Function FragmentParameterDelegate() As String

		<Fragment("ClosureFragmentType")>
		Private _safeField As String

		Private _unsafeField As String

		Private Sub New()
			Me._safeField = "dummy"
			Me._unsafeField = "dummy"
		End Sub

		Public Sub SafeClosureUsingLocalVariable()
			Dim safeSource As String = "safe"
			Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(param0 As String, param1 As String) Me.RequiresClosureFragment(safeSource)
			fragmentDelegate("safe", "safe")
		End Sub

		Public Sub UnsafeClosureUsingLocalVariable()
			Dim safeSource As String = MyBase.UnsafeSource()
			Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(param0 As String, param1 As String) Me.RequiresClosureFragment(safeSource)
			fragmentDelegate("safe", "safe")
		End Sub

		Public Sub SafeClosureUsingField()
			Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(param0 As String, param1 As String) Me.RequiresClosureFragment(Me._safeField)
			fragmentDelegate("safe", "safe")
		End Sub

		Public Sub UnsafeClosureUsingField()
			Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(param0 As String, param1 As String) Me.RequiresClosureFragment(Me._unsafeField)
			fragmentDelegate("safe", "safe")
		End Sub

		Private Function RequiresClosureFragment(<Fragment("ClosureFragmentType")> fragmentParameter As String) As String
			Return fragmentParameter
		End Function
	End Class
End Namespace
