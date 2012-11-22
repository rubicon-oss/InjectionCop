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
using InjectionCop.Parser._Block;
using InjectionCop.Parser._Method;
using InjectionCop.Parser._Type;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;
using Rhino.Mocks;

namespace InjectionCop.UnitTests.Parser._Method
{
  [TestFixture]
  public class MethodParserTest
  {
    private SymbolTable _methodPreConditions;
    private MethodParser _methodParser;
    private IBlackTypes _blackTypes;
    
    [SetUp]
    public void SetUp ()
    {
      _blackTypes = new IDbCommandBlackTypesStub();
      _methodPreConditions = new SymbolTable (_blackTypes);
      _methodPreConditions.SetSafeness ("x", "SqlFragment", true);
      _methodPreConditions.SetSafeness("y", "SqlFragment", false);
      TypeParser typeParser = new TypeParser ();
      _methodParser = new MethodParser (typeParser);
    }

    [Test]
    public void Parse_EmptyGraph_NoProblems ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();
      using(mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (true);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (result.Count, Is.EqualTo (0));
    }

    [Test]
    public void Parse_SingleNodeNoPrecondition_NoProblems()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();
      
      PreCondition[] preConditions = (new List<PreCondition>()).ToArray();
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      int[] successors = (new List<int>()).ToArray();
      BasicBlock node = new BasicBlock (0, preConditions, postConditions, successors);
      
      using(mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For(methodGraph.InitialBlock)
          .Return(node);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (result.Count, Is.EqualTo (0));
    }

    [Test]
    public void Parse_SingleNodePreconditionViolated_ReturnsProblem()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();
      
      PreCondition[] preConditions = { new PreCondition("y", "SqlFragment"),  };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      int[] successors = (new List<int>()).ToArray();
      BasicBlock node = new BasicBlock (0, preConditions, postConditions, successors);
      
      using(mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For(methodGraph.InitialBlock)
          .Return(node);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID("IC_SQLi", result), Is.True);
    }

    [Test]
    public void Parse_SingleNodePreconditionNotViolated_NoProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      PreCondition[] preConditions = { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      int[] successors = (new List<int>()).ToArray();
      BasicBlock node = new BasicBlock (0, preConditions, postConditions, successors);

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (node);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    public void Parse_SingleNodeNewPrecondition_ReturnsProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      PreCondition[] preConditions = { new PreCondition("z", "SqlFragment")  };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      int[] successors = (new List<int>()).ToArray();
      BasicBlock node = new BasicBlock (0, preConditions, postConditions, successors);

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (node);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    public void Parse_SequencePreconditionsNotViolated_NoProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int>();
      BasicBlock terminatingNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (initialNode);

        methodGraph.GetBasicBlockById (1);
        LastCall.Return (terminatingNode);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    public void Parse_SequencePreconditionsViolated_ReturnsProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragmen") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("y","SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int>();
      BasicBlock terminatingNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (initialNode);

        methodGraph.GetBasicBlockById (1);
        LastCall.Return (terminatingNode);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    public void Parse_SequencePreconditionViolatedInPrecedingBlock_ReturnsProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      postConditions.SetSafeness("x", "SqlFragment", false);
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int>();
      BasicBlock terminatingNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (initialNode);

        methodGraph.GetBasicBlockById (1);
        LastCall.Return (terminatingNode);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    public void Parse_SequenceNewPrecondition_ReturnsProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      postConditions.SetSafeness ("x", "SqlFragment", false);
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("z", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int>();
      BasicBlock terminatingNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (initialNode);

        methodGraph.GetBasicBlockById (1);
        LastCall.Return (terminatingNode);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    public void Parse_BranchPreconditionsNotViolated_NoProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int>();
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int>();
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (initialNode);

        methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    public void Parse_BranchPreconditionsOneBranchViolated_ReturnsProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int>();
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("y", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int>();
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (initialNode);

        methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result) && result.Count == 1, Is.True);
    }

    [Test]
    public void Parse_BranchPreconditionsTwoBranchesViolated_ReturnsProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      postConditions.SetSafeness("x", "SqlFragment", false);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int>();
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("y", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int>();
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (initialNode);

        methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result) && result.Count == 2, Is.True);
    }

    [Test]
    public void Parse_SinkPreconditionsNotViolated_NoProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int> { 3 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int> { 3 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int>();
      BasicBlock sink = new BasicBlock (3, preConditions.ToArray(), postConditions, successors.ToArray());

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (initialNode);

        methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);

        methodGraph.GetBasicBlockById (3);
        LastCall.Return (sink);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    public void Parse_SinkPreconditionsViolated_ReturnsProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int> { 3 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int> { 3 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("y", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int>();
      BasicBlock sink = new BasicBlock (3, preConditions.ToArray(), postConditions, successors.ToArray());

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (initialNode);

        methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);

        methodGraph.GetBasicBlockById (3);
        LastCall.Return (sink);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    public void Parse_SinkPreconditionsViolatedInBranch_ReturnsProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int> { 3 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      postConditions.SetSafeness("x", "SqlFragment", false);
      successors = new List<int> { 3 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int>();
      BasicBlock sink = new BasicBlock (3, preConditions.ToArray(), postConditions, successors.ToArray());

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (initialNode);

        methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);

        methodGraph.GetBasicBlockById (3);
        LastCall.Return (sink);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result) && result.Count == 1, Is.True);
    }

    [Test]
    public void Parse_SinkPreconditionsViolatedInOtherBranch_ReturnsProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      postConditions.SetSafeness ("x", "SqlFragment", false);
      successors = new List<int> { 3 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int> { 3 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int>();
      BasicBlock sink = new BasicBlock (3, preConditions.ToArray(), postConditions, successors.ToArray());

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (initialNode);

        methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);

        methodGraph.GetBasicBlockById (3);
        LastCall.Return (sink);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result) && result.Count == 1, Is.True);
    }

    [Test]
    public void Parse_SinkPreconditionsViolatedInTwoBranches_ReturnsProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      postConditions.SetSafeness("x", "SqlFragment", false);
      successors = new List<int> { 3 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      postConditions.SetSafeness("x", "SqlFragment", false);
      successors = new List<int> { 3 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int>();
      BasicBlock sink = new BasicBlock (3, preConditions.ToArray(), postConditions, successors.ToArray());

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (initialNode);

        methodGraph.GetBasicBlockById (1);
        LastCall.Return (firstBranch);

        methodGraph.GetBasicBlockById (2);
        LastCall.Return (secondBranch);

        methodGraph.GetBasicBlockById (3);
        LastCall.Return (sink);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    public void Parse_LoopPreconditionsNotViolated_NoProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int> { 0 };
      BasicBlock firstNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For (methodGraph.GetBasicBlockById (1))
                   .Return (firstNode);

        SetupResult.For (methodGraph.GetBasicBlockById (0))
                   .Return (initialNode);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.False);
    }

    [Test]
    public void Parse_LoopPreconditionsViolated_NoProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      List<int> successors = new List<int> { 1 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      postConditions.SetSafeness("x", "SqlFragment", false);
      successors = new List<int> { 0 };
      BasicBlock firstNode = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For (methodGraph.GetBasicBlockById (1))
                   .Return (firstNode);

        SetupResult.For (methodGraph.GetBasicBlockById (0))
                   .Return (initialNode);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result), Is.True);
    }

    [Test]
    public void Parse_LoopPreconditionsViolatedInBranch_ReturnsProblem ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();
      _methodPreConditions.SetSafeness("z", "SqlFragment", true);

      List<PreCondition> preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment"), new PreCondition("z", "SqlFragment") };
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      List<int> successors = new List<int> { 1, 2 };
      BasicBlock initialNode = new BasicBlock (0, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      successors = new List<int> { 0 };
      BasicBlock firstBranch = new BasicBlock (1, preConditions.ToArray(), postConditions, successors.ToArray());

      preConditions = new List<PreCondition> { new PreCondition("x", "SqlFragment") };
      postConditions = new SymbolTable (_blackTypes);
      postConditions.SetSafeness("z", "SqlFragment", false);
      successors = new List<int> { 0 };
      BasicBlock secondBranch = new BasicBlock (2, preConditions.ToArray(), postConditions, successors.ToArray());
      
      using (mocks.Record())
      {
        methodGraph.IsEmpty();
        LastCall.Return (false);

        SetupResult.For (methodGraph.InitialBlock)
                   .Return (initialNode);

        SetupResult.For (methodGraph.GetBasicBlockById (0))
                   .Return (initialNode);

        SetupResult.For (methodGraph.GetBasicBlockById (1))
                   .Return (firstBranch);

        SetupResult.For (methodGraph.GetBasicBlockById (2))
                   .Return (secondBranch);

      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _methodPreConditions);
      Assert.That (TestHelper.ContainsProblemID ("IC_SQLi", result) && result.Count == 1, Is.True);
      // how many entries??
    
    }
  }
}
