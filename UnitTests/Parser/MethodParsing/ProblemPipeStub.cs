﻿// Copyright 2012 rubicon informationstechnologie gmbh
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
using System.Collections.Generic;
using InjectionCop.Parser.ProblemPipe;

namespace InjectionCop.UnitTests.Parser.MethodParsing
{
  public class ProblemPipeStub : IProblemPipe
  {
    private readonly List<ProblemMetadata> _problems;

    public ProblemPipeStub ()
    {
      _problems = new List<ProblemMetadata>();
    }

    public void AddProblem (ProblemMetadata problemMetadata)
    {
      _problems.Add (problemMetadata);
    }

    public List<ProblemMetadata> Problems
    {
      get { return _problems; }
    }
  }
}
