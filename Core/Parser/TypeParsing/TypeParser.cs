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
using System.Diagnostics;
using System.Linq;
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
    private readonly IProblemPipe _problemFilter;
    private IBlacklistManager _blacklistManager;
    private MethodProfilingResults _methodProfilingResults;

    public TypeParser () : this(null)
    {
    }

    public TypeParser (IBlacklistManager blacklistManager)
        : base ("TypeParser")
    {
      _problemFilter = new ProblemDuplicateFilter (this);
      _blacklistManager = blacklistManager;
    }

    public override ProblemCollection Check (TypeNode type)
    {
      ArgumentUtility.CheckNotNull ("type", type);
      if (!IntrospectionUtility.IsCompilerGenerated (type))
      {
        InitializeBlacklistManager (type);
        CheckMembers (type);
      }
      return Problems;
    }

    private void CheckMembers (TypeNode type)
    {
      foreach (Member member in type.Members)
      {

        if (member is Method)
        {
          var method = (Method) member;
          var fragmentSignature = GetFragmentSignatureFromConfiguration(type, method);

          if (!FragmentUtility.IsFragmentGenerator (method)
              && (fragmentSignature == null || !fragmentSignature.IsGenerator))
          {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Parse (method);
            stopwatch.Stop();
            _methodProfilingResults.Add (method.FullName, stopwatch.Elapsed);
          }
        }
      }
    }

    private FragmentSignature GetFragmentSignatureFromConfiguration (TypeNode type, Method method)
    {
      var parameterTypes = method.Parameters.Select (_ => _.Type.FullName).ToList();
      var fragmentSignature = _blacklistManager.GetFragmentTypes (type.DeclaringModule.Name, type.FullName, method.Name.Name, parameterTypes);
      
      return fragmentSignature;
    }

    public void InitializeBlacklistManager (TypeNode type)
    {
      if (_blacklistManager == null)
        _blacklistManager = ConfigurationFactory.CreateFrom (type, new ConfigurationFileLocator());
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

    public override void BeforeAnalysis ()
    {
      base.BeforeAnalysis ();
      _methodProfilingResults = new MethodProfilingResults ();
    }

    public override void AfterAnalysis ()
    {
      base.AfterAnalysis();
      Console.WriteLine(_methodProfilingResults.ToString());
    }
  }
}
