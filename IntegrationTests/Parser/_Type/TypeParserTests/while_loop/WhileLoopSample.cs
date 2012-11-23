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

namespace InjectionCop.IntegrationTests.Parser._Type.TypeParserTests.While_loop
{
  internal class WhileLoopSample : ParserSampleBase
  {
    public void ValidCallInsideWhile ()
    {
      int i = 10;
      while (i > 0)
      {
        RequiresSqlFragment ("safe");
        i--;
      }
    }

    public void InValidCallInsideWhile ()
    {
      int i = 10;
      while (i > 0)
      {
        RequiresSqlFragment (UnsafeSource());
        i--;
      }
    }

    public void InValidCallInsideWhileReprocessingRequired ()
    {
      int i = 10;
      string parameter = SafeSource();
      while (i > 0)
      {
        RequiresSqlFragment (parameter);
        parameter = UnsafeSource();
        i--;
      }
    }

    public void InvalidCallInsideNestedWhile ()
    {
      int i = 10;
      while (i > 0)
      {
        while (i > 5)
        {
          RequiresSqlFragment (UnsafeSource());
          i--;
        }
        i--;
      }
    }

    public void InValidCallInsideNestedWhileReprocessingRequired ()
    {
      int i = 10;
      string parameter = SafeSource();
      while (i > 0)
      {
        while (i > 5)
        {
          RequiresSqlFragment (parameter);
          i--;
        }
        parameter = UnsafeSource();
        i--;
      }
    }

    public void InValidCallInsideDeeperNestedWhileReprocessingRequired ()
    {
      int i = 10;
      string parameter = SafeSource();
      while (i > 0)
      {
        while (i > 5)
        {
          while (i > 7)
          {
            RequiresSqlFragment (parameter);
            i--;
          }
          i--;
        }
        parameter = UnsafeSource();
        i--;
      }
    }
    
    public void ValidCallInsideWhileWithContinue ()
    {
      int i = 10;
      string x = "safe";
      while (i > 0)
      {
        RequiresSqlFragment (x);
        if (i != 3)
        {
          i--;
          continue;
        }
        i--;
      }
    }

    public void InvalidCallInsideWhileWithContinue ()
    {
      int i = 10;
      string x = "safe";
      while (i > 0)
      {
        RequiresSqlFragment (x);
        if (i != 3)
        {
          i--;
          continue;
        }
        x = UnsafeSource();
        i--;
      }
    }

    public void InvalidCallInsideIfWithContinue ()
    {
      int i = 10;
      string x = "safe";
      while (i > 0)
      {
        RequiresSqlFragment (x);
        if (i != 3)
        {
          x = UnsafeSource();
          i--;
          continue;
        }
        x = SafeSource();
        i--;
      }
    }

    public void ValidCallInsideWhileWithBreak ()
    {
      int i = 10;
      string x = "safe";
      while (i > 0)
      {
        if (i != 3)
        {
          break;
        }
        i--;
      }
      RequiresSqlFragment (x);
    }

    public void InvalidCallInsideWhileWithBreak ()
    {
      int i = 10;
      string x = "safe";
      while (i > 0)
      {
        if (i != 3)
        {
          break;
        }
        x = UnsafeSource();
        i--;
      }
      RequiresSqlFragment (x);
    }

    public void InvalidCallInsideIfWithBreak ()
    {
      int i = 10;
      string x = "safe";
      while (i > 0)
      {
        if (i != 3)
        {
          x = UnsafeSource();
          break;
        }
        x = SafeSource();
        i--;
      }
      RequiresSqlFragment (x);
    }

    public void InValidCallInsideWhileCondition ()
    {
      while (RequiresSqlFragmentReturnsBool (UnsafeSource()))
      {
        RequiresSqlFragment ("safe");
      }
    }
  }
}
