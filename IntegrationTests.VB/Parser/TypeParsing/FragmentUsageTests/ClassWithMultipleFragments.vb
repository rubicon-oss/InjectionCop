Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.FragmentUsageTests
	Public Class ClassWithMultipleFragments
		Public Sub InvalidFragmentUsage(<Fragment("FirstFragment"), SqlFragment()> parameter As Integer)
		End Sub

		Public Sub ValidFragmentUsage(<Fragment("FirstFragment")> parameter As Integer)
		End Sub

		Public Sub NoFragmentUsage(parameter As Integer)
		End Sub
	End Class
End Namespace
