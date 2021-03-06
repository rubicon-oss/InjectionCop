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
Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Inheritance.MemberMethod
	<TestFixture()>
	Public Class InheritanceMethod_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_SafeCallOnInheritedMethod_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleMethod)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeCallOnInheritedMethod", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallOnInheritedMethod_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleMethod)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeCallOnInheritedMethod", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallOnMethodInheritedFromSuperiorClass_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleMethod)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeCallOnMethodInheritedFromSuperiorClass", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallOnMethodInheritedFromSuperiorClass_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleMethod)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeCallOnMethodInheritedFromSuperiorClass", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallOnNewMethod_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleMethod)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeCallOnNewMethod", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallOnNewMethod_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleMethod)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeCallOnNewMethod", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeStaticBindingOnNewMethod_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleMethod)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeStaticBindingOnNewMethod", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeStaticBindingOnNewMethod_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleMethod)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeStaticBindingOnNewMethod", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallBaseMethod_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleMethod)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeCallBaseMethod", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallBaseMethod_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleMethod)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeCallBaseMethod", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallOnOverriddenMethod_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleMethod)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeCallOnOverriddenMethod", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_AnotherSafeCallOnOverriddenMethod_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleMethod)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "AnotherSafeCallOnOverriddenMethod", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallOnOverriddenMethod_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleMethod)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeCallOnOverriddenMethod", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeDynamicBinding_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleMethod)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeDynamicBinding", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeDynamicBinding_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleMethod)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeDynamicBinding", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
