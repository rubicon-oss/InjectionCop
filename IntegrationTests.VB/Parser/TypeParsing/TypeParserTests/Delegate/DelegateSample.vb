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

Namespace Parser.TypeParsing.TypeParserTests.[Delegate]
	Public Class DelegateSample
		Inherits ParserSampleBase

		Public Delegate Function FragmentParameterDelegate(<Fragment("DelegateFragmentType")> fragmentParameter As String, nonFragmentParameter As String) As String

		Public Delegate Function ReturnFragmentDelegate() As<Fragment("DelegateFragmentType")>
		String

		Public Delegate Function FragmentParameterAndReturnDelegate(<Fragment("DelegateFragmentType")> fragmentParameter As String, nonFragmentParameter As String) As<Fragment("DelegateFragmentType")>
		String

		Private _fragmentParameterDelegate As DelegateSample.FragmentParameterDelegate

		Public Sub New()
      Me._fragmentParameterDelegate = New DelegateSample.FragmentParameterDelegate(AddressOf MatchingFragmentParameterDelegate)
		End Sub

		Private Function MatchingFragmentParameterDelegate(fragmentParameter As String, nonFragmentParameter As String) As String
			MyBase.DummyMethod(nonFragmentParameter)
			Return fragmentParameter
		End Function

		Private Function MatchingFragmentParameterAndReturnDelegateSafeReturn(fragmentParameter As String, nonFragmentParameter As String) As String
			MyBase.DummyMethod(nonFragmentParameter)
			Return fragmentParameter
		End Function

		Private Function MatchingFragmentParameterAndReturnDelegateUnsafeReturn(fragmentParameter As String, nonFragmentParameter As String) As String
			MyBase.DummyMethod(fragmentParameter)
			Return nonFragmentParameter
		End Function

		Private Function SafeReturn() As String
			Return"safe"
		End Function

		Public Function UnsafeReturn() As String
			Return MyBase.UnsafeSource()
		End Function

		Public Sub SafeDelegateCall()
      Dim fragmentDelegate As DelegateSample.FragmentParameterDelegate = New DelegateSample.FragmentParameterDelegate(AddressOf MatchingFragmentParameterDelegate)
			fragmentDelegate("safe", "safe")
		End Sub

		Public Sub UnsafeDelegateCall()
      Dim fragmentDelegate As DelegateSample.FragmentParameterDelegate = New DelegateSample.FragmentParameterDelegate(AddressOf MatchingFragmentParameterDelegate)
			fragmentDelegate(MyBase.UnsafeSource(), "safe")
		End Sub

		Public Sub SafeDelegateCallUsingReturn()
      Dim fragmentParameterDelegate As DelegateSample.FragmentParameterDelegate = New DelegateSample.FragmentParameterDelegate(AddressOf MatchingFragmentParameterDelegate)
      Dim returnFragmentDelegate As DelegateSample.ReturnFragmentDelegate = New DelegateSample.ReturnFragmentDelegate(AddressOf SafeReturn)
			fragmentParameterDelegate(returnFragmentDelegate(), "safe")
		End Sub

		Public Sub SafeDelegateFieldCall()
			Me._fragmentParameterDelegate("safe", "safe")
		End Sub

		Public Sub UnsafeDelegateFieldCall()
			Me._fragmentParameterDelegate(MyBase.UnsafeSource(), "safe")
		End Sub

		Public Sub DelegateWithSafeReturn()
      Dim sampleDelegate As DelegateSample.FragmentParameterAndReturnDelegate = New DelegateSample.FragmentParameterAndReturnDelegate(AddressOf MatchingFragmentParameterAndReturnDelegateSafeReturn)
			sampleDelegate("safe", "safe")
		End Sub

		Public Sub DelegateWithUnsafeReturn()
      Dim sampleDelegate As DelegateSample.FragmentParameterAndReturnDelegate = New DelegateSample.FragmentParameterAndReturnDelegate(AddressOf MatchingFragmentParameterAndReturnDelegateUnsafeReturn)
			sampleDelegate("safe", "safe")
		End Sub

		Public Sub Foo()
			Dim f As DelegateSample.FragmentParameterDelegate = Me.ReturnsDelegate()
			f("", "")
		End Sub

		Private Function ReturnsDelegate() As DelegateSample.FragmentParameterDelegate
      Return New DelegateSample.FragmentParameterDelegate(AddressOf MatchingFragmentParameterAndReturnDelegateSafeReturn)
		End Function
	End Class
End Namespace
