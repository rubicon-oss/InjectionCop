Imports InjectionCop.Config
Imports InjectionCop.Parser.MethodParsing
Imports InjectionCop.Parser.ProblemPipe
Imports InjectionCop.Parser.TypeParsing
Imports Microsoft.FxCop.Sdk
Imports System

Namespace Parser.MethodParsing.MethodGraphTests
	Public Class MethodGraph_TestBase
		Protected Function BuildMethodGraph(method As Method) As IMethodGraph
			Dim problemPipe As IProblemPipe = New TypeParser()
			Dim blacklistManager As IBlacklistManager = New IDbCommandBlacklistManagerStub()
			Dim methodGraphBuilder As IMethodGraphBuilder = New MethodGraphBuilder(method, blacklistManager, problemPipe)
			methodGraphBuilder.Build()
			Return methodGraphBuilder.GetResult()
		End Function
	End Class
End Namespace
