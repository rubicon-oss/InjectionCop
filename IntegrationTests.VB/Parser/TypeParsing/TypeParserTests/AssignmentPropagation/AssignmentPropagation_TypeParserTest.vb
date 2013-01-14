Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.AssignmentPropagation
	<TestFixture()>
	Public Class AssignmentPropagation_TypeParserTest
		Inherits TypeParserTestBase

		<Category("AssignmentPropagation"), Test()>
		Public Sub Parse_ValidSafenessPropagation_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("ValidSafenessPropagation", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Category("AssignmentPropagation"), Test()>
		Public Sub Parse_InvalidSafenessPropagationParameter_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("InvalidSafenessPropagationParameter", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Category("AssignmentPropagation"), Test()>
		Public Sub Parse_ValidSafenessPropagationParameter_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("ValidSafenessPropagationParameter", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Category("AssignmentPropagation"), Test()>
		Public Sub Parse_InvalidSafenessPropagationVariable_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("InvalidSafenessPropagationVariable", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Category("AssignmentPropagation"), Test()>
		Public Sub Parse_ValidSafenessPropagationVariable_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("ValidSafenessPropagationVariable", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_ValidReturnWithIf_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("ValidReturnWithIf", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_InvalidReturnWithIf_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("InvalidReturnWithIf", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InvalidReturnWithIfFragmentTypeConsidered_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("InvalidReturnWithIfFragmentTypeConsidered", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InvalidReturnWithTempVariable_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("InvalidReturnWithTempVariable", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InvalidReturnWithParameterReset_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("InvalidReturnWithParameterReset", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_ValidReturnWithParameterReset_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("ValidReturnWithParameterReset", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_InvalidReturnWithFieldReset_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("InvalidReturnWithFieldReset", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_ValidReturnWithFieldReset_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("ValidReturnWithFieldReset", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_InvalidReturnWithField_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("InvalidReturnWithField", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_ValidReturnWithField_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("ValidReturnWithField", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_InvalidReturnWithFieldAndLoops_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("InvalidReturnWithFieldAndLoops", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_ValidReturnWithFieldAndLoops_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of AssignmentPropagationSample)("ValidReturnWithFieldAndLoops", New TypeNode() { stringTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub
	End Class
End Namespace
