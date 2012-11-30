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
using System.Linq;
using InjectionCop.Config;
using InjectionCop.Parser;
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Parser.MethodParsing;
using InjectionCop.Parser.TypeParsing;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.MethodParsing
{
  [TestFixture]
  public class MethodGraphTest
  {
    private IBlacklistManager _blacklistManager;

    [SetUp]
    public void SetUp()
    {
      _blacklistManager = new IDbCommandBlacklistManagerStub();
    }

    private IMethodGraph BuildMethodGraph (Block body)
    {
      IMethodGraphBuilder methodGraphBuilder = new MethodGraphBuilder (body, _blacklistManager, new TypeParser());
      methodGraphBuilder.Build();
      IMethodGraph methodGraph = methodGraphBuilder.GetResult();
      return methodGraph;
    }

    [Test]
    public void GetInitialBlockId_DeclarationWithReturn_ReturnsIdOfInitialBlock ()
    {
      Method sampleMethod = TestHelper.GetSample<MethodGraphSample> ("DeclarationWithReturn");
      Block sample = sampleMethod.Body;
      Block initialBlock = sampleMethod.Body.Statements[0] as Block;
      if (initialBlock != null)
      {
        IMethodGraph methodGraph = BuildMethodGraph(sample);
        Assert.That (methodGraph.InitialBlock.Id, Is.EqualTo (initialBlock.UniqueKey));
      }
    }

    [Test]
    public void GetBlockById_FirstBlockIdOfDeclarationWithReturnSample_ReturnsInitialBasicBlock ()
    {
      Method sampleMethod = TestHelper.GetSample<MethodGraphSample> ("DeclarationWithReturn");
      Block sample = sampleMethod.Body;
      Block initialBlock = sampleMethod.Body.Statements[0] as Block;
      if(initialBlock != null)
      {
        IMethodGraph methodGraph = BuildMethodGraph(sample);
        BasicBlock initialBasicBlock = methodGraph.GetBasicBlockById (initialBlock.UniqueKey);
        
        Assert.That (initialBasicBlock.Id, Is.EqualTo(initialBlock.UniqueKey));
      }
      else
      {
        Assert.Ignore ("Bad Sample");
      }
    }

    [Test]
    public void GetBlockById_SuccessorOfFirstBlockIdOfDeclarationWithReturnSample_ReturnsReturnBasicBlock ()
    {
      Method sampleMethod = TestHelper.GetSample<MethodGraphSample> ("DeclarationWithReturn");
      Block sample = sampleMethod.Body;
      Block initialBlock = sampleMethod.Body.Statements[0] as Block;
      Block returnBlock = sampleMethod.Body.Statements[1] as Block;
      if(initialBlock != null && returnBlock != null)
      {
        IMethodGraph methodGraph = BuildMethodGraph(sample);
        int returnBlockId = returnBlock.UniqueKey;
        BasicBlock returnBasicBlock = methodGraph.GetBasicBlockById (returnBlockId);
        
        Assert.That (returnBasicBlock.Id, Is.EqualTo(returnBlock.UniqueKey));
      }
      else
      {
        Assert.Ignore ("Bad Sample");
      }
    }

    [Test]
    [ExpectedException( typeof(InjectionCopException), ExpectedMessage = "The given key was not present in the MethodGraph")]
    public void GetBlockById_InvalidId_ThrowsException ()
    {
      Method sampleMethod = TestHelper.GetSample<MethodGraphSample> ("DeclarationWithReturn");
      Block sample = sampleMethod.Body;
      IMethodGraph methodGraph = BuildMethodGraph(sample);
      methodGraph.GetBasicBlockById (-1);
    }

    [Test]
    public void MethodGraph_DeclarationWithReturn_ReturnsCorrectInitialBlockSuccessors()
    {
      Method sampleMethod = TestHelper.GetSample<MethodGraphSample> ("DeclarationWithReturn");
      Block sample = sampleMethod.Body;
      Block initialBlock = sampleMethod.Body.Statements[0] as Block;
      Block returnBlock = sampleMethod.Body.Statements[1] as Block;
      if(initialBlock != null && returnBlock != null)
      {
        IMethodGraph methodGraph = BuildMethodGraph(sample);
        BasicBlock initialBasicBlock = methodGraph.GetBasicBlockById (initialBlock.UniqueKey);
        bool initialBasicBlockConnectedWithReturn = initialBasicBlock.SuccessorKeys.Any (key => key == returnBlock.UniqueKey);

        Assert.That (initialBasicBlockConnectedWithReturn, Is.True);
      }
      else
      {
        Assert.Ignore ("Bad Sample");
      }
    }

    [Test]
    public void MethodGraph_DeclarationWithReturn_ReturnBlockHasNoSuccessors()
    {
      Method sampleMethod = TestHelper.GetSample<MethodGraphSample> ("DeclarationWithReturn");
      Block sample = sampleMethod.Body;
      Block returnBlock = sampleMethod.Body.Statements[1] as Block;
      if(returnBlock != null)
      {
        IMethodGraph methodGraph = BuildMethodGraph(sample);
        BasicBlock returnBasicBlock = methodGraph.GetBasicBlockById (returnBlock.UniqueKey);
        
        Assert.That (returnBasicBlock.SuccessorKeys.Length, Is.EqualTo(0));
      }
      else
      {
        Assert.Ignore ("Bad Sample");
      }
    }

    [Test]
    public void MethodGraph_IfStatementTrueBlockOnly_ReturnsCorrectConditionSuccessors ()
    {
      TypeNode stringTypeNode = IntrospectionTools.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<MethodGraphSample> ("IfStatementTrueBlockOnly", stringTypeNode);
      Block sample = sampleMethod.Body;
      Block conditionBlock = sampleMethod.Body.Statements[0] as Block;
      Block trueBlock = sampleMethod.Body.Statements[1] as Block;
      Block preReturnBlock = sampleMethod.Body.Statements[2] as Block;
      if(conditionBlock != null /*&& returnBlock != null*/ && preReturnBlock != null && trueBlock != null)
      {
        IMethodGraph methodGraph = BuildMethodGraph(sample);
        BasicBlock conditionBasicBlock = methodGraph.GetBasicBlockById (conditionBlock.UniqueKey);
        
        bool conditionBasicBlockSuccessorsOk = conditionBasicBlock.SuccessorKeys.Any (key => key == trueBlock.UniqueKey)
                                               && conditionBasicBlock.SuccessorKeys.Any (key => key == preReturnBlock.UniqueKey);
        
        Assert.That (conditionBasicBlockSuccessorsOk, Is.True);
      }
      else
      {
        Assert.Ignore ("Bad Sample");
      }
    }

    [Test]
    public void MethodGraph_IfStatementTrueBlockOnly_ReturnsCorrectTrueBlockSuccessors ()
    {
      TypeNode stringTypeNode = IntrospectionTools.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<MethodGraphSample> ("IfStatementTrueBlockOnly", stringTypeNode);
      Block sample = sampleMethod.Body;
      Block trueBlock = sampleMethod.Body.Statements[1] as Block;
      Block preReturnBlock = sampleMethod.Body.Statements[2] as Block;
      if(preReturnBlock != null && trueBlock != null)
      {
        IMethodGraph methodGraph = BuildMethodGraph(sample);
        BasicBlock trueBasicBlock = methodGraph.GetBasicBlockById (trueBlock.UniqueKey);
        bool trueBasicBlockSuccessorsOk = trueBasicBlock.SuccessorKeys.Any (key => key == preReturnBlock.UniqueKey);

        Assert.That (trueBasicBlockSuccessorsOk, Is.True);
      }
      else
      {
        Assert.Ignore ("Bad Sample");
      }
    }

    [Test]
    public void MethodGraph_IfStatementTrueBlockOnly_ReturnsCorrectPreReturnSuccessors ()
    {
      TypeNode stringTypeNode = IntrospectionTools.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<MethodGraphSample> ("IfStatementTrueBlockOnly", stringTypeNode);
      Block sample = sampleMethod.Body;
      Block preReturnBlock = sampleMethod.Body.Statements[2] as Block;
      Block returnBlock = sampleMethod.Body.Statements[3] as Block;
      if(returnBlock != null && preReturnBlock != null)
      {
        IMethodGraph methodGraph = BuildMethodGraph(sample);
        BasicBlock preReturnBasicBlock = methodGraph.GetBasicBlockById (preReturnBlock.UniqueKey);
        bool preReturnBasicBlockSuccessorsOk = preReturnBasicBlock.SuccessorKeys.Any (key => key == returnBlock.UniqueKey);

        Assert.That (preReturnBasicBlockSuccessorsOk, Is.True);
      }
      else
      {
        Assert.Ignore ("Bad Sample");
      }
    }

    [Test]
    public void MethodGraph_IfStatementTrueBlockOnly_ReturnsCorrectReturnSuccessors ()
    {
      TypeNode stringTypeNode = IntrospectionTools.TypeNodeFactory<string>();
      Method sampleMethod = TestHelper.GetSample<MethodGraphSample> ("IfStatementTrueBlockOnly", stringTypeNode);
      Block sample = sampleMethod.Body;
      Block returnBlock = sampleMethod.Body.Statements[3] as Block;
      if(returnBlock != null)
      {
        IMethodGraph methodGraph = BuildMethodGraph(sample);
        BasicBlock preReturnBasicBlock = methodGraph.GetBasicBlockById (returnBlock.UniqueKey);
        
        Assert.That (preReturnBasicBlock.SuccessorKeys.Length, Is.EqualTo(0));
      }
      else
      {
        Assert.Ignore ("Bad Sample");
      }
    }

    [Test]
    public void MethodGraph_ForLoop_ReturnsCorrectPreForSuccessors ()
    {
      Method sampleMethod = TestHelper.GetSample<MethodGraphSample> ("ForLoop");
      Block sample = sampleMethod.Body;
      Block preForBlock = sampleMethod.Body.Statements[0] as Block;
      Block conditionBlock = sampleMethod.Body.Statements[2] as Block;
      if(preForBlock != null && conditionBlock != null)
      {
        IMethodGraph methodGraph = BuildMethodGraph(sample);
        BasicBlock preForBasicBlock = methodGraph.GetBasicBlockById (preForBlock.UniqueKey);
        bool preForBasicBlockSuccessorsCorrect = preForBasicBlock.SuccessorKeys.Length == 1
                                                 && preForBasicBlock.SuccessorKeys[0] == conditionBlock.UniqueKey;

        Assert.That (preForBasicBlockSuccessorsCorrect, Is.True);
      }
      else
      {
        Assert.Ignore ("Bad Sample");
      }
    }

    [Test]
    public void MethodGraph_ForLoop_ReturnsCorrectInnerForSuccessors ()
    {
      Method sampleMethod = TestHelper.GetSample<MethodGraphSample> ("ForLoop");
      Block sample = sampleMethod.Body;
      Block innerForBlock = sampleMethod.Body.Statements[1] as Block;
      Block conditionBlock = sampleMethod.Body.Statements[2] as Block;
      if (innerForBlock != null && conditionBlock != null)
      {
        IMethodGraph methodGraph = BuildMethodGraph(sample);
        BasicBlock innerForBasicBlock = methodGraph.GetBasicBlockById (innerForBlock.UniqueKey);
        bool innerForBasicBlockSuccessorsCorrect = innerForBasicBlock.SuccessorKeys.Length == 1
                                                   && innerForBasicBlock.SuccessorKeys[0] == conditionBlock.UniqueKey;

        Assert.That (innerForBasicBlockSuccessorsCorrect, Is.True);
      }
      else
      {
        Assert.Ignore ("Bad Sample");
      }
    }

    [Test]
    public void MethodGraph_ForLoop_ReturnsCorrectConditionSuccessors ()
    {
      Method sampleMethod = TestHelper.GetSample<MethodGraphSample> ("ForLoop");
      Block sample = sampleMethod.Body;
      Block innerForBlock = sampleMethod.Body.Statements[1] as Block;
      Block conditionBlock = sampleMethod.Body.Statements[2] as Block;
      Block preReturnBlock = sampleMethod.Body.Statements[3] as Block;
      if(innerForBlock != null && conditionBlock != null && preReturnBlock != null)
      {
        IMethodGraph methodGraph = BuildMethodGraph(sample);
        BasicBlock conditionBasicBlock = methodGraph.GetBasicBlockById (conditionBlock.UniqueKey);
        bool conditionBasicBlockSuccessorsCorrect =
            conditionBasicBlock.SuccessorKeys.Length == 2
            && conditionBasicBlock.SuccessorKeys.Any (key => key == innerForBlock.UniqueKey)
            && conditionBasicBlock.SuccessorKeys.Any (key => key == preReturnBlock.UniqueKey);

        Assert.That (conditionBasicBlockSuccessorsCorrect, Is.True);
      }
      else
      {
        Assert.Ignore ("Bad Sample");
      }
    }

    [Test]
    public void MethodGraph_ForLoop_ReturnsCorrectPreReturnSuccessors ()
    {
      Method sampleMethod = TestHelper.GetSample<MethodGraphSample> ("ForLoop");
      Block sample = sampleMethod.Body;
      Block preReturnBlock = sampleMethod.Body.Statements[3] as Block;
      Block returnBlock = sampleMethod.Body.Statements[4] as Block;
      if (preReturnBlock != null && returnBlock != null)
      {
        IMethodGraph methodGraph = BuildMethodGraph(sample);
        BasicBlock preReturnBasicBlock = methodGraph.GetBasicBlockById (preReturnBlock.UniqueKey);
        bool preReturnBasicBlockSuccessorsCorrect = preReturnBasicBlock.SuccessorKeys.Length == 1
                                                   && preReturnBasicBlock.SuccessorKeys[0] == returnBlock.UniqueKey;

        Assert.That (preReturnBasicBlockSuccessorsCorrect, Is.True);
      }
      else
      {
        Assert.Ignore ("Bad Sample");
      }
    }

    [Test]
    public void MethodGraph_ForLoop_ReturnsCorrectReturnSuccessors ()
    {
      Method sampleMethod = TestHelper.GetSample<MethodGraphSample> ("ForLoop");
      Block sample = sampleMethod.Body;
      Block returnBlock = sampleMethod.Body.Statements[4] as Block;
      if (returnBlock != null)
      {
        IMethodGraph methodGraph = BuildMethodGraph(sample);
        BasicBlock returnBasicBlock = methodGraph.GetBasicBlockById (returnBlock.UniqueKey);
        
        Assert.That (returnBasicBlock.SuccessorKeys.Length, Is.EqualTo(0));
      }
      else
      {
        Assert.Ignore ("Bad Sample");
      }
    }
  }
}
