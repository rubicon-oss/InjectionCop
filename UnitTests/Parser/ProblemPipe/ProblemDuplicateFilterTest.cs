// Copyright 2012 rubicon informationstechnologie gmbh
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;
using Rhino.Mocks;
using InjectionCop.Parser.ProblemPipe;

namespace InjectionCop.UnitTests.Parser.ProblemPipe
{
  [TestFixture]
  public class ProblemDuplicateFilterTest
  {
    private MockRepository _mocks;
    private IProblemPipe _problemDestination;

    private const int c_sourceExpressionId1 = 1;
    private const int c_sourceExpressionId2 = 2;

    [SetUp]
    public void SetUp ()
    {
      _mocks = new MockRepository();
      _problemDestination = _mocks.StrictMock<IProblemPipe>();
    }

    [Test]
    public void AddProblem_MultipleProblemMetadataObjectsWithDifferentSourceExpressionIds_PassesObjects_ ()
    {
      ProblemMetadata problem1 = new ProblemMetadata(c_sourceExpressionId1, new SourceContext(), "dummy", "dummy");
      ProblemMetadata problem2 = new ProblemMetadata(c_sourceExpressionId2, new SourceContext(), "dummy", "dummy");

      using (_mocks.Record())
      {
        _problemDestination.AddProblem (problem1);
        _problemDestination.AddProblem (problem2);
      }

      var problemDuplicateFilter = new ProblemDuplicateFilter (_problemDestination);
      problemDuplicateFilter.AddProblem (problem1);
      problemDuplicateFilter.AddProblem (problem2);

      _mocks.Verify (_problemDestination);
    }

    [Test]
    public void AddProblem_MultipleProblemMetadataObjectsWithDuplicate_PassesObjectsWithoutDuplicates_ ()
    {
      ProblemMetadata problem1 = new ProblemMetadata(c_sourceExpressionId1, new SourceContext(), "dummy", "dummy");
      ProblemMetadata problem2 = new ProblemMetadata(c_sourceExpressionId2, new SourceContext(), "dummy", "dummy");
      ProblemMetadata problem3 = new ProblemMetadata(c_sourceExpressionId2, new SourceContext(), "dummy", "dummy");

      using (_mocks.Record())
      {
        _problemDestination.AddProblem (problem1);
        _problemDestination.AddProblem (problem2);
      }

      var problemDuplicateFilter = new ProblemDuplicateFilter (_problemDestination);
      problemDuplicateFilter.AddProblem (problem1);
      problemDuplicateFilter.AddProblem (problem2);
      problemDuplicateFilter.AddProblem (problem3);

      _mocks.Verify (_problemDestination);
    }
  }
}
