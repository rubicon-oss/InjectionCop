Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.BlacklistManagerIntegration
	<Ignore("XML interface is in experimental state"), TestFixture()>
	Public Class BlacklistManagerIntegration_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_UnsafeBlacklistedCall_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of BlacklistManagerIntegrationSample)("UnsafeBlacklistedCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeBlacklistedCall_ReturnsExactlyOneProblem()
			Dim sample As Method = TestHelper.GetSample(Of BlacklistManagerIntegrationSample)("UnsafeBlacklistedCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(result.Count, [Is].EqualTo(1))
		End Sub

		<Test()>
		Public Sub Parse_SafeBlacklistedCall_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of BlacklistManagerIntegrationSample)("SafeBlacklistedCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(result.Count, [Is].EqualTo(0))
		End Sub

		<Test()>
		Public Sub Parse_ListedAndUnlistedViolation_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of BlacklistManagerIntegrationSample)("ListedAndUnlistedViolation", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_ListedAndUnlistedViolation_ReturnsExactlyTwoProblems()
			Dim sample As Method = TestHelper.GetSample(Of BlacklistManagerIntegrationSample)("ListedAndUnlistedViolation", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(result.Count, [Is].EqualTo(2))
		End Sub

		<Test()>
		Public Sub Parse_MixedViolations_ReturnsProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of BlacklistManagerIntegrationSample)("MixedViolations", New TypeNode() { intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_FragmentDefinedInXmlSafeCall_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of BlacklistManagerIntegrationSample)("FragmentDefinedInXmlSafeCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_FragmentDefinedInXmlUnsafeCall_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of BlacklistManagerIntegrationSample)("FragmentDefinedInXmlUnsafeCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_MixedViolations_FindsAllProblems()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of BlacklistManagerIntegrationSample)("MixedViolations", New TypeNode() { intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Dim problemIdsCorrect As Boolean = True
			For Each problem As Problem In result
				problemIdsCorrect = (problemIdsCorrect AndAlso problem.Id = Me.c_InjectionCopRuleId)
			Next
			Dim allViolationsFound As Boolean = result.Count = 4
			Assert.That(problemIdsCorrect AndAlso allViolationsFound, [Is].[True])
		End Sub
	End Class
End Namespace
