Imports InjectionCop.Fragment
Imports System

Namespace Parser
	Public Class ParserSampleBase
		<SqlFragment()>
		Protected _fragmentField As String = "safe"

		Protected Function UnsafeSource() As String
      Return "unsafe command"
		End Function

		Protected Function UnsafeSource(param As String) As String
			Return param + "unsafe"
		End Function

		Protected Function SafeSource() As<Fragment("SqlFragment")>
		String
			Return"safe command"
		End Function

		Protected Function SafeSourceRequiresSqlFragment(<SqlFragment()> param As String) As<Fragment("SqlFragment")>
		String
			Return Me._fragmentField
		End Function

		Protected Sub DummyMethod(parameter As String)
		End Sub

		Protected Function RequiresSqlFragment(<Fragment("SqlFragment")> param As String) As String
			Return param
		End Function

		Protected Function RequiresSqlFragment(<Fragment("SqlFragment")> a As String, b As String, <Fragment("SqlFragment")> c As String) As String
			Return a + b + c
		End Function

		Protected Function RequiresSqlFragmentReturnsBool(<SqlFragment()> param As String) As Boolean
			Return True
		End Function

		Public Function ReturnsHtmlFragment() As<Fragment("HtmlFragment")>
		String
			Return"HtmlFragment"
		End Function
	End Class
End Namespace
