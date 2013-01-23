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
Imports InjectionCop.Config
Imports InjectionCop.Parser
Imports InjectionCop.Parser.MethodParsing
Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.MethodParsing.EmbeddedInitialSymbolTableBuilderTest
	<TestFixture()>
	Public Class EmbeddedInitialSymbolTableBuilderTest
		Private _blacklistManager As IBlacklistManager

		Private _environment As ISymbolTable

		Private _floatType As TypeNode

		Private _objectType As TypeNode

		<SetUp()>
		Public Sub SetUp()
			Me._blacklistManager = New IDbCommandBlacklistManagerStub()
			Me._environment = New SymbolTable(Me._blacklistManager)
			Me._floatType = IntrospectionUtility.TypeNodeFactory(Of Single)()
			Me._objectType = IntrospectionUtility.TypeNodeFactory(Of Object)()
		End Sub

		<Test()>
		Public Sub GetResult_ParameterizedMethodSymbolTableBuiltWithEmptyEnvironment_FragmentFieldIsEmptyFragment()
			Dim sampleMethod As Method = TestHelper.GetSample(Of SymbolTableBuilderSample)("ParameterizedMethod", New TypeNode() { Me._floatType, Me._objectType })
			Dim embeddedInitialSymbolTableBuilder As EmbeddedInitialSymbolTableBuilder = New EmbeddedInitialSymbolTableBuilder(sampleMethod, Me._blacklistManager, Me._environment)
			Dim resultSymbolTable As ISymbolTable = embeddedInitialSymbolTableBuilder.GetResult()
      Assert.That(resultSymbolTable.GetFragmentType("_fragmentField"), [Is].EqualTo(Fragment.CreateEmpty()))
		End Sub

		<Test()>
		Public Sub GetResult_ParameterizedMethodSymbolTableBuiltWithOverlappingEnvironment_FragmentFieldIsEmptyFragment()
			Dim sampleMethod As Method = TestHelper.GetSample(Of SymbolTableBuilderSample)("ParameterizedMethod", New TypeNode() { Me._floatType, Me._objectType })
			Dim embeddedInitialSymbolTableBuilder As EmbeddedInitialSymbolTableBuilder = New EmbeddedInitialSymbolTableBuilder(sampleMethod, Me._blacklistManager, Me._environment)
      Me._environment.MakeSafe("_fragmentField", Fragment.CreateNamed("ThisShouldBeIgnored"))
			Dim resultSymbolTable As ISymbolTable = embeddedInitialSymbolTableBuilder.GetResult()
      Assert.That(resultSymbolTable.GetFragmentType("_fragmentField"), [Is].EqualTo(Fragment.CreateEmpty()))
		End Sub

		<Test()>
		Public Sub GetResult_ParameterizedMethodSymbolTableBuiltWithNonOverlappingEnvironment_FragmentFieldIsEmptyFragment()
			Dim sampleMethod As Method = TestHelper.GetSample(Of SymbolTableBuilderSample)("ParameterizedMethod", New TypeNode() { Me._floatType, Me._objectType })
			Dim embeddedInitialSymbolTableBuilder As EmbeddedInitialSymbolTableBuilder = New EmbeddedInitialSymbolTableBuilder(sampleMethod, Me._blacklistManager, Me._environment)
      Me._environment.MakeSafe("_nonFragmentField", Fragment.CreateNamed("ThisShouldBeIgnored"))
			Dim resultSymbolTable As ISymbolTable = embeddedInitialSymbolTableBuilder.GetResult()
      Assert.That(resultSymbolTable.GetFragmentType("_fragmentField"), [Is].EqualTo(Fragment.CreateEmpty()))
		End Sub

		<Test()>
		Public Sub GetResult_ParameterizedMethodSymbolTableBuiltWithEmptyEnvironment_NonFragmentFieldIsEmptyFragment()
			Dim sampleMethod As Method = TestHelper.GetSample(Of SymbolTableBuilderSample)("ParameterizedMethod", New TypeNode() { Me._floatType, Me._objectType })
			Dim embeddedInitialSymbolTableBuilder As EmbeddedInitialSymbolTableBuilder = New EmbeddedInitialSymbolTableBuilder(sampleMethod, Me._blacklistManager, Me._environment)
			Dim resultSymbolTable As ISymbolTable = embeddedInitialSymbolTableBuilder.GetResult()
      Assert.That(resultSymbolTable.GetFragmentType("_nonFragmentField"), [Is].EqualTo(Fragment.CreateEmpty()))
    End Sub

    <Test()>
    Public Sub GetResult_ParameterizedMethodSymbolTableBuiltWithNonOverlappingEnvironment_EnvironmentSymbolIsPropagated()
      Dim sampleMethod As Method = TestHelper.GetSample(Of SymbolTableBuilderSample)("ParameterizedMethod", New TypeNode() {Me._floatType, Me._objectType})
      Dim embeddedInitialSymbolTableBuilder As EmbeddedInitialSymbolTableBuilder = New EmbeddedInitialSymbolTableBuilder(sampleMethod, Me._blacklistManager, Me._environment)
      Me._environment.MakeSafe("environmentSymbol", Fragment.CreateNamed("DummyType"))
      Dim resultSymbolTable As ISymbolTable = embeddedInitialSymbolTableBuilder.GetResult()
      Assert.That(resultSymbolTable.GetFragmentType("environmentSymbol"), [Is].EqualTo(Fragment.CreateNamed("DummyType")))
    End Sub

    <Test()>
    Public Sub GetResult_ParameterizedMethodSymbolTableBuiltWithOverlappingNonFragmentField_EnvironmentSymbolIsPropagated()
      Dim sampleMethod As Method = TestHelper.GetSample(Of SymbolTableBuilderSample)("ParameterizedMethod", New TypeNode() {Me._floatType, Me._objectType})
      Dim embeddedInitialSymbolTableBuilder As EmbeddedInitialSymbolTableBuilder = New EmbeddedInitialSymbolTableBuilder(sampleMethod, Me._blacklistManager, Me._environment)
      Me._environment.MakeSafe("_nonFragmentField", Fragment.CreateNamed("OverlappingType"))
      Dim resultSymbolTable As ISymbolTable = embeddedInitialSymbolTableBuilder.GetResult()
      Assert.That(resultSymbolTable.GetFragmentType("_nonFragmentField"), [Is].EqualTo(Fragment.CreateNamed("OverlappingType")))
    End Sub

    <Test()>
    Public Sub GetResult_ParameterizedMethodSymbolTableBuiltWithEmptyEnvironment_UnknownSymbolIsEmptyFragment()
      Dim sampleMethod As Method = TestHelper.GetSample(Of SymbolTableBuilderSample)("ParameterizedMethod", New TypeNode() {Me._floatType, Me._objectType})
      Dim embeddedInitialSymbolTableBuilder As EmbeddedInitialSymbolTableBuilder = New EmbeddedInitialSymbolTableBuilder(sampleMethod, Me._blacklistManager, Me._environment)
      Dim resultSymbolTable As ISymbolTable = embeddedInitialSymbolTableBuilder.GetResult()
      Assert.That(resultSymbolTable.GetFragmentType("unknownSymbol"), [Is].EqualTo(Fragment.CreateEmpty()))
    End Sub

    <Test()>
    Public Sub GetResult_ParameterizedMethodSymbolTableBuiltWithNonOverlappingEnvironment_NonFragmentParameterIsEmptyFragment()
      Dim sampleMethod As Method = TestHelper.GetSample(Of SymbolTableBuilderSample)("ParameterizedMethod", New TypeNode() {Me._floatType, Me._objectType})
      Dim embeddedInitialSymbolTableBuilder As EmbeddedInitialSymbolTableBuilder = New EmbeddedInitialSymbolTableBuilder(sampleMethod, Me._blacklistManager, Me._environment)
      Dim resultSymbolTable As ISymbolTable = embeddedInitialSymbolTableBuilder.GetResult()
      Assert.That(resultSymbolTable.GetFragmentType("nonFragmentParameter"), [Is].EqualTo(Fragment.CreateEmpty()))
    End Sub

		<Test()>
		Public Sub GetResult_ParameterizedMethodSymbolTableBuiltWithNonOverlappingEnvironment_FragmentParameterIsOfCorrectFragmentType()
			Dim sampleMethod As Method = TestHelper.GetSample(Of SymbolTableBuilderSample)("ParameterizedMethod", New TypeNode() { Me._floatType, Me._objectType })
			Dim embeddedInitialSymbolTableBuilder As EmbeddedInitialSymbolTableBuilder = New EmbeddedInitialSymbolTableBuilder(sampleMethod, Me._blacklistManager, Me._environment)
			Dim resultSymbolTable As ISymbolTable = embeddedInitialSymbolTableBuilder.GetResult()
      Assert.That(resultSymbolTable.GetFragmentType("fragmentParameter"), [Is].EqualTo(Fragment.CreateNamed("FragmentType")))
		End Sub

		<Test()>
		Public Sub GetResult_ParameterizedMethodSymbolTableBuiltWithParameterOverlappingEnvironment_FragmentParameterIsOfPropagatedFragmentType()
			Dim sampleMethod As Method = TestHelper.GetSample(Of SymbolTableBuilderSample)("ParameterizedMethod", New TypeNode() { Me._floatType, Me._objectType })
      Me._environment.MakeSafe("fragmentParameter", Fragment.CreateNamed("OverlappingType"))
			Dim embeddedInitialSymbolTableBuilder As EmbeddedInitialSymbolTableBuilder = New EmbeddedInitialSymbolTableBuilder(sampleMethod, Me._blacklistManager, Me._environment)
			Dim resultSymbolTable As ISymbolTable = embeddedInitialSymbolTableBuilder.GetResult()
      Assert.That(resultSymbolTable.GetFragmentType("fragmentParameter"), [Is].EqualTo(Fragment.CreateNamed("OverlappingType")))
		End Sub
	End Class
End Namespace
