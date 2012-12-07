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
using InjectionCop.Config;
using InjectionCop.Parser.MethodParsing;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Parser.TypeParsing;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.IntegrationTests.Parser.MethodParsing.MethodGraphTests
{
  public class MethodGraph_TestBase
  {
    protected IMethodGraph BuildMethodGraph (Method method)
    {
      IProblemPipe problemPipe = new TypeParser();
      IBlacklistManager blacklistManager = new IDbCommandBlacklistManagerStub();
      IMethodGraphBuilder methodGraphBuilder = new MethodGraphBuilder (method, blacklistManager, problemPipe);
      methodGraphBuilder.Build();
      IMethodGraph methodGraph = methodGraphBuilder.GetResult();
      return methodGraph;
    }
  }
}
