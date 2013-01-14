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
Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.[Interface]
	Public Interface InterfaceSample
		Sub ParameterLess()

		Sub NonFragmentParameter(parameter As Object)

		Sub FragmentParameter(parameter1 As Integer, <Fragment("Type")> parameter2 As Object, <SqlFragment()> parameter3 As String)
	End Interface
End Namespace
