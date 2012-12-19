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
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Utilities
{
  [TestFixture]
  public class IntrospectionUtility_InterfaceTest
  {
    [Test]
    public void TypeNodeFactory_Interface_ReturnsInterfaceTypeNode ()
    {
      Type sampleType = typeof (IntrospectionUtility_InterfaceSample);
      TypeNode result = IntrospectionUtility.TypeNodeFactory (sampleType);
      Assert.That (result.FullName, Is.EqualTo (sampleType.FullName));
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TypeNodeFactory_Null_ReturnsException ()
    {
      IntrospectionUtility.TypeNodeFactory (null);
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void MethodFactory_TypeNull_ReturnsException ()
    {
      IntrospectionUtility.MethodFactory ((Type)null, "methodname");
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void MethodFactory_MethodNameNull_ReturnsException ()
    {
      IntrospectionUtility.MethodFactory (typeof(IntrospectionUtility_InterfaceSample), null);
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void MethodFactory_MethodWithParametersParametersNull_ReturnsException ()
    {
      IntrospectionUtility.MethodFactory (typeof(IntrospectionUtility_InterfaceSample), "MethodWithParameter", null);
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void MethodFactory_MethodWithoutParametersParametersNull_ReturnsException ()
    {
      IntrospectionUtility.MethodFactory (typeof(IntrospectionUtility_InterfaceSample), "MethodWithoutParameters", null);
    }

    [Test]
    public void MethodFactory_MethodWithoutParametersCorrectCall_ReturnsMethod ()
    {
      Type sampleType = typeof (IntrospectionUtility_InterfaceSample);
      string sampleMethodname = "MethodWithoutParameters";
      Method result = IntrospectionUtility.MethodFactory (sampleType, sampleMethodname);
      bool correctType = result.DeclaringType.FullName == sampleType.FullName;
      bool correctMethod = result.Name.Name == sampleMethodname;

      Assert.That (correctMethod && correctType, Is.True);
    }

    [Test]
    public void MethodFactory_MethodWithParametersCorrectCall_ReturnsMethod ()
    {
      Type sampleType = typeof (IntrospectionUtility_InterfaceSample);
      TypeNode intTypeNode = IntrospectionUtility.TypeNodeFactory<int>();
      string sampleMethodname = "MethodWithParameter";
      Method result = IntrospectionUtility.MethodFactory (sampleType, sampleMethodname, intTypeNode);
      bool correctType = result.DeclaringType.FullName == sampleType.FullName;
      bool correctMethod = result.Name.Name == sampleMethodname;

      Assert.That (correctMethod && correctType, Is.True);
    }

    [Test]
    public void MethodFactory_MethodWithParametersWrongParameterType_ReturnsNull ()
    {
      Type sampleType = typeof (IntrospectionUtility_InterfaceSample);
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      string sampleMethodname = "MethodWithParameter";
      Method result = IntrospectionUtility.MethodFactory (sampleType, sampleMethodname, stringTypeNode);
      
      Assert.That (result, Is.Null);
    }
  }
}
