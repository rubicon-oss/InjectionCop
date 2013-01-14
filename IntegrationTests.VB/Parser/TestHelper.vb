Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports System
Imports System.Linq

Namespace Parser
  Public Class TestHelper
    Public Shared Function ContainsProblemID(id As String, result As ProblemCollection) As Boolean
      Return result.Any(Function(problem As Problem) problem.Id = id)
    End Function

    Public Shared Function GetSample(Of SampleClass)(methodName As String, ParamArray methodParameters As TypeNode()) As Method
      Return IntrospectionUtility.MethodFactory(Of SampleClass)(methodName, methodParameters)
    End Function

    Public Shared Function GetSample(targetType As System.Type, methodName As String, ParamArray methodParameters As TypeNode()) As Method
      Return IntrospectionUtility.MethodFactory(targetType, methodName, methodParameters)
    End Function
  End Class
End Namespace
