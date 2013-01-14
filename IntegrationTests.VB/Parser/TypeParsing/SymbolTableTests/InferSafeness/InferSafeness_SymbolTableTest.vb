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
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.SymbolTableTests.InferSafeness
	<TestFixture()>
	Public Class InferSafeness_SymbolTableTest
		Private _symbolTable As SymbolTable

		<SetUp()>
		Public Sub SetUp()
			Dim blacklistManager As IBlacklistManager = New IDbCommandBlacklistManagerStub()
			Me._symbolTable = New SymbolTable(blacklistManager)
		End Sub

		<Test()>
		Public Sub InferSafeness_NullSymbolName_ToleratedAndNoExceptionThrown()
			Dim sample As Method = TestHelper.GetSample(Of InferSafenessSample)("Foo", New TypeNode() {})
			Dim targetBlock As Block = CType(sample.Body.Statements(0), Block)
			Dim targetAssignment As AssignmentStatement = CType(targetBlock.Statements(1), AssignmentStatement)
			Dim targetExpression As Expression = targetAssignment.Target
			Me._symbolTable.InferSafeness(Nothing, targetExpression)
		End Sub
	End Class
End Namespace
