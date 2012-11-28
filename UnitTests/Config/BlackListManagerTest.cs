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
using System.Xml.Linq;
using InjectionCop.Parser;
using NUnit.Framework;
using InjectionCop.Config;

namespace InjectionCop.UnitTests.Config
{
  [TestFixture]
  public class BlacklistManagerTest
  {
    private BlacklistManager _blackListManager;

    [SetUp]
    public void SetUp ()
    {
      string xmlBlackList =
        "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + 
        "<ic:Blacklist xmlns:ic=\"eu.rubicon.injectioncop\">" +
        " <ic:Type qualifiedName=\"namespace.BlackType\">" +
        "   <ic:Method name=\"Method\">" + 
        "     <ic:Parameter qualifiedParameterTypeName=\"namespace.type1\" fragmentType=\"fragmentType\">fragmentParameter</ic:Parameter>" +
        "     <ic:Parameter qualifiedParameterTypeName=\"namespace.type2\" >nonFragmentParameter</ic:Parameter>" +
        "   </ic:Method>" +
        " </ic:Type>" +
        " <ic:Type qualifiedName=\"namespace.BlackType2\">" +
        "   <ic:Method name=\"Method2\">" + 
        "     <ic:Parameter qualifiedParameterTypeName=\"namespace.type3\" fragmentType=\"fragmentType2\">fragmentParameter2</ic:Parameter>" +
        "     <ic:Parameter qualifiedParameterTypeName=\"namespace.type4\" >nonFragmentParameter2</ic:Parameter>" +
        "   </ic:Method>" +
        " </ic:Type>" +
        "</ic:Blacklist>";
      XElement blacklist = XElement.Parse (xmlBlackList);
      _blackListManager = new BlacklistManager(blacklist);
    }

    [Test]
    public void IsListed_FirstListedMethod_ReturnsTrue ()
    {
      List<string> parameters = new List<string> { "namespace.type1", "namespace.type2" };
      bool isListed = _blackListManager.IsListed ("namespace.BlackType", "Method", parameters);
      Assert.That (isListed, Is.True);
    }

    [Test]
    public void IsListed_ListedMethod_ReturnsTrue ()
    {
      List<string> parameters = new List<string> { "namespace.type3", "namespace.type4" };
      bool isListed = _blackListManager.IsListed ("namespace.BlackType2", "Method2", parameters);
      Assert.That (isListed, Is.True);
    }

    [Test]
    public void IsListed_UnknownType_ReturnsFalse ()
    {
      List<string> parameters = new List<string>();
      bool isListed = _blackListManager.IsListed ("ns.NotExistingType", "Method", parameters);
      Assert.That (isListed, Is.False);
    }

    [Test]
    public void IsListed_UnknownMethod_ReturnsFalse ()
    {
      List<string> parameters = new List<string> { "namespace.type3", "namespace.type4" };
      bool isListed = _blackListManager.IsListed ("namespace.BlackType2", "UnknownMethod", parameters);
      Assert.That (isListed, Is.False);
    }

    [Test]
    public void IsListed_UnknownMethodSignature_ReturnsFalse ()
    {
      List<string> parameters = new List<string> { "namespace.type3", "unknowntype" };
      bool isListed = _blackListManager.IsListed ("namespace.BlackType2", "Method2", parameters);
      Assert.That (isListed, Is.False);
    }

    [Test]
    public void GetFragmentTypes_UnknownMethodSignature_ReturnsFalse ()
    {
      List<string> parameters = new List<string> { "namespace.type3", "unknowntype" };
      Dictionary<string,string> fragmentTypes = _blackListManager.GetFragmentTypes ("namespace.BlackType2", "Method2", parameters);
      Assert.That (fragmentTypes.Keys.Count, Is.EqualTo(0));
    }

    [Test]
    public void GetFragmentTypes_ListetMethod_ReturnsFragmentTypes ()
    {
      List<string> parameters = new List<string> { "namespace.type1", "namespace.type2" };
      Dictionary<string,string> fragmentTypes = _blackListManager.GetFragmentTypes ("namespace.BlackType", "Method", parameters);
      bool fragmentCorrect = fragmentTypes["fragmentParameter"] == "fragmentType";
      bool nonFragmentCorrect = fragmentTypes["nonFragmentParameter"] == SymbolTable.EmptyFragment;
      Assert.That (fragmentCorrect && nonFragmentCorrect, Is.True);
    }

    // Todo Tests für emptytype, parameterless method, interface
  }
}
