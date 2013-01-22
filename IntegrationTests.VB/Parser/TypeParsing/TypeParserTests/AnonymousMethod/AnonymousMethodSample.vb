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

Namespace Parser.TypeParsing.TypeParserTests.AnonymousMethod
  Friend Class AnonymousMethodSample
    Inherits ParserSampleBase

    Public Delegate Function FragmentParameterDelegate(<Fragment("AnonymousMethodFragmentType")> fragmentParameter As String, nonFragmentParameter As String) As String

    Public Delegate Function ReturnFragmentDelegate() As <Fragment("AnonymousMethodFragmentType")>
    String

    Public Sub SafeAnonymousMethodCall()
      Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(fragmentParameter As String, nonFragmentParameter As String) fragmentParameter + nonFragmentParameter
      fragmentDelegate("safe", "safe")
    End Sub

    Public Sub UnsafeAnonymousMethodCall()
      Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(fragmentParameter As String, nonFragmentParameter As String) fragmentParameter + nonFragmentParameter
      fragmentDelegate(MyBase.UnsafeSource(), "safe")
    End Sub

    Public Sub SafeAnonymousMethodCallUsingReturn()
      Dim fragmentParameterDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(fragmentParameter As String, nonFragmentParameter As String) fragmentParameter + nonFragmentParameter
      Dim returnFragmentDelegate As AnonymousMethodSample.ReturnFragmentDelegate = Function() "safe"
      fragmentParameterDelegate(returnFragmentDelegate(), "safe")
    End Sub

    Public Sub SafeMethodCallInsideAnonymousMethod()
      Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(fragmentParameter As String, nonFragmentParameter As String)
                                                                                  Me.RequiresAnonymousMethodFragment(fragmentParameter)
                                                                                  Return fragmentParameter + nonFragmentParameter
                                                                                End Function
      fragmentDelegate("safe", "safe")
    End Sub

    Public Sub UnsafeMethodCallInsideAnonymousMethod()
      Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(fragmentParameter As String, nonFragmentParameter As String)
                                                                                  Me.RequiresAnonymousMethodFragment(nonFragmentParameter)
                                                                                  Return fragmentParameter + nonFragmentParameter
                                                                                End Function
      fragmentDelegate("safe", "safe")
    End Sub

    Public Sub SafeReturnInsideAnonymousMethod()
      Dim returnFragmentDelegate As AnonymousMethodSample.ReturnFragmentDelegate = Function() "safe"
      returnFragmentDelegate()
    End Sub

    Public Sub UnsafeReturnInsideAnonymousMethod()
      Dim returnFragmentDelegate As AnonymousMethodSample.ReturnFragmentDelegate = Function() MyBase.UnsafeSource()
      returnFragmentDelegate()
    End Sub

    Private Sub RequiresAnonymousMethodFragment(<Fragment("AnonymousMethodFragmentType")> fragmentParameter As String)
      MyBase.DummyMethod(fragmentParameter)
    End Sub
  End Class
End Namespace
