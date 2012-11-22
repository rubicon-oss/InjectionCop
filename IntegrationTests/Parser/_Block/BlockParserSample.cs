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
using InjectionCop.Attributes;

namespace InjectionCop.IntegrationTests.Parser._Block
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
  }
}
