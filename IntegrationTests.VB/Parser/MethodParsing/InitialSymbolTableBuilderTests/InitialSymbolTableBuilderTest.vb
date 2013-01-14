Imports InjectionCop.Config
Imports InjectionCop.Parser
Imports InjectionCop.Parser.MethodParsing
Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.MethodParsing.InitialSymbolTableBuilderTests
	<TestFixture()>
	Public Class InitialSymbolTableBuilderTest
		Private _blacklistManager As IBlacklistManager

		Private _floatType As TypeNode

		Private _objectType As TypeNode

		<SetUp()>
		Public Sub SetUp()
			Me._blacklistManager = New IDbCommandBlacklistManagerStub()
			Me._floatType = IntrospectionUtility.TypeNodeFactory(Of Single)()
			Me._objectType = IntrospectionUtility.TypeNodeFactory(Of Object)()
		End Sub

		<Test()>
		Public Sub GetResult_ParameterizedMethodSymbolTable_FragmentFieldIsListedAsEmptyFragment()
			Dim sampleMethod As Method = TestHelper.GetSample(Of SymbolTableBuilderSample)("ParameterizedMethod", New TypeNode() { Me._floatType, Me._objectType })
			Dim initialSymbolTableBuilder As InitialSymbolTableBuilder = New InitialSymbolTableBuilder(sampleMethod, Me._blacklistManager)
			Dim resultSymbolTable As ISymbolTable = initialSymbolTableBuilder.GetResult()
			Assert.That(resultSymbolTable.GetFragmentType("_fragmentField"), [Is].EqualTo(SymbolTable.EMPTY_FRAGMENT))
		End Sub

		<Test()>
		Public Sub GetResult_ParameterizedMethodSymbolTable_NonFragmentFieldIsEmptyFragment()
			Dim sampleMethod As Method = TestHelper.GetSample(Of SymbolTableBuilderSample)("ParameterizedMethod", New TypeNode() { Me._floatType, Me._objectType })
			Dim initialSymbolTableBuilder As InitialSymbolTableBuilder = New InitialSymbolTableBuilder(sampleMethod, Me._blacklistManager)
			Dim resultSymbolTable As ISymbolTable = initialSymbolTableBuilder.GetResult()
			Assert.That(resultSymbolTable.GetFragmentType("_nonFragmentField"), [Is].EqualTo(SymbolTable.EMPTY_FRAGMENT))
		End Sub

		<Test()>
		Public Sub GetResult_ParameterizedMethodSymbolTable_UnknownSymbolIsListedAsEmptyFragment()
			Dim sampleMethod As Method = TestHelper.GetSample(Of SymbolTableBuilderSample)("ParameterizedMethod", New TypeNode() { Me._floatType, Me._objectType })
			Dim initialSymbolTableBuilder As InitialSymbolTableBuilder = New InitialSymbolTableBuilder(sampleMethod, Me._blacklistManager)
			Dim resultSymbolTable As ISymbolTable = initialSymbolTableBuilder.GetResult()
			Assert.That(resultSymbolTable.GetFragmentType("unknownSymbol"), [Is].EqualTo(SymbolTable.EMPTY_FRAGMENT))
		End Sub

		<Test()>
		Public Sub GetResult_ParameterizedMethodSymbolTable_NonFragmentParameterIsEmptyFragment()
			Dim sampleMethod As Method = TestHelper.GetSample(Of SymbolTableBuilderSample)("ParameterizedMethod", New TypeNode() { Me._floatType, Me._objectType })
			Dim initialSymbolTableBuilder As InitialSymbolTableBuilder = New InitialSymbolTableBuilder(sampleMethod, Me._blacklistManager)
			Dim resultSymbolTable As ISymbolTable = initialSymbolTableBuilder.GetResult()
			Assert.That(resultSymbolTable.GetFragmentType("nonFragmentParameter"), [Is].EqualTo(SymbolTable.EMPTY_FRAGMENT))
		End Sub

		<Test()>
		Public Sub GetResult_ParameterizedMethodSymbolTable_FragmentParameterIsOfCorrectFragmentType()
			Dim sampleMethod As Method = TestHelper.GetSample(Of SymbolTableBuilderSample)("ParameterizedMethod", New TypeNode() { Me._floatType, Me._objectType })
			Dim initialSymbolTableBuilder As InitialSymbolTableBuilder = New InitialSymbolTableBuilder(sampleMethod, Me._blacklistManager)
			Dim resultSymbolTable As ISymbolTable = initialSymbolTableBuilder.GetResult()
			Assert.That(resultSymbolTable.GetFragmentType("fragmentParameter"), [Is].EqualTo("FragmentType"))
		End Sub
	End Class
End Namespace
