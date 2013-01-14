Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Lambda
  Friend Class LambdaSample
    Inherits ParserSampleBase

    Public Delegate Function FragmentParameterDelegate(<Fragment("LambdaFragmentType")> fragmentParameter As String, nonFragmentParameter As String) As String

    Public Delegate Function ReturnFragmentDelegate() As <Fragment("LambdaFragmentType")>
    String

    Public Sub SafeLambdaCall()
      Dim fragmentDelegate As LambdaSample.FragmentParameterDelegate = Function(fragmentParameter As String, nonFragmentParameter As String) fragmentParameter + nonFragmentParameter
      fragmentDelegate("safe", "safe")
    End Sub

    Public Sub UnsafeLambdaCall()
      Dim fragmentDelegate As LambdaSample.FragmentParameterDelegate = Function(fragmentParameter As String, nonFragmentParameter As String) fragmentParameter + nonFragmentParameter
      fragmentDelegate(MyBase.UnsafeSource(), "safe")
    End Sub

    Public Sub SafeLambdaCallUsingReturn()
      Dim fragmentParameterDelegate As LambdaSample.FragmentParameterDelegate = Function(fragmentParameter As String, nonFragmentParameter As String) fragmentParameter + nonFragmentParameter
      Dim returnFragmentDelegate As LambdaSample.ReturnFragmentDelegate = Function() "safe"
      fragmentParameterDelegate(returnFragmentDelegate(), "safe")
    End Sub

    Public Sub SafeMethodCallInsideLambda()
      Dim fragmentDelegate As LambdaSample.FragmentParameterDelegate = Function(fragmentParameter As String, nonFragmentParameter As String)
                                                                         Me.RequiresLambdaFragment(fragmentParameter)
                                                                         Return fragmentParameter + nonFragmentParameter
                                                                       End Function
      fragmentDelegate("dummy", "dummy")
    End Sub

    Public Sub UnsafeMethodCallInsideLambda()
      Dim fragmentDelegate As LambdaSample.FragmentParameterDelegate = Function(fragmentParameter As String, nonFragmentParameter As String)
                                                                         Me.RequiresLambdaFragment(nonFragmentParameter)
                                                                         Return fragmentParameter + nonFragmentParameter
                                                                       End Function
      fragmentDelegate("dummy", "dummy")
    End Sub

    Public Sub SafeReturnInsideLambda()
      Dim returnFragmentDelegate As LambdaSample.ReturnFragmentDelegate = Function() "safe"
      returnFragmentDelegate()
    End Sub

    Public Sub UnsafeReturnInsideLambda()
      Dim returnFragmentDelegate As LambdaSample.ReturnFragmentDelegate = Function() MyBase.UnsafeSource()
      returnFragmentDelegate()
    End Sub

    Private Sub RequiresLambdaFragment(<Fragment("LambdaFragmentType")> fragmentParameter As String)
      MyBase.DummyMethod(fragmentParameter)
    End Sub
  End Class
End Namespace
