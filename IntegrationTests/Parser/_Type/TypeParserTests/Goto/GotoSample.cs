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

namespace InjectionCop.IntegrationTests.Parser._Type.TypeParserTests.Goto
{
  class GotoSample : ParserSampleBase
  {
    public void SimpleGoto ()
    {
      string x = "safe";
      DummyMethod (x);
      if("dummy" == SafeSource())
      {
        goto End;
      }
      x = SafeSource();
      End:
      RequiresSqlFragment (x);
    }

    public void GotoJumpsOverUnsafeAssignment ()
    {
      string x = "safe";
      DummyMethod (x);
      if("dummy" == SafeSource())
      {
        goto End;
      }
      x = UnsafeSource();
      End:
      RequiresSqlFragment (x);
    }

    public void InvalidCallInsideWhileWithGoto ()
    {
      int i = 10;
      string x = "safe";
      while (i > 0)
      {
        if (i != 3)
        {
          goto Afterwhile;
        }
        x = UnsafeSource();
        i--;
      }
      Afterwhile:
      RequiresSqlFragment (x);
    }

    public void InvalidCallInsideIfWithGoto ()
    {
      int i = 10;
      string x = "safe";
      while (i > 0)
      {
        if (i != 3)
        {
          x = UnsafeSource();
          goto Afterwhile;
        }
        x = SafeSource();
        i--;
      }
      Afterwhile:
      RequiresSqlFragment (x);
    }

    public void InvalidCallInsideIfWithGotoAndBreak ()
    {
      int i = 10;
      string x = "safe";
      while (i > 0)
      {
        if (i != 3)
        {
          goto UnsafeArea;
        }
        x = SafeSource();
        break;
      UnsafeArea:
        x = UnsafeSource();
      }
      RequiresSqlFragment (x);
    }
  }
}
