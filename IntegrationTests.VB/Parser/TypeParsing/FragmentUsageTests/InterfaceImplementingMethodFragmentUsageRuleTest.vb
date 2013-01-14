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
