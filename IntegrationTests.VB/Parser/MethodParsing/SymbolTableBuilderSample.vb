Imports InjectionCop.Fragment
Imports System

Namespace Parser.MethodParsing
	Friend Class SymbolTableBuilderSample
		<Fragment("FragmentType")>
		Protected _fragmentField As Integer = 0

		Protected _nonFragmentField As Object = New Object()

		Public Function ParameterizedMethod(nonFragmentParameter As Single, <Fragment("FragmentType")> fragmentParameter As Object) As String
      Return String.Concat(New Object() {fragmentParameter.ToString(), nonFragmentParameter, Me._fragmentField, Me._nonFragmentField})
		End Function
	End Class
End Namespace
