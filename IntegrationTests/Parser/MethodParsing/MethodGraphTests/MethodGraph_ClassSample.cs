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
using System.Globalization;
using InjectionCop.Attributes;

namespace InjectionCop.IntegrationTests.Parser.MethodParsing.MethodGraphTests
{
  public class MethodGraph_ClassSample: ParserSampleBase
  {
    [return: Fragment("ReturnFragmentType")]
    public int DeclarationWithReturn()
    {
      int i = 3;
      return i;
    }

    public string IfStatementTrueBlockOnly(string param)
    {
      if (param == "dummy")
      {
        param = "changed";
      }
      return param;
    }

    public int ForLoop()
    {
      int result = 0;
      for(int i = 10; i > 0; i--)
      {
        result += i;
      }
      return result;
    }

    [return: Fragment ("DummyFragment")]
    public string ValidReturnWithIf ([Fragment ("DummyFragment")] string parameter)
    {
      string returnValue = "";
      if (parameter == "Dummy")
      {
        returnValue = "safe";
      }
      return returnValue;
    }

    public void FragmentOutParameterSafeReturn([Fragment("SqlFragment")] out string safe)
    {
      string temp = "safe";
      safe = "safe";
      if (SafeSource() == "dummy")
      {
        safe = temp;
      }
    }

    public void FragmentRefParameterSafeReturn([Fragment("SqlFragment")] ref string safe)
    {
      DummyMethod (safe);
      string temp = "safe";
      safe = "safe";
      if (SafeSource() == "dummy")
      {
        safe = temp;
      }
    }

    public void TryCatchFinally ()
    {
      try
      {
        RequiresSqlFragment (UnsafeSource());
        ThrowsException (0);
      }
      catch (ArgumentNullException ex)
      {
        DummyMethod (ex.Message);
      }
      finally
      {
        DummyMethod ("");
      }
      DummyMethod ("dummy");
    }

    private void ThrowsException (int parameter)
    {
      if (parameter == 1)
      {
        throw new ArgumentNullException();
      }
      DummyMethod (parameter.ToString (CultureInfo.InvariantCulture));
    }
  }
}
