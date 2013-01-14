Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Indexer
  Friend Class IndexerSample
    Inherits ParserSampleBase

    Public Function IndexerSafeAssignmentOnParameter(<Fragment("IndexerFragment")> arrayParameter As String()) As <Fragment("IndexerFragment")>
    String()
      arrayParameter(0) = Me.SafeIndexerFragmentSource()
      Return arrayParameter
    End Function

    Public Function IndexerUnsafeAssignmentOnParameter(<Fragment("IndexerFragment")> arrayParameter As String()) As <Fragment("IndexerFragment")>
    String()
      arrayParameter(0) = MyBase.UnsafeSource()
      Return arrayParameter
    End Function

    Public Sub SafeCallUsingIndexer(<Fragment("IndexerFragment")> arrayParameter As String())
      Me.RequiresIndexerFragment(arrayParameter(0))
    End Sub

    Public Sub UnsafeCallUsingIndexer(arrayParameter As String())
      Me.RequiresIndexerFragment(arrayParameter(0))
    End Sub

    Public Sub UnsafeCallWithElementSetUnsafeByIndexer(<Fragment("IndexerFragment")> arrayParameter As String())
      arrayParameter(1) = MyBase.UnsafeSource()
      Me.RequiresIndexerFragment(arrayParameter(0))
    End Sub

    Public Sub UnsafeCallWithSingleVariableSetSafeByIndexer(arrayParameter As String())
      arrayParameter(1) = Me.SafeIndexerFragmentSource()
      Me.RequiresIndexerFragment(arrayParameter(0))
    End Sub

    Public Sub SafeCallUsingArray()
      Dim safeArray As String() = Me.SafeIndexerFragmentArraySource()
      Me.RequiresIndexerFragment(safeArray(0))
    End Sub

    Public Sub UnsafeCallUsingIndexerArray()
      Dim unsafeArray As String() = New String() {"unsafe"}
      Me.RequiresIndexerFragment(unsafeArray(0))
    End Sub

    Private Function SafeIndexerFragmentSource() As <Fragment("IndexerFragment")>
    String
      Return "safe"
    End Function

    Private Function SafeIndexerFragmentArraySource() As <Fragment("IndexerFragment")>
    String()
      Return New String() {"safe"}
    End Function

    Private Sub RequiresIndexerFragment(<Fragment("IndexerFragment")> indexerFragment As String)
      MyBase.DummyMethod(indexerFragment)
    End Sub
  End Class
End Namespace
