Imports InjectionCop.Config
Imports InjectionCop.Parser
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.SymbolTableTests.ParameterSafe
	<TestFixture()>
	Public Class ParameterSafe_SymbolTableTest
		Private _symbolTable As ISymbolTable

		<SetUp()>
		Public Sub SetUp()
			Me._symbolTable = New SymbolTable(New IDbCommandBlacklistManagerStub())
		End Sub
	End Class
End Namespace
