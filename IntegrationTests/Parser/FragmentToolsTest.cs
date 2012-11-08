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
using InjectionCop.Attributes;
using InjectionCop.Parser;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser
{
  [TestFixture]
  public class FragmentToolsTest
  {
    [Test]
    public void Contains_Fragment_True()
    {
      TypeNode stringTypeNode = Helper.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentToolsSample> ("ContainsFragmentParameter", stringTypeNode);
      bool containsResult = FragmentTools.Contains (FragmentAttribute.OfType ("FragmentType"), sample.Parameters[0].Attributes);
      Assert.That (containsResult, Is.True);
    }

    
    [Test]
    public void Contains_NoFragment_False()
    {
      TypeNode stringTypeNode = Helper.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentToolsSample> ("NoFragmentParameter", stringTypeNode);
      bool containsResult = FragmentTools.Contains (FragmentAttribute.OfType ("FragmentType"), sample.Parameters[0].Attributes);
      Assert.That (containsResult, Is.False);
    }

    
    [Test]
    public void Is_FragmentAttributeNode_True()
    {
      TypeNode stringTypeNode = Helper.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentToolsSample> ("ContainsFragmentParameter", stringTypeNode);
      bool isResult = FragmentTools.Is(FragmentAttribute.OfType ("FragmentType"), sample.Parameters[0].Attributes[0]);
      Assert.That (isResult, Is.True);
    }

    [Test]
    public void Is_ContainsNonFragmentAttributeNode_False()
    {
      TypeNode stringTypeNode = Helper.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentToolsSample> ("ContainsNonFragmentParameter", stringTypeNode);
      bool isResult = FragmentTools.Is(FragmentAttribute.OfType ("FragmentType"), sample.Parameters[0].Attributes[0]);
      Assert.That (isResult, Is.False);
    }

    [Test]
    public void Contains_SqlFragment_True()
    {
      TypeNode stringTypeNode = Helper.TypeNodeFactory<string>();
      Method sample = TestHelper.GetSample<FragmentToolsSample> ("ContainsSqlFragmentParameter", stringTypeNode);
      bool containsResult = FragmentTools.Contains (new FragmentAttribute("SqlFragment"), sample.Parameters[0].Attributes);
      Assert.That (containsResult, Is.True);
    }
  }
}
