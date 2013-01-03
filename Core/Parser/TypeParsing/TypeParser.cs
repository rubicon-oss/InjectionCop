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
using System.Diagnostics;
using InjectionCop.Config;
using InjectionCop.Parser.MethodParsing;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.TypeParsing
{
  /// <summary>
  /// Checks all members of a given type for safeness violations
  /// </summary>
  public class TypeParser : BaseFxCopRule, IProblemPipe
  {
    private readonly IBlacklistManager _blacklistManager;
    private readonly IProblemPipe _problemFilter;

    public TypeParser ()
        : base ("TypeParser")
    {
      _blacklistManager = ConfigLoader.LoadBlacklist();
      _problemFilter = new ProblemDuplicateFilter (this);
      //Debugger.Launch();
    }

    public override ProblemCollection Check (TypeNode type)
    {
      ArgumentUtility.CheckNotNull ("type", type);
      foreach (Member member in type.Members)
      {
        if (member is Method)
        {
          Method method = (Method) member;
          Parse (method);
        }
      }
      return Problems;
    }
    
    public void AddProblem (ProblemMetadata problemMetadata)
    {
      ArgumentUtility.CheckNotNull ("problemMetadata", problemMetadata);
      Resolution resolution = GetResolution (problemMetadata.ExpectedFragment, problemMetadata.GivenFragment);
      Problem problem = new Problem (resolution, problemMetadata.SourceContext, CheckId);
      Problems.Add (problem);
    }
    
    public void Parse (Method method)
    {
      ArgumentUtility.CheckNotNull ("method", method);
      IMethodGraphAnalyzer methodParser = new MethodGraphAnalyzer (_problemFilter);
      IMethodGraphBuilder methodGraphBuilder = new MethodGraphBuilder (method, _blacklistManager, _problemFilter);
      IInitialSymbolTableBuilder parameterSymbolTableBuilder = new InitialSymbolTableBuilder (method, _blacklistManager);
      methodParser.Parse (methodGraphBuilder, parameterSymbolTableBuilder);
    }
  }
}
