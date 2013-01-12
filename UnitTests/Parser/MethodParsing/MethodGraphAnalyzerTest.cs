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
using System.Collections.Generic;
using InjectionCop.Config;
using InjectionCop.IntegrationTests.Parser;
using InjectionCop.Parser;
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Parser.MethodParsing;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;
using Rhino.Mocks;

namespace InjectionCop.UnitTests.Parser.MethodParsing
{
  [TestFixture]
  public class MethodGraphAnalyzerTest
  {
    private SymbolTable _methodPreConditions;
    private ProblemPipeStub _problemPipeStub;
    private MethodGraphAnalyzer _methodGraphAnalyzer;
    private IBlacklistManager _blacklistManager;
    private MockRepository _mocks;
    private IMethodGraph _methodGraph;
    private IMethodGraphBuilder _methodGraphBuilder;
    private IInitialSymbolTableBuilder _parameterSymbolTableBuilder;

    private const string c_InjectionCopRuleId = "IC0001";
    private readonly BlockAssignment[] c_EmptyAssignments = new BlockAssignment[] { };

    [SetUp]
    public void SetUp ()
    {
      _blacklistManager = new IDbCommandBlacklistManagerStub();
      _methodPreConditions = new SymbolTable (_blacklistManager);
      _methodPreConditions.MakeSafe ("x", "SqlFragment");
      _methodPreConditions.MakeSafe ("l", SymbolTable.LITERAL);
      _methodPreConditions.MakeUnsafe ("y");
      _problemPipeStub = new ProblemPipeStub();
      _methodGraphAnalyzer = new MethodGraphAnalyzer (_problemPipeStub);
      _mocks = new MockRepository();
      _methodGraph = _mocks.Stub<IMethodGraph>();
      _methodGraphBuilder = _mocks.Stub<IMethodGraphBuilder>();
      _parameterSymbolTableBuilder = _mocks.Stub<IInitialSymbolTableBuilder>();
    }

    private ProblemCollection ParseGraph ()
    {
      _methodGraphAnalyzer.Parse (_methodGraphBuilder, _parameterSymbolTableBuilder);
      ProblemCollection problemCollection = new ProblemCollection();
      _problemPipeStub.Problems.ForEach (problem => problemCollection.Add (new Problem (new Resolution("resolution"), c_InjectionCopRuleId)));
      return problemCollection;
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
      AssignabilityPreCondition[] preConditions = (new List<AssignabilityPreCondition>()).ToArray();
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      int[] successors = (new List<int>()).ToArray();
      BasicBlock node = new BasicBlock (0, preConditions, postConditions, successors, c_EmptyAssignments);
      
      using(_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For(_methodGraph.InitialBlock)
          .Return(node);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { node });

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
      AssignabilityPreCondition[] preConditions = { new AssignabilityPreCondition("y", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      int[] successors = (new List<int>()).ToArray();

      BasicBlock node = new BasicBlock (0, preConditions, postConditions, successors, c_EmptyAssignments);
      
      using(_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For(_methodGraph.InitialBlock)
          .Return(node);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { node });

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID(c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SingleNodePreconditionNotViolated_NoProblem ()
    {
      AssignabilityPreCondition[] preConditions = { new AssignabilityPreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      int[] successors = (new List<int>()).ToArray();
      BasicBlock node = new BasicBlock (0, preConditions, postConditions, successors, c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (node);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { node });

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_SingleNodePreconditionNotViolatedBecauseOfLiteral_NoProblem ()
    {
      AssignabilityPreCondition[] preConditions = { new AssignabilityPreCondition("l", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      int[] successors = (new List<int>()).ToArray();
      BasicBlock node = new BasicBlock (0, preConditions, postConditions, successors, c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (node);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { node });

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_SingleNodeNewPrecondition_ReturnsProblem ()
    {
      AssignabilityPreCondition[] preConditions = { new AssignabilityPreCondition("z", "SqlFragment")  };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      int[] successors = (new List<int>()).ToArray();
      BasicBlock node = new BasicBlock (0, preConditions, postConditions, successors, c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (node);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { node });

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SequencePreconditionsNotViolated_NoProblem ()
    {
      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock terminatingNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { initialNode, terminatingNode });

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (terminatingNode);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_SequencePreconditionsViolated_ReturnsProblem ()
    {
      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragmen") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("y","SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock terminatingNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { initialNode, terminatingNode });

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (terminatingNode);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SequencePreconditionViolatedInPrecedingBlock_ReturnsProblem ()
    {
      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock terminatingNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { initialNode, terminatingNode });

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (terminatingNode);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SequenceNewPrecondition_ReturnsProblem ()
    {
      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("z", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock terminatingNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { initialNode, terminatingNode });

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (terminatingNode);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_BranchPreconditionsNotViolated_NoProblem ()
    {
      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For (_methodGraph.Blocks)
                    .Return(new BasicBlock[] {initialNode, firstBranch, secondBranch});

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
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
    [Test]
    public void Parse_BranchPreconditionsOneBranchViolated_ReturnsProblem ()
    {
      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("y", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { initialNode, firstBranch, secondBranch });

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
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result) && result.Count == 1, Is.True);
    }

    [Test]
    public void Parse_BranchPreconditionsTwoBranchesViolated_ReturnsProblem ()
    {
      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("y", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { initialNode, firstBranch, secondBranch });

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
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result) && result.Count == 2, Is.True);
    }

    
    [Test]
    public void Parse_SinkPreconditionsNotViolated_NoProblem ()
    {
      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 3 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 3 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock sink = new BasicBlock (3, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { initialNode, firstBranch, secondBranch, sink });

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
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    
    [Test]
    public void Parse_SinkPreconditionsViolated_ReturnsProblem ()
    {
      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 3 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 3 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("y", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock sink = new BasicBlock (3, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { initialNode, firstBranch, secondBranch, sink });

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
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_SinkPreconditionsViolatedInBranch_ReturnsProblem ()
    {
      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 3 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      successors = new List<int> { 3 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock sink = new BasicBlock (3, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { initialNode, firstBranch, secondBranch, sink });

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
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result) && result.Count == 1, Is.True);
    }

    [Test]
    public void Parse_SinkPreconditionsViolatedInOtherBranch_ReturnsProblem ()
    {
      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      successors = new List<int> { 3 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 3 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock sink = new BasicBlock (3, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { initialNode, firstBranch, secondBranch, sink });

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
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result) && result.Count == 1, Is.True);
    }

    
    [Test]
    public void Parse_SinkPreconditionsViolatedInTwoBranches_ReturnsProblem ()
    {
      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      successors = new List<int> { 3 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      successors = new List<int> { 3 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock sink = new BasicBlock (3, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { initialNode, firstBranch, secondBranch, sink });

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
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_LoopPreconditionsNotViolated_NoProblem ()
    {
      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 0 };
      BasicBlock firstNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { initialNode, firstNode });

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
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }

    [Test]
    public void Parse_LoopPreconditionsViolated_ReturnsProblem ()
    {
      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("x");
      successors = new List<int> { 0 };
      BasicBlock firstNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { initialNode, firstNode });

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
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.True);
    }
    
    [Test]
    public void Parse_LoopPreconditionsViolatedInBranch_ReturnsProblem ()
    {
      _methodPreConditions.MakeSafe ("z", "SqlFragment");

      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment"), new AssignabilityPreCondition("z", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int> { 0 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      postConditions.MakeUnsafe("z");
      successors = new List<int> { 0 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);
      
      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { initialNode, firstBranch, secondBranch });

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
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result) && result.Count == 1, Is.True);
      
    }

    [Test]
    public void Parse_SequenceWithLocalAssignment_NoProblem ()
    {
      List<AssignabilityPreCondition> preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blacklistManager);
      List<int> successors = new List<int> { 1 };
      List<BlockAssignment> localAssignments = new List<BlockAssignment> { new BlockAssignment ("x", "y") };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray(), localAssignments.ToArray());

      preConditions = new List<AssignabilityPreCondition> { new AssignabilityPreCondition("x", "SqlFragment"), new AssignabilityPreCondition("y", "SqlFragment") };
      postConditions = new SymbolTable (_blacklistManager);
      successors = new List<int>();
      BasicBlock terminatingNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray(), c_EmptyAssignments);

      using (_mocks.Record())
      {
        _methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (_methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For(_methodGraph.Blocks)
                    .Return(new BasicBlock[] { initialNode, terminatingNode });

        _methodGraph.GetBasicBlockById (1);
        LastCall.Return (terminatingNode);

        _methodGraphBuilder.GetResult();
        LastCall.Return (_methodGraph);
        _parameterSymbolTableBuilder.GetResult();
        LastCall.Return(_methodPreConditions);
      }
      ProblemCollection result = ParseGraph();
      Assert.That (TestHelper.ContainsProblemID (c_InjectionCopRuleId, result), Is.False);
    }
    
  }
}
