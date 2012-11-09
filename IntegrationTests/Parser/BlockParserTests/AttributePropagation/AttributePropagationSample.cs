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

namespace InjectionCop.IntegrationTests.Parser.BlockParserTests.AttributePropagation
{
  class AttributePropagationSample: ParserSampleBase
  {
    public void SafeCallOfSqlFragmentCallee()
    {
      RequiresSqlFragment (SafeSource());
    }

    public string UnsafeCallOfSqlFragmentCallee()
    {
      return RequiresSqlFragment (UnsafeSource());
    }

    public string SafeCallOfMixedCallee()
    {
      return RequiresSqlFragment ("literal", UnsafeSource(), SafeSource());
    }

    public void UnsafeCallOfMixedCallee()
    {
      RequiresSqlFragment ("literal", SafeSource(), UnsafeSource());
    }
  }
}
