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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.NestedCall
{
  class NestedCallSample : ParserSampleBase
  {
    public bool NestedValidCallReturn ()
    {
      return RequiresSqlFragmentReturnsBool ("safe");
    }

    public bool NestedInvalidCallReturn ()
    {
      return RequiresSqlFragmentReturnsBool (UnsafeSource());
    }

    public void NestedInvalidCall ()
    {
      RequiresSqlFragment (UnsafeSource());
    }

    public bool DeeperNestedInvalidCall ()
    {
      return "dummy" == RequiresSqlFragment (UnsafeSource());
    }

    public void ValidMethodCallChain ()
    {
      RequiresSqlFragment (SafeSourceRequiresSqlFragment (SafeSourceRequiresSqlFragment("safe")), "safe", "safe");
    }

    public void InvalidMethodCallChain ()
    {
      RequiresSqlFragment (SafeSourceRequiresSqlFragment (SafeSourceRequiresSqlFragment(UnsafeSource())), "safe", "safe");
    }

    public void ValidMethodCallChainDifferentOperand ()
    {
      RequiresSqlFragment ("safe", "safe", SafeSourceRequiresSqlFragment (SafeSourceRequiresSqlFragment("safe")));
    }

    public void InvalidMethodCallChainDifferentOperand ()
    {
      RequiresSqlFragment ("safe", "safe", SafeSourceRequiresSqlFragment (SafeSourceRequiresSqlFragment(UnsafeSource())));
    }
  }
}
