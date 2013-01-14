Imports InjectionCop.Fragment
Imports InjectionCop.IntegrationTests.VB.Parser.TypeParsing.TypeParserTests.AnonymousMethod
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Closure
	Public Class ClosureSample
		Inherits ParserSampleBase

		Public Delegate Function FragmentParameterDelegate() As String

		<Fragment("ClosureFragmentType")>
		Private _safeField As String

		Private _unsafeField As String

		Private Sub New()
			Me._safeField = "dummy"
			Me._unsafeField = "dummy"
		End Sub

		Public Sub SafeClosureUsingLocalVariable()
			Dim safeSource As String = "safe"
			Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(param0 As String, param1 As String) Me.RequiresClosureFragment(safeSource)
			fragmentDelegate("safe", "safe")
		End Sub

		Public Sub UnsafeClosureUsingLocalVariable()
			Dim safeSource As String = MyBase.UnsafeSource()
			Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(param0 As String, param1 As String) Me.RequiresClosureFragment(safeSource)
			fragmentDelegate("safe", "safe")
		End Sub

		Public Sub SafeClosureUsingField()
			Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(param0 As String, param1 As String) Me.RequiresClosureFragment(Me._safeField)
			fragmentDelegate("safe", "safe")
		End Sub

		Public Sub UnsafeClosureUsingField()
			Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(param0 As String, param1 As String) Me.RequiresClosureFragment(Me._unsafeField)
			fragmentDelegate("safe", "safe")
		End Sub

		Private Function RequiresClosureFragment(<Fragment("ClosureFragmentType")> fragmentParameter As String) As String
			Return fragmentParameter
		End Function
	End Class
End Namespace
