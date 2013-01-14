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
