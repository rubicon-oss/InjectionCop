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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.AddProblem
{
  class AddProblemSample : ParserSampleBase
  {
    public void NoViolation ()
    {
      RequiresSqlFragment(SafeSource());
    }

    public void OneViolation ()
    {
      RequiresSqlFragment (UnsafeSource());
    }

    public void TwoViolations ()
    {
      RequiresSqlFragment (UnsafeSource());
      RequiresSqlFragment (UnsafeSource());
    }

    public void OneViolationInWhile ()
    {
      int i = 0;
      string source = UnsafeSource();
      while (i < 10)
      {
        RequiresSqlFragment (source);
        i++;
      }
    }

    public void OneViolationInWhileAssignmentAfterCall ()
    {
      int i = 0;
      string source = "safe";
      while (i < 10)
      {
        RequiresSqlFragment (source);
        source = UnsafeSource();
        i++;
      }
    }

    public void TwoViolationsInNestedWhile ()
    {
      int i = 0;
      string sqlSource = UnsafeSource();
      while (i < 10)
      {
        int source = 0;
        while (i < 5)
        {
          RequiresSqlFragment (sqlSource);
          RequiresValidatedFragment (source);
          source = UnsafeIntSource();
          i++;
        }
        i++;
      }
    }

    public int RequiresValidatedFragment ([Fragment("Validated")] int source)
    {
      return source;
    }

    public int UnsafeIntSource ()
    {
      return 0;
    }
  }
}
