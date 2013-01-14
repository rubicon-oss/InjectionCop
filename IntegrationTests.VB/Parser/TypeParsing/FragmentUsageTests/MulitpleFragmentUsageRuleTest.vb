Imports InjectionCop.Parser.TypeParsing
Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.FragmentUsageTests
	<TestFixture()>
	Public Class MulitpleFragmentUsageRuleTest
		Protected c_InjectionCopRuleId As String = "IC0004"

		<Test()>
		Public Sub CheckMember_InvalidFragmentUsage()
			Dim sampleMethod As Method = Me.GetSampleMethod("InvalidFragmentUsage")
			Dim rule As MultipleFragmentUsageRule = New MultipleFragmentUsageRule()
			Dim result As ProblemCollection = rule.Check(sampleMethod)
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub CheckMember_FormatsMessage()
			Dim sampleMethod As Method = Me.GetSampleMethod("InvalidFragmentUsage")
			Dim rule As MultipleFragmentUsageRule = New MultipleFragmentUsageRule()
			Dim result As ProblemCollection = rule.Check(sampleMethod)
			Assert.That(result(0).Resolution.ToString(), [Is].EqualTo("Parameter 'parameter' has multiple fragment types assigned."))
		End Sub

		<Test()>
		Public Sub CheckMember_ValidFragmentUsage()
			Dim sampleMethod As Method = Me.GetSampleMethod("ValidFragmentUsage")
			Dim rule As MultipleFragmentUsageRule = New MultipleFragmentUsageRule()
			Dim result As ProblemCollection = rule.Check(sampleMethod)
			Assert.That(result, [Is].Empty)
		End Sub

		<Test()>
		Public Sub CheckMember_NoFragmentUsage()
			Dim sampleMethod As Method = Me.GetSampleMethod("NoFragmentUsage")
			Dim rule As MultipleFragmentUsageRule = New MultipleFragmentUsageRule()
			Dim result As ProblemCollection = rule.Check(sampleMethod)
			Assert.That(result, [Is].Empty)
		End Sub

		Private Function GetSampleMethod(methodName As String) As Method
			Return IntrospectionUtility.MethodFactory(Of ClassWithMultipleFragments)(methodName, New TypeNode() { IntrospectionUtility.TypeNodeFactory(Of Integer)() })
		End Function
	End Class
End Namespace
