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

    public void LoadValidExample ()
    {
      string xmlBlackList =
        "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + 
        "<ic:Blacklist xmlns:ic=\"eu.rubicon.injectioncop\">" +
        " <ic:Type qualifiedName=\"namespace.BlackType\">" +
        "   <ic:Method name=\"Method\">" + 
        "     <ic:Parameter qualifiedParameterTypeName=\"namespace.type1\" fragmentType=\"fragmentType\"/>" +
        "     <ic:Parameter qualifiedParameterTypeName=\"namespace.type2\"/>" +
        "   </ic:Method>" +
        " </ic:Type>" +
        " <ic:Type qualifiedName=\"namespace.BlackType2\">" +
        "   <ic:Method name=\"Method2\">" + 
        "     <ic:Parameter qualifiedParameterTypeName=\"namespace.type3\" fragmentType=\"fragmentType2\"/>" +
        "     <ic:Parameter qualifiedParameterTypeName=\"namespace.type4\"/>" +
        "   </ic:Method>" +
        "   <ic:Method name=\"ParameterlessMethod\"/>" +
        " </ic:Type>" +
        " <ic:Type qualifiedName=\"namespace.EmptyType\"/>" +
        "</ic:Blacklist>";
      XElement blacklist = XElement.Parse (xmlBlackList);
      _blackListManager = new BlacklistManager(blacklist);
    }

    public void LoadHeaderlessExample ()
    {
      string xmlBlackList =
        "<ic:Blacklist xmlns:ic=\"eu.rubicon.injectioncop\">" +
        " <ic:Type qualifiedName=\"namespace.BlackType\">" +
        "   <ic:Method name=\"Method\">" + 
        "   </ic:Method>" +
        " </ic:Type>" +
        "</ic:Blacklist>";
      XElement blacklist = XElement.Parse (xmlBlackList);
      _blackListManager = new BlacklistManager(blacklist);
    }

    public void LoadWrongSource ()
    {
      string xmlBlackList =
        "<ic:WrongRootElement xmlns:ic=\"eu.rubicon.injectioncop\">" +
        " <ic:Type qualifiedName=\"namespace.BlackType\">" +
        "   <ic:Method name=\"Method\">" + 
        "   </ic:Method>" +
        " </ic:Type>" +
        "</ic:WrongRootElement>";
      XElement blacklist = XElement.Parse (xmlBlackList);
      _blackListManager = new BlacklistManager(blacklist);
    }

    public void LoadTypoExample ()
    {
      string xmlBlackList =
        "<ic:Blacklist xmlns:ic=\"eu.rubicon.injectioncop\">" +
        " <ic:Typ0 qualifiedName=\"namespace.BlackType\">" +
        "   <ic:Method name=\"Method\">" + 
        "   </ic:Method>" +
        " </ic:Typ0>" +
        "</ic:Blacklist>";
      XElement blacklist = XElement.Parse (xmlBlackList);
      _blackListManager = new BlacklistManager(blacklist);
    }

    [Test]
    public void GetFragmentTypes_FirstListedMethod_RecognizesMethodSignature ()
    {
      LoadValidExample();
      List<string> parameters = new List<string> { "namespace.type1", "namespace.type2" };
      bool isListed = null != _blackListManager.GetFragmentTypes ("namespace.BlackType", "Method", parameters);
      Assert.That (isListed, Is.True);
    }

    [Test]
    public void GetFragmentTypes_ListedMethod_RecognizesMethodSignature ()
    {
      LoadValidExample();
      List<string> parameters = new List<string> { "namespace.type3", "namespace.type4" };
      bool isListed = null != _blackListManager.GetFragmentTypes ("namespace.BlackType2", "Method2", parameters);
      Assert.That (isListed, Is.True);
    }

    [Test]
    public void GetFragmentTypes_UnknownType_ReturnsNull ()
    {
      LoadValidExample();
      List<string> parameters = new List<string>();
      string[] fragmentTypes = _blackListManager.GetFragmentTypes ("ns.NotExistingType", "Method", parameters);
      Assert.That (fragmentTypes, Is.Null);
    }

    [Test]
    public void GetFragmentTypes_UnknownMethod_ReturnsNull ()
    {
      LoadValidExample();
      List<string> parameters = new List<string> { "namespace.type3", "namespace.type4" };
      string[] fragmentTypes = _blackListManager.GetFragmentTypes ("namespace.BlackType2", "UnknownMethod", parameters);
      Assert.That (fragmentTypes, Is.Null);
    }

    [Test]
    public void GetFragmentTypes_UnknownMethodSignature_ReturnsNull ()
    {
      LoadValidExample();
      List<string> parameters = new List<string> { "namespace.type3", "unknowntype" };
      string[] fragmentTypes = _blackListManager.GetFragmentTypes ("namespace.BlackType2", "Method2", parameters);
      Assert.That (fragmentTypes, Is.Null);
    }

    [Test]
    public void GetFragmentTypes_ParameterlessMethod_RecognizesSignature ()
    {
      LoadValidExample();
      List<string> parameters = new List<string> ();
      bool isListed = null != _blackListManager.GetFragmentTypes ("namespace.BlackType2", "ParameterlessMethod", parameters);
      Assert.That (isListed, Is.True);
    }
    
    [Test]
    public void GetFragmentTypes_EmptyType_ReturnsNull ()
    {
      LoadValidExample();
      List<string> parameters = new List<string> ();
      string[] fragmentTypes = _blackListManager.GetFragmentTypes ("namespace.EmptyType", "dummy", parameters);
      Assert.That (fragmentTypes, Is.Null);
    }

    [Test]
    public void GetFragmentTypes_NonExistingMethodInHeaderLessXML_ReturnsNull ()
    {
      LoadHeaderlessExample();
      List<string> parameters = new List<string>();
      string[] fragmentTypes = _blackListManager.GetFragmentTypes ("namespace.BlackType", "NonExistingMethod", parameters);
      Assert.That (fragmentTypes, Is.Null);
    }

    [Test]
    public void GetFragmentTypes_ExistingMethodInHeaderLessXML_RecognizesHeader ()
    {
      LoadHeaderlessExample();
      List<string> parameters = new List<string>();
      bool isListed = null != _blackListManager.GetFragmentTypes ("namespace.BlackType", "Method", parameters);
      Assert.That (isListed, Is.True);
    }

    [Test]
    public void GetFragmentTypes_WrongElementNaming_ReturnsNull ()
    {
      LoadWrongSource();
      List<string> parameters = new List<string>();
      string[] fragmentTypes = _blackListManager.GetFragmentTypes ("namespace.BlackType", "Method", parameters);
      Assert.That (fragmentTypes, Is.Null);
    }

    [Test]
    public void GetFragmentTypes_TypoInElementNaming_ReturnsNull ()
    {
      LoadTypoExample();
      List<string> parameters = new List<string>();
      string[] fragmentTypes = _blackListManager.GetFragmentTypes ("namespace.BlackType", "Method", parameters);
      Assert.That (fragmentTypes, Is.Null);
    }
    
    [Test]
    public void GetFragmentTypes_AnotherUnknownMethodSignature_ReturnsNull ()
    {
      LoadValidExample();
      List<string> parameters = new List<string> { "namespace.type3", "unknowntype" };
      string[] fragmentTypes = _blackListManager.GetFragmentTypes ("namespace.BlackType2", "Method2", parameters);
      Assert.That (fragmentTypes, Is.Null);
    }

    [Test]
    public void GetFragmentTypes_ListetMethod_ReturnsFragmentTypes ()
    {
      LoadValidExample();
      List<string> parameters = new List<string> { "namespace.type1", "namespace.type2" };
      string[] fragmentTypes = _blackListManager.GetFragmentTypes ("namespace.BlackType", "Method", parameters);
      bool fragmentCorrect = fragmentTypes[0] == "fragmentType";
      bool nonFragmentCorrect = fragmentTypes[1] == SymbolTable.EMPTY_FRAGMENT;
      Assert.That (fragmentCorrect && nonFragmentCorrect, Is.True);
    }

    [Test]
    public void GetFragmentTypes_ListetParameterlessMethod_ReturnsNoFragmentTypes ()
    {
      LoadValidExample();
      List<string> parameters = new List<string> ();
      string[] fragmentTypes = _blackListManager.GetFragmentTypes ("namespace.BlackType2", "ParameterlessMethod", parameters);
      bool noFragmentTypesReturned = fragmentTypes.Length == 0;
      Assert.That (noFragmentTypesReturned, Is.True);
    }
  }
}
