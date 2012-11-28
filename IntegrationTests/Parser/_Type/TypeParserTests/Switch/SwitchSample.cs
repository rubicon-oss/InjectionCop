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

namespace InjectionCop.IntegrationTests.Parser._Type.TypeParserTests.Switch
{
  class SwitchSample : ParserSampleBase
  {
    public void ValidSwitch (int i)
    {
      switch (i)
      {
        case 1:
          RequiresSqlFragment ("safe");
          break;
        case 2:
          RequiresSqlFragment (SafeSource());
          break;
        default:
          RequiresSqlFragment ("safe");
          break;
      }
    }

    public void UnsafeCallInsideSwitch (int i)
    {
      switch (i)
      {
        case 1:
          RequiresSqlFragment ("safe");
          break;
        case 2:
          RequiresSqlFragment (UnsafeSource());
          break;
        default:
          RequiresSqlFragment ("safe");
          break;
      }
    }

    public void UnsafeCallAfterSwitch (int i)
    {
      string param;
      switch (i)
      {
        case 1:
          param = "safe";
          break;
        case 2:
          param = UnsafeSource();
          break;
        default:
          param = "safe";
          break;
      }
      RequiresSqlFragment (param);
    }

    public void UnsafeCallAfterNestedSwitch (int i, int j)
    {
      string param;
      switch (i)
      {
        case 1:
          param = "safe";
          break;
        case 2:
          switch (j)
          {
            case 1:
              param = "safe";
              break;
            default:
              param = UnsafeSource();
              break;
          }
          break;
        default:
          param = "safe";
          break;
      }
      RequiresSqlFragment (param);
    }

    public void SafeCallAfterNestedSwitch (int i, int j)
    {
      string param;
      switch (i)
      {
        case 1:
          param = "safe";
          break;
        case 2:
          switch (j)
          {
            case 1:
              param = "safe";
              break;
            default:
              param = UnsafeSource();
              break;
          }
          DummyMethod (param);
          param = SafeSource();
          break;
        default:
          param = "safe";
          break;
      }
      RequiresSqlFragment (param);
    }

    public void UnsafeCallInsideNestedSwitch (int i, int j)
    {
      string param;
      switch (i)
      {
        case 1:
          param = "safe";
          break;
        case 2:
          switch (j)
          {
            case 1:
              param = "safe";
              break;
            default:
              param = UnsafeSource();
              break;
          }
          RequiresSqlFragment (param);
          break;
        default:
          param = "safe";
          break;
      }
      DummyMethod (param);
    }

    public void ValidFallThrough (int i)
    {
      switch (i)
      {
        case 1:
        case 2:
          RequiresSqlFragment (SafeSource());
          break;
        default:
          RequiresSqlFragment ("safe");
          break;
      }
    }

    public void ValidFallThroughGoto (int i)
    {
      switch (i)
      {
        case 1:
          RequiresSqlFragment ("safe");
          goto case 2;
        case 2:
          RequiresSqlFragment (SafeSource());
          break;
        default:
          RequiresSqlFragment ("safe");
          break;
      }
    }

    public void InvalidFallThrough (int i)
    {
      switch (i)
      {
        case 1:
        case 2:
          RequiresSqlFragment (UnsafeSource());
          break;
        default:
          RequiresSqlFragment ("safe");
          break;
      }
    }

    public void InvalidFallThroughGoto (int i)
    {
      switch (i)
      {
        case 1:
          RequiresSqlFragment ("safe");
          goto case 2;
        case 2:
          RequiresSqlFragment (UnsafeSource());
          break;
        default:
          RequiresSqlFragment ("safe");
          break;
      }
    }
  }
}
