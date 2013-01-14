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
