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

namespace InjectionCop.IntegrationTests.Parser.If
{
  class IfSample: TypeParserSample
  {
    public void ValidExampleInsideIf()
    {
      if(true)
      {
        RequiresSqlFragment("safe");
      }
    }

    public void InvalidExampleInsideIf(int x, int y)
    {
      if(x == y)
      {
        RequiresSqlFragment(UnsafeSource());
      }
    }

    public void InvalidExampleInsideElse(int x, int y)
    {
      if(x==y)
      {
        string temp = "";
        RequiresSqlFragment (temp);
      }
      else
      {
        RequiresSqlFragment (UnsafeSource());  
      }
    }

    public void UnsafeAssignmentInsideIf(int x, int y)
    {
      string temp;
      if(x==y)
      {
        temp = UnsafeSource();
      }
      else
      {
        temp = "safe";
      }
      RequiresSqlFragment (temp);
    }
  }
}
