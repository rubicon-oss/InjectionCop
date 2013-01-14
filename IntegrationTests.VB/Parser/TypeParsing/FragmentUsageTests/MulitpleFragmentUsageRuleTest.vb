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
