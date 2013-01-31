// Copyright 2013 rubicon informationstechnologie gmbh
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
using System.Text;
using InjectionCop.Attributes;

namespace InjectionCop.IntegrationTests.Parser.BlockParsing.StatementHandler
{
  public class StringBuilderConstructStatementHandlerSample
  {
    public string NonStringBuilderAssignment ()
    {
      string dummy = "dummy";
      return dummy;
    }

    public StringBuilder InitializationWithEmptyConstructor ()
    {
      StringBuilder stringBuilder = new StringBuilder();
      return stringBuilder;
    }

    public StringBuilder InitializationWithCapacity ()
    {
      StringBuilder stringBuilder = new StringBuilder(128);
      return stringBuilder;
    }

    public StringBuilder InitializationWithCapacityAndMaximum ()
    {
      StringBuilder stringBuilder = new StringBuilder(128, 256);
      return stringBuilder;
    }

    public StringBuilder InitializationWithLiteral ()
    {
      StringBuilder stringBuilder = new StringBuilder("literal");
      return stringBuilder;
    }

    public StringBuilder InitializationWithFragment ()
    {
      StringBuilder stringBuilder = new StringBuilder(ReturnsStringBuilderFragment());
      return stringBuilder;
    }
    
    public StringBuilder InitializationWithEmptyFragment ()
    {
      StringBuilder stringBuilder = new StringBuilder(ReturnsEmptyFragment());
      return stringBuilder;
    }

    public StringBuilder InitializationWithLiteralAndInt ()
    {
      StringBuilder stringBuilder = new StringBuilder ("literal", 0);
      return stringBuilder;
    }

    public StringBuilder InitializationWithFragmentAndInts ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsStringBuilderFragment(), 0, 0, 0);
      return stringBuilder;
    }

    [return: Fragment ("StringBuilderFragment")]
    private string ReturnsStringBuilderFragment ()
    {
      return "safe";
    }

    private string ReturnsEmptyFragment ()
    {
      return "dummy";
    }

    // stringbuilder als parameter
  }
}
