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
using InjectionCop.Config;
using InjectionCop.Parser;
using InjectionCop.Parser.BlockParsing.PreCondition;
using InjectionCop.Parser.ProblemPipe;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;
using Rhino.Mocks;

namespace InjectionCop.UnitTests.Parser.BlockParsing.PreCondition
{
  [TestFixture]
  public class CustomInferencePreConditionTest
  {
    private const string c_expectedType = "expectedType";
    private const string c_symbol = "symbolName";

    private ProblemMetadata _problemMetadata;
    private IBlacklistManager _blacklistManager;
    
    [SetUp]
    public void SetUp ()
    {
      _problemMetadata = new ProblemMetadata (0, new SourceContext(), Fragment.CreateEmpty(), Fragment.CreateEmpty());
      MockRepository mocks = new MockRepository();
      _blacklistManager = mocks.Stub<IBlacklistManager>();
    }

    [Test]
    public void IsViolated_NamedExpectedNamedGiven_ReturnsFalse ()
    {
      var expectedFragment = Fragment.CreateNamed (c_expectedType);
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      context.MakeSafe (c_symbol, expectedFragment);

      Assert.That (preCondition.IsViolated (context), Is.False);
    }

    [Test]
    public void IsViolated_NamedExpectedDifferentlyNamedgiven_ReturnsTrue ()
    {
      var expectedFragment = Fragment.CreateNamed (c_expectedType);
      var givenFragment = Fragment.CreateNamed ("unexpected");
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      context.MakeSafe (c_symbol, givenFragment);

      Assert.That (preCondition.IsViolated (context), Is.True);
    }

    [Test]
    public void IsViolated_NamedExpectedLiteralGiven_ReturnsFalse ()
    {
      var expectedFragment = Fragment.CreateNamed (c_expectedType);
      var givenFragment = Fragment.CreateLiteral();
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      context.MakeSafe (c_symbol, givenFragment);

      Assert.That (preCondition.IsViolated (context), Is.False);
    }

    [Test]
    public void IsViolated_NamedExpectedEmptyGiven_ReturnsTrue ()
    {
      var expectedFragment = Fragment.CreateNamed (c_expectedType);
      var givenFragment = Fragment.CreateEmpty();
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      context.MakeSafe (c_symbol, givenFragment);

      Assert.That (preCondition.IsViolated (context), Is.True);
    }

    [Test]
    public void IsViolated_LiteralExpectedLiteralGiven_ReturnsFalse ()
    {
      var expectedFragment = Fragment.CreateLiteral();
      var givenFragment = Fragment.CreateLiteral();
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      context.MakeSafe (c_symbol, givenFragment);

      Assert.That (preCondition.IsViolated (context), Is.False);
    }

    [Test]
    public void IsViolated_LiteralExpectedNamedFragmentGiven_ReturnsTrue ()
    {
      var expectedFragment = Fragment.CreateLiteral();
      var givenFragment = Fragment.CreateNamed ("unexpected");
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      context.MakeSafe (c_symbol, givenFragment);

      Assert.That (preCondition.IsViolated (context), Is.True);
    }

    [Test]
    public void IsViolated_LiteralExpectedEmptyGiven_ReturnsTrue ()
    {
      var expectedFragment = Fragment.CreateLiteral();
      var givenFragment = Fragment.CreateEmpty();
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      context.MakeSafe (c_symbol, givenFragment);

      Assert.That (preCondition.IsViolated (context), Is.True);
    }

    [Test]
    public void IsViolated_EmptyExpectedLiteralGiven_ReturnsFalse ()
    {
      var expectedFragment = Fragment.CreateEmpty();
      var givenFragment = Fragment.CreateLiteral();
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      context.MakeSafe (c_symbol, givenFragment);

      Assert.That (preCondition.IsViolated (context), Is.False);
    }

    [Test]
    public void IsViolated_EmptyExpectedNamedFragmentGiven_ReturnsFalse ()
    {
      var expectedFragment = Fragment.CreateEmpty();
      var givenFragment = Fragment.CreateNamed ("unexpected");
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      context.MakeSafe (c_symbol, givenFragment);

      Assert.That (preCondition.IsViolated (context), Is.False);
    }

    [Test]
    public void IsViolated_EmptyExpectedEmptyGiven_ReturnsFalse ()
    {
      var expectedFragment = Fragment.CreateEmpty();
      var givenFragment = Fragment.CreateEmpty();
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      context.MakeSafe (c_symbol, givenFragment);

      Assert.That (preCondition.IsViolated (context), Is.False);
    }

    [Test]
    public void IsViolated_NamedExpectedSymbolUnknown_ReturnsTrue ()
    {
      var expectedFragment = Fragment.CreateNamed(c_expectedType);
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      
      Assert.That (preCondition.IsViolated (context), Is.True);
    }

    [Test]
    public void IsViolated_LiteralExpectedSymbolUnknown_ReturnsTrue ()
    {
      var expectedFragment = Fragment.CreateLiteral();
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      
      Assert.That (preCondition.IsViolated (context), Is.True);
    }

    [Test]
    public void IsViolated_EmptyExpectedSymbolUnknown_ReturnsFalse ()
    {
      var expectedFragment = Fragment.CreateEmpty();
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      
      Assert.That (preCondition.IsViolated (context), Is.False);
    }
    
    [Test]
    public void HandleViolation_ProblemMetadataGiven_AddsProblem ()
    {
      var expectedFragment = Fragment.CreateNamed (c_expectedType);
      var givenFragment = Fragment.CreateNamed ("unexpected");
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      context.MakeSafe (c_symbol, givenFragment);
      IProblemPipe problemPipe = MockRepository.GenerateMock<IProblemPipe>();
      
      preCondition.HandleViolation (context, problemPipe);

      problemPipe.AssertWasCalled (pipe => pipe.AddProblem (Arg<ProblemMetadata>.Is.Equal(_problemMetadata)));
    }

    [Test]
    public void HandleViolation_NoProblemMetadataGiven_NoProblemAdded ()
    {
      var expectedFragment = Fragment.CreateNamed (c_expectedType);
      var givenFragment = Fragment.CreateNamed ("unexpected");
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment);
      var context = new SymbolTable(_blacklistManager);
      context.MakeSafe (c_symbol, givenFragment);
      IProblemPipe problemPipe = MockRepository.GenerateMock<IProblemPipe>();
      
      preCondition.HandleViolation (context, problemPipe);

      problemPipe.AssertWasNotCalled (pipe => pipe.AddProblem (Arg<ProblemMetadata>.Is.Anything));
    }

    [Test]
    public void HandleViolation_ProblemMetadataGivenButNoViolation_NoProblemAdded ()
    {
      var expectedFragment = Fragment.CreateNamed (c_expectedType);
      var givenFragment = Fragment.CreateNamed (c_expectedType);
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      context.MakeSafe (c_symbol, givenFragment);
      IProblemPipe problemPipe = MockRepository.GenerateMock<IProblemPipe>();
      
      preCondition.HandleViolation (context, problemPipe);

      problemPipe.AssertWasNotCalled (pipe => pipe.AddProblem (Arg<ProblemMetadata>.Is.Anything));
    }
    
    [Test]
    public void HandleViolation_ViolationProvoked_SetsSymbolUnsafe ()
    {
      var expectedFragment = Fragment.CreateNamed (c_expectedType);
      var givenFragment = Fragment.CreateNamed ("unexpected");
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      context.MakeSafe (c_symbol, givenFragment);
      IProblemPipe problemPipe = MockRepository.GenerateMock<IProblemPipe>();
      
      preCondition.HandleViolation (context, problemPipe);

      bool symbolSetUnafe = context.GetFragmentType (c_symbol) == Fragment.CreateEmpty();
      Assert.That (symbolSetUnafe, Is.True);
    }

    [Test]
    public void HandleViolation_ViolationNotProvoked_KeepsSymbolFragment ()
    {
      var expectedFragment = Fragment.CreateNamed (c_expectedType);
      var givenFragment = expectedFragment;
      
      IPreCondition preCondition = new CustomInferencePreCondition(c_symbol, expectedFragment, _problemMetadata);
      var context = new SymbolTable(_blacklistManager);
      context.MakeSafe (c_symbol, givenFragment);
      IProblemPipe problemPipe = MockRepository.GenerateMock<IProblemPipe>();
      
      preCondition.HandleViolation (context, problemPipe);

      bool symbolFragmentKept = context.GetFragmentType (c_symbol) == expectedFragment;
      Assert.That (symbolFragmentKept, Is.True);
    }
  }
}
