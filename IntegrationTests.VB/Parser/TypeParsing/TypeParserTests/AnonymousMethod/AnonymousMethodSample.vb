Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.AnonymousMethod
  Friend Class AnonymousMethodSample
    Inherits ParserSampleBase

    Public Delegate Function FragmentParameterDelegate(<Fragment("AnonymousMethodFragmentType")> fragmentParameter As String, nonFragmentParameter As String) As String

    Public Delegate Function ReturnFragmentDelegate() As <Fragment("AnonymousMethodFragmentType")>
    String

    Public Sub SafeAnonymousMethodCall()
      Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(fragmentParameter As String, nonFragmentParameter As String) fragmentParameter + nonFragmentParameter
      fragmentDelegate("safe", "safe")
    End Sub

    Public Sub UnsafeAnonymousMethodCall()
      Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(fragmentParameter As String, nonFragmentParameter As String) fragmentParameter + nonFragmentParameter
      fragmentDelegate(MyBase.UnsafeSource(), "safe")
    End Sub

    Public Sub SafeAnonymousMethodCallUsingReturn()
      Dim fragmentParameterDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(fragmentParameter As String, nonFragmentParameter As String) fragmentParameter + nonFragmentParameter
      Dim returnFragmentDelegate As AnonymousMethodSample.ReturnFragmentDelegate = Function() "safe"
      fragmentParameterDelegate(returnFragmentDelegate(), "safe")
    End Sub

    Public Sub SafeMethodCallInsideAnonymousMethod()
      Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(fragmentParameter As String, nonFragmentParameter As String)
                                                                                  Me.RequiresAnonymousMethodFragment(fragmentParameter)
                                                                                  Return fragmentParameter + nonFragmentParameter
                                                                                End Function
      fragmentDelegate("safe", "safe")
    End Sub

    Public Sub UnsafeMethodCallInsideAnonymousMethod()
      Dim fragmentDelegate As AnonymousMethodSample.FragmentParameterDelegate = Function(fragmentParameter As String, nonFragmentParameter As String)
                                                                                  Me.RequiresAnonymousMethodFragment(nonFragmentParameter)
                                                                                  Return fragmentParameter + nonFragmentParameter
                                                                                End Function
      fragmentDelegate("safe", "safe")
    End Sub

    Public Sub SafeReturnInsideAnonymousMethod()
      Dim returnFragmentDelegate As AnonymousMethodSample.ReturnFragmentDelegate = Function() "safe"
      returnFragmentDelegate()
    End Sub

    Public Sub UnsafeReturnInsideAnonymousMethod()
      Dim returnFragmentDelegate As AnonymousMethodSample.ReturnFragmentDelegate = Function() MyBase.UnsafeSource()
      returnFragmentDelegate()
    End Sub

    Private Sub RequiresAnonymousMethodFragment(<Fragment("AnonymousMethodFragmentType")> fragmentParameter As String)
      MyBase.DummyMethod(fragmentParameter)
    End Sub
  End Class
End Namespace
