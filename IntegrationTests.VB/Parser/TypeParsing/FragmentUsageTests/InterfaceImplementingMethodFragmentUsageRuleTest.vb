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
Imports InjectionCop.Parser.TypeParsing
Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.FragmentUsageTests
  <TestFixture()>
  Public Class InterfaceImplementingMethodFragmentUsageRuleTest
    Protected c_InjectionCopRuleId As String = "IC0003"

    <Test()>
    Public Sub Check_FindsProblem()
      Dim rule As InterfaceImplementingMethodFragmentUsageRule = New InterfaceImplementingMethodFragmentUsageRule()
      Dim method As Method = Me.GetMethodFromSampleClass(Of ImplementingClassWithInvalidFragmentUsage)()
      Dim result As ProblemCollection = rule.Check(method)
      Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
    End Sub

    <Test()>
    Public Sub Check_NoProblem()
      Dim rule As InterfaceImplementingMethodFragmentUsageRule = New InterfaceImplementingMethodFragmentUsageRule()
      Dim method As Method = Me.GetMethodFromSampleClass(Of ImplementingClassWithoutFragmentUsage)()
      Dim result As ProblemCollection = rule.Check(method)
      Assert.That(result, [Is].Empty)
    End Sub

    <Test()>
    Public Sub Check_FormatsMessage()
      Dim rule As InterfaceImplementingMethodFragmentUsageRule = New InterfaceImplementingMethodFragmentUsageRule()
      Dim method As Method = Me.GetMethodFromSampleClass(Of ImplementingClassWithInvalidFragmentUsage)()
      Dim result As ProblemCollection = rule.Check(method)
      Dim problem As Problem = result(0)
      Assert.That(problem.Resolution.ToString(), [Is].EqualTo("Expected fragment of type 'ValidFragmentType' from implemented interface method, but got 'InvalidFragmentType'."))
    End Sub

    Private Function GetMethodFromSampleClass(Of T)() As Method
      Return IntrospectionUtility.MethodFactory(Of T)("Foo", New TypeNode() {IntrospectionUtility.TypeNodeFactory(Of Integer)(), IntrospectionUtility.TypeNodeFactory(Of String)()})
    End Function
  End Class
End Namespace
