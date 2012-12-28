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
using InjectionCop.Fragment;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Inheritance.Interface
{
  public interface IInheritanceSample
  {
    string MethodWithFragmentParameter ([Fragment ("InterfaceFragment")] string fragmentParameter, string nonFragmentParameter);

    [return: Fragment ("InterfaceFragment")]
    string MethodWithReturnFragment ();
  }

  public interface IInheritanceSampleDuplicate
  {
    string MethodWithFragmentParameter ([Fragment ("InterfaceFragment")] string fragmentParameter, string nonFragmentParameter);

    [return: Fragment ("InterfaceFragment")]
    string MethodWithReturnFragment ();
  }

  public class InterfaceSampleImplicitDeclarations : IInheritanceSample
  {
    public string MethodWithFragmentParameter (string fragmentParameter, string nonFragmentParameter)
    {
      return "dummy";
    }

    public string MethodWithReturnFragment ()
    {
      return "safe";
    }
  }

  public class InterfaceSampleImplicitDeclarationsInvalidReturn : ParserSampleBase, IInheritanceSample
  {
    public string MethodWithFragmentParameter (string fragmentParameter, string nonFragmentParameter)
    {
      return "dummy";
    }

    public string MethodWithReturnFragment ()
    {
      return UnsafeSource();
    }
  }

  public class InterfaceSampleExplicitDeclarations : IInheritanceSample
  {
    string IInheritanceSample.MethodWithFragmentParameter (string fragmentParameter, string nonFragmentParameter)
    {
      return "dummy";
    }

    string IInheritanceSample.MethodWithReturnFragment ()
    {
      return "safe";
    }
  }

  public class InterfaceSampleExplicitDeclarationsInvalidReturn : ParserSampleBase, IInheritanceSample
  {
    string IInheritanceSample.MethodWithFragmentParameter (string fragmentParameter, string nonFragmentParameter)
    {
      return "dummy";
    }

    string IInheritanceSample.MethodWithReturnFragment ()
    {
      return UnsafeSource();
    }
  }

  public class InterfaceSampleImplicitDeclarationsMultipleInheritance : IInheritanceSample, IInheritanceSampleDuplicate
  {
    public string MethodWithFragmentParameter (string fragmentParameter, string nonFragmentParameter)
    {
      return "dummy";
    }

    public string MethodWithReturnFragment ()
    {
      return "safe";
    }
  }

// ReSharper disable PossibleInterfaceMemberAmbiguity
  public interface IExtendedInheritanceSample : IInheritanceSample, IInheritanceSampleDuplicate
// ReSharper restore PossibleInterfaceMemberAmbiguity
  {
  }

  public class ExtendedInterfaceSampleImplicitDeclarations : IExtendedInheritanceSample
  {
    public string MethodWithFragmentParameter (string fragmentParameter, string nonFragmentParameter)
    {
      return "dummy";
    }

    public string MethodWithReturnFragment ()
    {
      return "safe";
    }
  }

  public class ExtendedInterfaceSampleImplicitDeclarationsInvalidReturn : ParserSampleBase, IExtendedInheritanceSample
  {
    public string MethodWithFragmentParameter (string fragmentParameter, string nonFragmentParameter)
    {
      return "dummy";
    }

    public string MethodWithReturnFragment ()
    {
      return UnsafeSource();
    }
  }
  
  public class InheritanceSampleInterface : ParserSampleBase
  {
    public void SafeCallOnInterfaceMethodWithFragmentParameter ()
    {
      IInheritanceSample sample = new InterfaceSampleImplicitDeclarations();
      sample.MethodWithFragmentParameter ("safe", "safe");
    }

    public void UnsafeCallOnInterfaceMethodWithFragmentParameter ()
    {
      IInheritanceSample sample = new InterfaceSampleImplicitDeclarations();
      sample.MethodWithFragmentParameter (UnsafeSource(), "safe");
    }

    public void InterfaceReturnFragmentsAreConsidered ()
    {
      IInheritanceSample sample = new InterfaceSampleImplicitDeclarations();
      sample.MethodWithFragmentParameter (sample.MethodWithReturnFragment(), "safe");
    }

    public void SafeCallOnExplicitInterfaceMethodWithFragmentParameter ()
    {
      IInheritanceSample sample = new InterfaceSampleExplicitDeclarations();
      sample.MethodWithFragmentParameter ("safe", "safe");
    }

    public void UnsafeCallOnExplicitInterfaceMethodWithFragmentParameter ()
    {
      IInheritanceSample sample = new InterfaceSampleExplicitDeclarations();
      sample.MethodWithFragmentParameter (UnsafeSource(), "safe");
    }

    public void InterfaceReturnFragmentsOfExplicitlyDeclaredMethodAreConsidered ()
    {
      IInheritanceSample sample = new InterfaceSampleExplicitDeclarations();
      sample.MethodWithFragmentParameter (sample.MethodWithReturnFragment(), "safe");
    }

    public void SafeCallOnClassImplementingInterfaceMethodWithFragmentParameter ()
    {
      InterfaceSampleImplicitDeclarations sample = new InterfaceSampleImplicitDeclarations();
      sample.MethodWithFragmentParameter ("safe", "safe");
    }

    public void UnsafeCallOnClassImplementingInterfaceMethodWithFragmentParameter ()
    {
      InterfaceSampleImplicitDeclarations sample = new InterfaceSampleImplicitDeclarations();
      sample.MethodWithFragmentParameter (UnsafeSource(), "safe");
    }

    public void InterfaceReturnFragmentsOfClassImplementingInterfaceMethodAreConsidered ()
    {
      InterfaceSampleImplicitDeclarations sample = new InterfaceSampleImplicitDeclarations();
      sample.MethodWithFragmentParameter (sample.MethodWithReturnFragment(), "safe");
    }

    public void SafeCallOnClassImplementingMethodWithFragmentParameterFromMultipleInterfaces ()
    {
      InterfaceSampleImplicitDeclarationsMultipleInheritance sample = new InterfaceSampleImplicitDeclarationsMultipleInheritance();
      sample.MethodWithFragmentParameter ("safe", "safe");
    }

    public void UnsafeCallOnClassImplementingMethodWithFragmentParameterFromMultipleInterfaces ()
    {
      InterfaceSampleImplicitDeclarationsMultipleInheritance sample = new InterfaceSampleImplicitDeclarationsMultipleInheritance();
      sample.MethodWithFragmentParameter (UnsafeSource(), "safe");
    }

    public void InterfaceReturnFragmentsOfClassImplementingInterfaceMethodFromMultipleInterfacesAreConsidered ()
    {
      InterfaceSampleImplicitDeclarationsMultipleInheritance sample = new InterfaceSampleImplicitDeclarationsMultipleInheritance();
      sample.MethodWithFragmentParameter (sample.MethodWithReturnFragment(), "safe");
    }
    
    public void SafeCallOnExtendedInterfaceMethodWithFragmentParameter ()
    {
      IInheritanceSampleDuplicate sample = new ExtendedInterfaceSampleImplicitDeclarations();
      sample.MethodWithFragmentParameter ("safe", "safe");
    }

    public void UnsafeCallOnExtendedInterfaceMethodWithFragmentParameter ()
    {
      IInheritanceSample sample = new ExtendedInterfaceSampleImplicitDeclarations();
      sample.MethodWithFragmentParameter (UnsafeSource(), "safe");
    }

    public void ExtendedInterfaceReturnFragmentsAreConsidered ()
    {
      IInheritanceSampleDuplicate sample = new ExtendedInterfaceSampleImplicitDeclarations();
      sample.MethodWithFragmentParameter (sample.MethodWithReturnFragment(), "safe");
    }
  }
}
