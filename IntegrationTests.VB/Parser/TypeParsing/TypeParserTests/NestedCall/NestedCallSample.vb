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
