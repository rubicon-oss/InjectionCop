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

namespace InjectionCop.IntegrationTests.Parser.BlockParserTests.If
{
  internal class IfSample : ParserSampleBase
  {
    public void ValidExampleInsideIf (int x, int y)
    {
      if (x == y)
      {
        RequiresSqlFragment ("safe");
      }
    }

    public void InvalidExampleInsideIf (int x, int y)
    {
      if (x == y)
      {
        RequiresSqlFragment (UnsafeSource());
      }
    }

    public void InvalidExampleInsideElse (int x, int y)
    {
      if (x == y)
      {
        string temp = "";
        RequiresSqlFragment (temp);
      }
      else
      {
        RequiresSqlFragment (UnsafeSource());
      }
    }

    public void UnsafeAssignmentInsideIf (int x, int y)
    {
      string temp;
      if (x == y)
      {
        temp = UnsafeSource();
      }
      else
      {
        temp = "safe";
      }
      RequiresSqlFragment (temp);
    }

    public void UnsafeAssignmentInsideIfTwisted (int x, int y)
    {
      string temp;
      if (x == y)
      {
        temp = "safe";
      }
      else
      {
        temp = UnsafeSource();
      }
      RequiresSqlFragment (temp);
    }

    public void UnsafeAssignmentInsideIfNested (int x, int y, int z)
    {
      string temp;
      if (x == z)
      {
        temp = "safe";
        if (x == y)
        {
          temp = UnsafeSource();
        }
      }
      else
      {
        temp = SafeSource();
      }
      RequiresSqlFragment (temp);
    }

    public void SafeAssignmentInsideIfNested (int x, int y, int z)
    {
      string temp;
      if (x == z)
      {
        temp = "safe";
        if (x == y)
        {
          temp = SafeSource();
        }
      }
      else
      {
        temp = SafeSource();
      }
      RequiresSqlFragment (temp);
    }

    public void UnsafeAssignmentInsideIfNestedDeeper (int x, int y, int z)
    {
      string temp = "safe";
      if (x == z)
      {
        temp = "safe";
      }
      else
      {
        if (x == y)
        {
          if (y == z)
          {
            temp = SafeSource();
          }
          else
          {
            temp = UnsafeSource();
          }
        }
      }
      RequiresSqlFragment (temp);
    }

    public void UnsafeAssignmentInsideIfNestedElse (int x, int y, int z)
    {
      string temp;
      if (x == z)
      {
        temp = "safe";
      }
      else
      {
        if (z == y)
        {
          temp = UnsafeSource();
        }
        else
        {
          temp = SafeSource();
        }
      }

      RequiresSqlFragment (temp);
    }

    public void UnsafeAssignmentInsideIfReversed (int x, int y, int z)
    {
      string temp;
      if (x == z)
      {
        // ReSharper disable RedundantAssignment
        temp = "safe";
        // ReSharper restore RedundantAssignment
        if (x == y)
        {
// ReSharper disable RedundantAssignment
          temp = UnsafeSource();
// ReSharper restore RedundantAssignment
        }
        temp = "safe again";
      }
      else
      {
        if (z == y)
        {
          temp = UnsafeSource();
        }
        else
        {
          temp = SafeSource();
        }
      }

      RequiresSqlFragment (temp);
    }
  }
}