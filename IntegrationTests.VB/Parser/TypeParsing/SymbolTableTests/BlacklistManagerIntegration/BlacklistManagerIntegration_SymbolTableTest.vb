Imports InjectionCop.Config
Imports InjectionCop.Parser
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.SymbolTableTests.BlacklistManagerIntegration
	<TestFixture()>
	Public Class BlacklistManagerIntegration_SymbolTableTest
		Private _symbolTable As ISymbolTable

		<SetUp()>
		Public Sub SetUp()
			Dim blacklistManager As IBlacklistManager = New IDbCommandBlacklistManagerStub()
			Me._symbolTable = New SymbolTable(blacklistManager)
		End Sub
	End Class
End Namespace
