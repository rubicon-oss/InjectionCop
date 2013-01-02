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

namespace InjectionCop.IntegrationTests.Parser.BlockParsing
{
  internal class BlockParserSample : ParserSampleBase
  {
    public void PostConditionOnlySafeSymbols ()
    {
      int x = SafeSourceInt();
      string y = "safe";
      doSomething (x, y);
    }

    public void PostConditionSafeAndUnsafeSymbols()
    {
      int x = 0;
      string y = UnsafeSource();
      doSomething (x, y);
    }

    public void UnsafePreCondition(string unSafe)
    {
      RequiresSqlFragment (unSafe);
    }

    public void SafePreCondition(string ignore)
    {
      RequiresSqlFragment ("safe");
    }

    public void MultipleUnsafePreCondition(string unSafe1, string unSafe2)
    {
      RequiresSqlFragment (unSafe1);
      RequiresSqlFragment (unSafe2);
    }

    public void BlockInternalSafenessCondition (string x)
    {
      String y = "Safe";
      RequiresSqlFragment (x);
      RequiresSqlFragment (y);
    }

    [return : Fragment("ReturnFragmentType")]
    public string ReturnFragmentRequiredLiteralReturn ()
    {
      return "dummy";
    }

    public string UnsafeReturnWhenFragmentRequired ()
    {
      return UnsafeSource();
    }

    public string UnsafeReturnWhenFragmentRequiredMoreComplex ()
    {
      int piIstGenauDrei = 3;
      string dummy = "dummy";
      doSomething (piIstGenauDrei, dummy);
      return UnsafeSource();
    }

    public void DummyProcedure ()
    {
      int piIstGenauDrei = 3;
      string dummy = "dummy";
      doSomething (piIstGenauDrei, dummy);
    }

    public string SetSuccessor(string param)
    {
      param += "dummy";
      return param;
    }

    public string doSomething (int x, string y)
    {
      return x + y;
    }

    [return:Fragment("SqlFragment")]
    public int SafeSourceInt()
    {
      return 3;
    }

    [return: Fragment ("DummyFragment")]
    public string ValidReturnWithLiteralAssignmentInsideIf ([Fragment ("DummyFragment")] string parameter)
    {
      string returnValue = "";
      if (parameter == "Dummy")
      {
        returnValue = "safe";
      }
      return returnValue;
    }

    [return: Fragment("ReturnFragmentType")]
    public int DeclarationWithReturn()
    {
      int i = 3;
      return i;
    }

    [return: Fragment ("DummyFragment")]
    public string ValidReturnWithParameterAssignmentInsideIf ([Fragment ("DummyFragment")] string parameter)
    {
      string returnValue = "";
      if (parameter == "Dummy")
      {
        returnValue = parameter;
      }
      return returnValue;
    }

    [return: Fragment ("DummyFragment")]
    public string ValidReturnWithParameterResetAndAssignmentInsideIf ([Fragment ("DummyFragment")] string parameter)
    {
      string returnValue = "";
      if (parameter == "Dummy")
      {
        parameter = "safe";
        returnValue = parameter;
      }
      return returnValue;
    }

    public string ReturnLiteral ()
    {
      return "dummy";
    }

    public void ReturnPreconditionCheckSafe (out string returnPreCondition)
    {
      returnPreCondition = "safe";
    }

    public void ReturnPreconditionCheckUnSafe (out string returnPreCondition)
    {
      returnPreCondition = UnsafeSource();
    }

    public void ReturnPreconditionCheckSafeLiteralAssignment (out string returnPreCondition)
    {
      returnPreCondition = UnsafeSource();
      DummyMethod (returnPreCondition);
      returnPreCondition = "safe";
    }

    public void ReturnPreconditionCheckSafeFragmentAssignment (out string returnPreCondition)
    {
      returnPreCondition = UnsafeSource();
      DummyMethod (returnPreCondition);
      returnPreCondition = "safe";
    }

    public void ReturnPreconditionConditional (out string returnPreCondition)
    {
      string temp = "safe";
      returnPreCondition = UnsafeSource();
      if (returnPreCondition == "dummy")
      {
        returnPreCondition = temp;
      }
    }

    public void ReturnPreconditionConditionalWithReturnInsideIf (out string returnPreCondition)
    {
      string temp = "safe";
      returnPreCondition = UnsafeSource();
      if (returnPreCondition == "dummy")
      {
        returnPreCondition = temp;
        return;
      }
      DummyMethod (returnPreCondition);
    }

    public void ReturnPreconditionConditionalWithReturnAfterIf (out string returnPreCondition)
    {
      string temp = "safe";
      returnPreCondition = UnsafeSource();
      if (returnPreCondition == "dummy")
      {
        returnPreCondition = temp;
        DummyMethod (returnPreCondition);
      }
      DummyMethod (returnPreCondition);
      
    }
    
  }
}
