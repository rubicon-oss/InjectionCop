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
Imports InjectionCop.Attributes
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
