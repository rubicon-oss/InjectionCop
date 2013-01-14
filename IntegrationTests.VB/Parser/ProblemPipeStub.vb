Imports InjectionCop.Parser.ProblemPipe
Imports System
Imports System.Collections.Generic

Namespace Parser
	Friend Class ProblemPipeStub
		Implements IProblemPipe

		Private _problems As System.Collections.Generic.List(Of ProblemMetadata)

		Public ReadOnly Property Problems() As System.Collections.Generic.List(Of ProblemMetadata)
			Get
				Return Me._problems
			End Get
		End Property

		Public Sub New()
			Me._problems = New System.Collections.Generic.List(Of ProblemMetadata)()
		End Sub

    Public Sub AddProblem(problemMetadata As ProblemMetadata) Implements IProblemPipe.AddProblem
      Me._problems.Add(problemMetadata)
    End Sub
	End Class
End Namespace
