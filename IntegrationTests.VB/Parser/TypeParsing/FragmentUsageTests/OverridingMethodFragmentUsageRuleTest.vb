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
  Public Class OverridingMethodFragmentUsageRuleTest
    Protected c_InjectionCopRuleId As String = "IC0002"

    <Test()>
    Public Sub Check_FindsProblem()
      Dim rule As OverridingMethodFragmentUsageRule = New OverridingMethodFragmentUsageRule()
      Dim method As Method = Me.GetMethodFromSampleClass(Of DerivedClassWithInvalidFragmentUsage)()
      Dim result As ProblemCollection = rule.Check(method)
      Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
    End Sub

    <Test()>
    Public Sub Check_MatchesParametersCorrectly()
      Dim rule As OverridingMethodFragmentUsageRule = New OverridingMethodFragmentUsageRule()
      Dim method As Method = IntrospectionUtility.MethodFactory(Of DerivedClassWithInvalidFragmentUsage)("MethodWithFragmentOnSecondParameter", New TypeNode() {IntrospectionUtility.TypeNodeFactory(Of Integer)(), IntrospectionUtility.TypeNodeFactory(Of Integer)()})
      Dim method2 As Method = method
      Dim result As ProblemCollection = rule.Check(method2)
      Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
    End Sub

    <Test()>
    Public Sub Check_FormatsMessage()
      Dim rule As OverridingMethodFragmentUsageRule = New OverridingMethodFragmentUsageRule()
      Dim method As Method = Me.GetMethodFromSampleClass(Of DerivedClassWithInvalidFragmentUsage)()
      Dim result As ProblemCollection = rule.Check(method)
      Dim problem As Problem = result(0)
      Assert.That(problem.Resolution.ToString(), [Is].EqualTo("Expected fragment of type 'ValidFragmentType' from overriden method, but got 'InvalidFragmentType'."))
    End Sub

    <Test()>
    Public Sub Check_DerivedClassWithoutFragmentUsage()
      Dim rule As OverridingMethodFragmentUsageRule = New OverridingMethodFragmentUsageRule()
      Dim method As Method = Me.GetMethodFromSampleClass(Of DerivedClassWithoutFragmentUsage)()
      Dim result As ProblemCollection = rule.Check(method)
      Assert.That(result, [Is].Empty)
    End Sub

    <Test()>
    Public Sub Check_DerivedClassWithDuplicatedFragmentUsage()
      Dim rule As OverridingMethodFragmentUsageRule = New OverridingMethodFragmentUsageRule()
      Dim method As Method = Me.GetMethodFromSampleClass(Of DerivedClassWithValidFragmentUsage)()
      Dim result As ProblemCollection = rule.Check(method)
      Assert.That(result, [Is].Empty)
    End Sub

    Private Function GetMethodFromSampleClass(Of T)() As Method
      Return IntrospectionUtility.MethodFactory(Of T)("Foo", New TypeNode() {IntrospectionUtility.TypeNodeFactory(Of Integer)(), IntrospectionUtility.TypeNodeFactory(Of String)()})
    End Function
  End Class
End Namespace
