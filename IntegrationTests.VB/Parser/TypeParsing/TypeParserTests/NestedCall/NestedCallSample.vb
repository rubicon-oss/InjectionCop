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
Imports System

Namespace Parser.TypeParsing.TypeParserTests.NestedCall
	Friend Class NestedCallSample
		Inherits ParserSampleBase

		Public Function NestedValidCallReturn() As Boolean
			Return MyBase.RequiresSqlFragmentReturnsBool("safe")
		End Function

		Public Function NestedInvalidCallReturn() As Boolean
			Return MyBase.RequiresSqlFragmentReturnsBool(MyBase.UnsafeSource())
		End Function

		Public Sub NestedInvalidCall()
			MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
		End Sub

		Public Function DeeperNestedInvalidCall() As Boolean
			Return"dummy" = MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
		End Function

		Public Sub ValidMethodCallChain()
			MyBase.RequiresSqlFragment(MyBase.SafeSourceRequiresSqlFragment(MyBase.SafeSourceRequiresSqlFragment("safe")), "safe", "safe")
		End Sub

		Public Sub InvalidMethodCallChain()
			MyBase.RequiresSqlFragment(MyBase.SafeSourceRequiresSqlFragment(MyBase.SafeSourceRequiresSqlFragment(MyBase.UnsafeSource())), "safe", "safe")
		End Sub

		Public Sub ValidMethodCallChainDifferentOperand()
			MyBase.RequiresSqlFragment("safe", "safe", MyBase.SafeSourceRequiresSqlFragment(MyBase.SafeSourceRequiresSqlFragment("safe")))
		End Sub

		Public Sub InvalidMethodCallChainDifferentOperand()
			MyBase.RequiresSqlFragment("safe", "safe", MyBase.SafeSourceRequiresSqlFragment(MyBase.SafeSourceRequiresSqlFragment(MyBase.UnsafeSource())))
		End Sub
	End Class
End Namespace
