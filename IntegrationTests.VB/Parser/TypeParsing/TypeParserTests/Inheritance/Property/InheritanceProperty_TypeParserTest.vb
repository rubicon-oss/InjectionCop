Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Inheritance.[Property]
	<TestFixture()>
	Public Class InheritanceProperty_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_UnsafeAssignmentOnInheritedProperty_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeAssignmentOnInheritedProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeAssignmentOnInheritedPropertyWithLiteral_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeAssignmentOnInheritedPropertyWithLiteral", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SafeAssignmentOnInheritedProperty_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeAssignmentOnInheritedProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SafeAssignmentOnPropertyHidingParent_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeAssignmentOnPropertyHidingParent", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeAssignmentOnPropertyHidingParent_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeAssignmentOnPropertyHidingParent", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallWithInheritedProperty_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeCallWithInheritedProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallWithInheritedProperty_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeCallWithInheritedProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallWithPropertyHidingParent_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeCallWithPropertyHidingParent", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallWithPropertyHidingParent_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeCallWithPropertyHidingParent", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeStaticBindingOnNewProperty_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeStaticBindingOnNewProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeStaticBindingOnNewProperty_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeStaticBindingOnNewProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeSetOnOverriddenProperty_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeSetOnOverriddenProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_AnotherSafeSetOfOverriddenProperty_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "AnotherSafeSetOfOverriddenProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeSetOnOverriddenProperty_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeSetOnOverriddenProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeDynamicBindingOnProperty_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeDynamicBindingOnProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeDynamicBindingOnProperty_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeDynamicBindingOnProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeAssignmentOnBaseProperty_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeAssignmentOnBaseProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeAssignmentOnBaseProperty_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeAssignmentOnBaseProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeAssignmentOfBaseProperty_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeAssignmentOfBaseProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeAssignmentOfBaseProperty_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeAssignmentOfBaseProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeMethodCallUsingBaseProperty_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeMethodCallUsingBaseProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeMethodCallUsingBaseProperty_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleProperty)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeMethodCallUsingBaseProperty", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
