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

using InjectionCop.Attributes;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.FragmentUsageTests
{
  public class DerivedClassWithInvalidFragmentUsage : BaseClass
  {
    public override void Foo ([Fragment("InvalidFragmentType")]int parameter1, string parameter2)
    {
    }

    public override void MethodWithFragmentOnSecondParameter (int parameter1, [Fragment("InvalidFragmentTypeOnSecondParameter")] int parameter2)
    {
    }
  }
}