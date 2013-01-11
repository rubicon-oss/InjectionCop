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
using System.Collections.Generic;
using InjectionCop.Config;
using NUnit.Framework;
using Rhino.Mocks;

namespace InjectionCop.UnitTests.Config
{
  [TestFixture]
  public class LayeredConfigurationAdapterTest
  {
    private string _assemblyName = "AssemblyName";
    private string _typename = "TypeName";
    private string _methodname = "MethodName";
    private List<string> _parameters = new List<string>();

    [Test]
    public void GetFragmentTypes_ReturnsFragmentType ()
    {
      var blacklistManagerStub = MockRepository.GenerateStub<IBlacklistManager>();
      var expected = new FragmentSignature (new string[0], "ReturnType", false);
      blacklistManagerStub
          .Stub (stub => stub.GetFragmentTypes (_assemblyName, _typename, _methodname, _parameters))
          .Return (expected);

      var layers = new Stack<IBlacklistManager>();
      layers.Push (blacklistManagerStub);

      var layeredConfigurationAdapter = new LayeredConfigurationAdapter (layers);

      Assert.That (layeredConfigurationAdapter.GetFragmentTypes (_assemblyName, _typename, _methodname, _parameters), Is.SameAs (expected));
    }

    [Test]
    public void GetFragmentTypes_UnknownMethod ()
    {
      var blacklistManagerStub = MockRepository.GenerateStub<IBlacklistManager>();
      var layers = new Stack<IBlacklistManager>();
      layers.Push (blacklistManagerStub);

      var layeredConfigurationAdapter = new LayeredConfigurationAdapter (layers);

      Assert.That (layeredConfigurationAdapter.GetFragmentTypes (_assemblyName, _typename, _methodname, _parameters), Is.Null);
    }

    [Test]
    public void GetFragmentTypes_PrioritizesLayers ()
    {
      var lowerPriorityStub = MockRepository.GenerateStub<IBlacklistManager>();
      var lowerPriorityFragmentSignature = new FragmentSignature (new string[0], "ReturnType", false);
      lowerPriorityStub
          .Stub (stub => stub.GetFragmentTypes (_assemblyName, _typename, _methodname, _parameters))
          .Return (lowerPriorityFragmentSignature);

      var higherPriorityStub = MockRepository.GenerateStub<IBlacklistManager>();
      var higherPriorityFragmentSignature = new FragmentSignature (new string[0], "ReturnType", false);
      higherPriorityStub
          .Stub (stub => stub.GetFragmentTypes (_assemblyName, _typename, _methodname, _parameters))
          .Return (higherPriorityFragmentSignature);

      var layers = new Stack<IBlacklistManager>();
      layers.Push (lowerPriorityStub);
      layers.Push (higherPriorityStub);

      var layeredConfigurationAdapter = new LayeredConfigurationAdapter (layers);

      Assert.That (
          layeredConfigurationAdapter.GetFragmentTypes (_assemblyName, _typename, _methodname, _parameters),
          Is.SameAs (higherPriorityFragmentSignature));
    }

    [Test]
    public void GetFragmentTypes_FallsBackToLowerLayers ()
    {
      var lowerPriorityStub = MockRepository.GenerateStub<IBlacklistManager>();
      var lowerPriorityFragmentSignature = new FragmentSignature (new string[0], "ReturnType", false);
      lowerPriorityStub
          .Stub (stub => stub.GetFragmentTypes (_assemblyName, _typename, _methodname, _parameters))
          .Return (lowerPriorityFragmentSignature);

      var higherPriorityStub = MockRepository.GenerateStub<IBlacklistManager>();

      var layers = new Stack<IBlacklistManager>();
      layers.Push (lowerPriorityStub);
      layers.Push (higherPriorityStub);

      var layeredConfigurationAdapter = new LayeredConfigurationAdapter (layers);

      Assert.That (
          layeredConfigurationAdapter.GetFragmentTypes (_assemblyName, _typename, _methodname, _parameters),
          Is.SameAs (lowerPriorityFragmentSignature));
    }
  }
}