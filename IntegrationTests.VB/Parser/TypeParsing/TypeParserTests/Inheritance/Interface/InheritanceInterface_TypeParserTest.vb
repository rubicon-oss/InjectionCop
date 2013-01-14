Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Inheritance.[Interface]
	<TestFixture()>
	Public Class InheritanceInterface_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_SafeCallOnInheritedMethod_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleInterface)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeCallOnInterfaceMethodWithFragmentParameter", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallOnInterfaceMethodWithFragmentParameter_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleInterface)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeCallOnInterfaceMethodWithFragmentParameter", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InterfaceReturnFragmentsAreConsidered_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleInterface)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "InterfaceReturnFragmentsAreConsidered", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallOnExplicitInterfaceMethodWithFragmentParameter_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleInterface)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeCallOnExplicitInterfaceMethodWithFragmentParameter", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallOnExplicitInterfaceMethodWithFragmentParameter_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleInterface)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeCallOnExplicitInterfaceMethodWithFragmentParameter", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InterfaceReturnFragmentsOfExplicitlyDeclaredMethodAreConsidered_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleInterface)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "InterfaceReturnFragmentsOfExplicitlyDeclaredMethodAreConsidered", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallOnClassImplementingInterfaceMethodWithFragmentParameter_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleInterface)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeCallOnClassImplementingInterfaceMethodWithFragmentParameter", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallOnClassImplementingInterfaceMethodWithFragmentParameter_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleInterface)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeCallOnClassImplementingInterfaceMethodWithFragmentParameter", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InterfaceReturnFragmentsOfClassImplementingInterfaceMethodAreConsidered_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleInterface)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "InterfaceReturnFragmentsOfClassImplementingInterfaceMethodAreConsidered", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_ValidReturnOnImplicitInterfaceMethod_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(GetType(InterfaceSampleImplicitDeclarations))
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "MethodWithReturnFragment", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_InvalidReturnOnImplicitInterfaceMethod_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(GetType(InterfaceSampleImplicitDeclarationsInvalidReturn))
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "MethodWithReturnFragment", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_ValidReturnOnExplicitInterfaceMethod_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(GetType(InterfaceSampleExplicitDeclarations))
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Inheritance.Interface.IInheritanceSample.MethodWithReturnFragment", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_InvalidReturnOnExplicitInterfaceMethod_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(GetType(InterfaceSampleExplicitDeclarationsInvalidReturn))
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Inheritance.Interface.IInheritanceSample.MethodWithReturnFragment", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallOnClassImplementingMethodWithFragmentParameterFromMultipleInterfaces_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleInterface)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeCallOnClassImplementingMethodWithFragmentParameterFromMultipleInterfaces", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallOnClassImplementingMethodWithFragmentParameterFromMultipleInterfaces_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleInterface)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeCallOnClassImplementingMethodWithFragmentParameterFromMultipleInterfaces", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InterfaceReturnFragmentsOfClassImplementingInterfaceMethodFromMultipleInterfacesAreConsidered_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleInterface)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "InterfaceReturnFragmentsOfClassImplementingInterfaceMethodFromMultipleInterfacesAreConsidered", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallOnExtendedInterfaceMethodWithFragmentParameter_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleInterface)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeCallOnExtendedInterfaceMethodWithFragmentParameter", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallOnExtendedInterfaceMethodWithFragmentParameter_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleInterface)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeCallOnExtendedInterfaceMethodWithFragmentParameter", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_ExtendedInterfaceReturnFragmentsAreConsidered_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleInterface)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "ExtendedInterfaceReturnFragmentsAreConsidered", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_ValidReturnOnImplicitExtendedInterfaceMethod_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(GetType(ExtendedInterfaceSampleImplicitDeclarations))
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "MethodWithReturnFragment", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_InvalidReturnOnImplicitExtendedInterfaceMethod_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(GetType(ExtendedInterfaceSampleImplicitDeclarationsInvalidReturn))
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "MethodWithReturnFragment", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
