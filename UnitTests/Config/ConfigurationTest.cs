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
using System.Xml.Schema;
using InjectionCop.Config;
using NUnit.Framework;

namespace InjectionCop.UnitTests.Config
{
  [TestFixture]
  public class ConfigurationTest
  {
    [Test]
    public void GetMethodKey ()
    {
      var configuration = new Configuration();

      var actual = configuration.GetMethodKey ("Assembly", "TypeName", "MethodName", new List<string> { "Parameter1", "Parameter2" });
      Assert.That (actual, Is.EqualTo ("Assembly|TypeName|MethodName|Parameter1|Parameter2"));
    }

    [Test]
    public void LoadXml_EmptyConfiguration ()
    {
      var configuration = new Configuration();

      configuration.LoadXml (@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Blacklist xmlns=""http://injectioncop.codeplex.com/"">
</Blacklist>");
    }

    [Test]
    public void LoadXml_CreatesFragmentSignature ()
    {
      var configuration = new Configuration();

      configuration.LoadXml (@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Blacklist xmlns=""http://injectioncop.codeplex.com/"">
  <Assembly name=""mscorlib"">
    <Type name=""System.IO.File"">
      <Method name=""ReadAllText"" returnFragmentType=""ReturnFragmentType"" fragmentGenerator=""false"" >
        <Parameter type=""System.String"" fragmentType=""ParameterFragmentType"" />
      </Method>
    </Type>
  </Assembly>
</Blacklist>");

      var fragmentSignature = configuration.GetFragmentTypes ("mscorlib", "System.IO.File", "ReadAllText", new List<string> { "System.String" });

      Assert.That (fragmentSignature, Is.Not.Null);
      Assert.That (fragmentSignature.ReturnFragmentType, Is.EqualTo ("ReturnFragmentType"));
      Assert.That (fragmentSignature.ParameterFragmentTypes, Is.Not.Null);
      Assert.That (fragmentSignature.ParameterFragmentTypes.Length, Is.EqualTo (1));
      Assert.That (fragmentSignature.ParameterFragmentTypes[0], Is.EqualTo ("ParameterFragmentType"));
    }

    [Test]
    [ExpectedException (typeof (ArgumentException),
        ExpectedMessage = "Method TypeName.MethodName(ParameterType1, ParameterType2) is defined more than once.")]
    public void LoadXml_DuplicateMethod ()
    {
      var configuration = new Configuration();

      configuration.LoadXml (@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Blacklist xmlns=""http://injectioncop.codeplex.com/"">
  <Assembly name=""AssemlbyName"">
    <Type name=""TypeName"">
      <Method name=""MethodName"" returnFragmentType=""ReturnFragmentType"" fragmentGenerator=""false"" >
        <Parameter type=""ParameterType1"" fragmentType=""ParameterFragmentType"" />
        <Parameter type=""ParameterType2"" fragmentType=""ParameterFragmentType"" />
      </Method>
      <Method name=""MethodName"" returnFragmentType=""ReturnFragmentType"" fragmentGenerator=""false"" >
        <Parameter type=""ParameterType1"" fragmentType=""ParameterFragmentType"" />
        <Parameter type=""ParameterType2"" fragmentType=""ParameterFragmentType"" />
      </Method>
    </Type>
  </Assembly>
</Blacklist>");
    }

    [Test]
    public void LoadXml_AcceptsMethodOverloads ()
    {
      var configuration = new Configuration();

      configuration.LoadXml (@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Blacklist xmlns=""http://injectioncop.codeplex.com/"">
  <Assembly name=""AssemlbyName"">
    <Type name=""TypeName"">
      <Method name=""MethodName"" returnFragmentType=""ReturnFragmentType"" fragmentGenerator=""false"" >
        <Parameter type=""ParameterType1"" fragmentType=""ParameterFragmentType""/>
      </Method>
      <Method name=""MethodName"" returnFragmentType=""ReturnFragmentType"" fragmentGenerator=""false"" >
        <Parameter type=""ParameterType2"" fragmentType=""ParameterFragmentType"" />
      </Method>
    </Type>
  </Assembly>
</Blacklist>");
    }

    [Test]
    public void LoadXml_MergesXmlFiles ()
    {
      var configuration = new Configuration();

      configuration.LoadXml (@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Blacklist xmlns=""http://injectioncop.codeplex.com/"">
  <Assembly name=""AssemlbyName"">
    <Type name=""TypeName"">
      <Method name=""MethodName"" returnFragmentType=""ReturnFragmentType"" fragmentGenerator=""false"" >
        <Parameter type=""ParameterType1"" fragmentType=""ParameterFragmentType"" />
      </Method>
    </Type>
  </Assembly>
</Blacklist>");

      configuration.LoadXml (@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Blacklist xmlns=""http://injectioncop.codeplex.com/"">
  <Assembly name=""AssemlbyName"">
    <Type name=""TypeName"">
      <Method name=""MethodName"" returnFragmentType=""ReturnFragmentType"" fragmentGenerator=""false"" >
        <Parameter type=""ParameterType2"" fragmentType=""ParameterFragmentType"" />
      </Method>
    </Type>
  </Assembly>
</Blacklist>");

      var fragmentSignature1 = configuration.GetFragmentTypes ("AssemlbyName", "TypeName", "MethodName", new List<string> { "ParameterType1" });
      Assert.That (fragmentSignature1, Is.Not.Null);

      var fragmentSignature2 = configuration.GetFragmentTypes ("AssemlbyName", "TypeName", "MethodName", new List<string> { "ParameterType2" });
      Assert.That (fragmentSignature2, Is.Not.Null);
    }

    [Test]
    public void GetFragmentTypes_UnknownMethod ()
    {
      var configuration = new Configuration();

      var fragmentSignature = configuration.GetFragmentTypes ("AssemlbyName", "TypeName", "UnknownMethodName", new List<string> { "ParameterType1" });
      Assert.That (fragmentSignature, Is.Null);
    }

    [Test]
    [ExpectedException (typeof (XmlSchemaValidationException))]
    public void LoadXml_ValidatesSchema ()
    {
      var configuration = new Configuration();

      configuration.LoadXml (@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Blacklist xmlns=""http://injectioncop.codeplex.com/"">
  <InvalidElement />
</Blacklist>");
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void LoadXml_IsGenerator (bool isFragmentGenerator)
    {
      var configuration = new Configuration();

      configuration.LoadXml(string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Blacklist xmlns=""http://injectioncop.codeplex.com/"">
  <Assembly name=""AssemlbyName"">
    <Type name=""TypeName"">
      <Method name=""MethodName"" returnFragmentType=""ReturnFragmentType"" fragmentGenerator=""{0}"">
        <Parameter type=""ParameterType1"" fragmentType=""ParameterFragmentType"" />
      </Method>
    </Type>
  </Assembly>
</Blacklist>", isFragmentGenerator.ToString().ToLower()));

      var fragmentSignature = configuration.GetFragmentTypes("AssemlbyName", "TypeName", "MethodName", new List<string> { "ParameterType1" });
      Assert.That (fragmentSignature.IsGenerator, Is.EqualTo(isFragmentGenerator));
    }
  }
}