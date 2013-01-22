// Copyright 2013 rubicon informationstechnologie gmbh
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
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.ProblemPipe
{
  public class ProblemMetadata
  {
    private readonly int _sourceExpressionId;
    private readonly SourceContext _sourceContext;
    private readonly Fragment _expectedFragment;
    private Fragment _givenFragment;

    public ProblemMetadata (int sourceExpressionId, SourceContext sourceContext, Fragment expectedFragment, Fragment givenFragment)
    {
      _sourceExpressionId = sourceExpressionId;
      _sourceContext = ArgumentUtility.CheckNotNull ("sourceContext", sourceContext);
      _expectedFragment = ArgumentUtility.CheckNotNull ("expectedFragment", expectedFragment);
      _givenFragment = givenFragment;
    }

    public int SourceExpressionId
    {
      get { return _sourceExpressionId; }
    }
  
    public SourceContext SourceContext
    {
      get { return _sourceContext; }
    }

    public Fragment ExpectedFragment
    {
      get { return _expectedFragment; }
    }

    public Fragment GivenFragment
    {
      get { return _givenFragment; }
      set { _givenFragment = value; }
    }
  }
}
