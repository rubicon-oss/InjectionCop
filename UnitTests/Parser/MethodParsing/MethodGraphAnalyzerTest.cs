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
using InjectionCop.Config;
using InjectionCop.IntegrationTests.Parser;
using InjectionCop.Parser;
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Parser.MethodParsing;
using InjectionCop.Parser.TypeParsing;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;
using Rhino.Mocks;

namespace InjectionCop.UnitTests.Parser.MethodParsing
{
  [TestFixture]
  public class MethodGraphAnalyzerTest
  {
    private SymbolTable _methodPreConditions;
    private MethodGraphAnalyzer _methodGraphAnalyzer;
    private IBlacklistManager _blacklistManager;
    private MockRepository _mocks;
    private IMethodGraph _methodGraph;
    private IMethodGraphBuilder _methodGraphBuilder;
    private IParameterSymbolTableBuilder _parameterSymbolTableBuilder;
    
    [SetUp]
    public void SetUp ()
    {
      _blacklistManager = new IDbCommandBlacklistManagerStub();
      _methodPreConditions = new SymbolTable (_blacklistManager);
      _methodPreConditions.MakeSafe ("x", "SqlFragment");
      _methodPreConditions.MakeUnsafe ("y");
      TypeParser typeParser = new TypeParser ();
      _methodGraphAnalyzer = new MethodGraphAnalyzer (typeParser);
      _mocks = new MockRepository();
      _methodGraph = _mocks.Stub<IMethodGraph>();
      _methodGraphBuilder = _mocks.Stub<IMethodGraphBuilder>();
      _parameterSymbolTableBuilder = _mocks.Stub<IParameterSymbolTableBuilder>();
      
    }

    private ProblemCollection ParseGraph ()
    {
      return _methodGraphAnalyzer.Parse (_methodGraphBuilder, _parameterSymbolTableBuilder);
    }

    [Test]
    public void Parse_EmptyGraph_NoProblems ()
    {
      using(_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (true);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }

      ProblemCollection result = ParseGraph();
      Assert.That (result.Count, Is.EqualTo (0));
    }

    
    [Test]
    public void Parse_SingleNodeNoPrecondition_NoProblems()
    {
      PreCondition[] preConditions = (new List<PreCondition>()).ToArray();
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      int[] successors = (new List<int>()).ToArray();
      BasicBlock node = new BasicBlock (0, preConditions, postConditions, successors);
      
      using(_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For(_methodGraph.InitialBlock)
          .Return(node);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (result.Count, Is.EqualTo (0));
    }
  
    [Test]
    public void Parse_SingleNodePreconditionViolated_ReturnsProblem()
    {
      PreCondition[] preConditions = { new PreCondition("y", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      int[] successors = (new List<int>()).ToArray();
      BasicBlock node = new BasicBlock (0, preConditions, postConditions, successors);
      
      using(_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For(_methodGraph.InitialBlock)
          .Return(node);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID("IC_SQLi", result), Is.True);
    }
    
    [Test]
    public void Parse_SingleNodePreconditionNotViolated_NoProblem ()
    {
      PreCondition[] preConditions = { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      int[] successors = (new List<int>()).ToArray();
      BasicBlock node = new BasicBlock (0, preConditions, postConditions, successors);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (node);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }
    
    [Test]
    public void Parse_SingleNodeNewPrecondition_ReturnsProblem ()
    {
      PreCondition[] preConditions = { new PreCondition("z", "SqlFragment")  };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      int[] successors = (new List<int>()).ToArray();
      BasicBlock node = new BasicBlock (0, preConditions, postConditions, successors);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (node);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }
    
    [Test]
    public void Parse_SequencePreconditionsNotViolated_NoProblem ()
    {
      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock terminatingNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (terminatingNode);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }
    
    [Test]
    public void Parse_SequencePreconditionsViolated_ReturnsProblem ()
    {
      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragmen") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("y","SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock terminatingNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (terminatingNode);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }
    
    [Test]
    public void Parse_SequencePreconditionViolatedInPrecedingBlock_ReturnsProblem ()
    {
      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock terminatingNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (terminatingNode);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }
    
    [Test]
    public void Parse_SequenceNewPrecondition_ReturnsProblem ()
    {
      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("z", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock terminatingNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (terminatingNode);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }
    
    [Test]
    public void Parse_BranchPreconditionsNotViolated_NoProblem ()
    {
      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        _methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }
    
    [Test]
    public void Parse_BranchPreconditionsOneBranchViolated_ReturnsProblem ()
    {
      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("y", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        _methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result) && result.Count == 1, Is.True);
    }

    [Test]
    public void Parse_BranchPreconditionsTwoBranchesViolated_ReturnsProblem ()
    {
      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("y", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        _methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result) && result.Count == 2, Is.True);
    }

    
    [Test]
    public void Parse_SinkPreconditionsNotViolated_NoProblem ()
    {
      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 3 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 3 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock sink = new BasicBlock (3, preConditions.ToArray(), postConditions, successors.ToArray());

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        _methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);

        _methodGraph.GetBasicBlockById (3);
        LastCall.Return (sink);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    
    [Test]
    public void Parse_SinkPreconditionsViolated_ReturnsProblem ()
    {
      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 3 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 3 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("y", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock sink = new BasicBlock (3, preConditions.ToArray(), postConditions, successors.ToArray());

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        _methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);

        _methodGraph.GetBasicBlockById (3);
        LastCall.Return (sink);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }
    
    [Test]
    public void Parse_SinkPreconditionsViolatedInBranch_ReturnsProblem ()
    {
      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 3 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      successors = new List<int> { 3 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock sink = new BasicBlock (3, preConditions.ToArray(), postConditions, successors.ToArray());

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        _methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);

        _methodGraph.GetBasicBlockById (3);
        LastCall.Return (sink);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result) && result.Count == 1, Is.True);
    }

    [Test]
    public void Parse_SinkPreconditionsViolatedInOtherBranch_ReturnsProblem ()
    {
      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      successors = new List<int> { 3 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 3 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock sink = new BasicBlock (3, preConditions.ToArray(), postConditions, successors.ToArray());

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        _methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);

        _methodGraph.GetBasicBlockById (3);
        LastCall.Return (sink);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result) && result.Count == 1, Is.True);
    }

    
    [Test]
    public void Parse_SinkPreconditionsViolatedInTwoBranches_ReturnsProblem ()
    {
      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      successors = new List<int> { 3 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      successors = new List<int> { 3 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock sink = new BasicBlock (3, preConditions.ToArray(), postConditions, successors.ToArray());

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        _methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);

        _methodGraph.GetBasicBlockById (3);
        LastCall.Return (sink);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }
    
    [Test]
    public void Parse_LoopPreconditionsNotViolated_NoProblem ()
    {
      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 0 };
      BasicBlock firstNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For (_methodGraph.GetBasicBlockById (1))
                   .Return (firstNode);

        SetupResult.For (_methodGraph.GetBasicBlockById (0))
                   .Return (initialNode);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    public void Parse_LoopPreconditionsViolated_ReturnsProblem ()
    {
      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      successors = new List<int> { 0 };
      BasicBlock firstNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For (_methodGraph.GetBasicBlockById (1))
                   .Return (firstNode);

        SetupResult.For (_methodGraph.GetBasicBlockById (0))
                   .Return (initialNode);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }
    
    [Test]
    public void Parse_LoopPreconditionsViolatedInBranch_ReturnsProblem ()
    {
      _methodPreConditions.MakeSafe ("z", "SqlFragment");

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment"), new PreCondition("z", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 0 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("z");
      successors = new List<int> { 0 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());
      
      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For (_methodGraph.GetBasicBlockById (0))
                   .Return (initialNode);

        SetupResult.For (_methodGraph.GetBasicBlockById (1))
                   .Return (firstBranch);

        SetupResult.For (_methodGraph.GetBasicBlockById (2))
                   .Return (secondBranch);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result) && result.Count == 1, Is.True);
      
    }
    
  }
}