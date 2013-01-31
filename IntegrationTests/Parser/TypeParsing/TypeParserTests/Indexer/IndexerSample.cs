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
using InjectionCop.Attributes;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Indexer
{
  public class IndexerSample: ParserSampleBase
  {
    [return: Fragment("IndexerFragment")]
    public string[] IndexerSafeAssignmentOnParameter ([Fragment("IndexerFragment")] string[] arrayParameter)
    {
      arrayParameter[0] = SafeIndexerFragmentSource();
      return arrayParameter;
    }

    [return: Fragment("IndexerFragment")]
    public string[] IndexerUnsafeAssignmentOnParameter ([Fragment("IndexerFragment")] string[] arrayParameter)
    {
      arrayParameter[0] = UnsafeSource();
      return arrayParameter;
    }

    public void SafeCallUsingIndexer ([Fragment("IndexerFragment")] string[] arrayParameter)
    {
      RequiresIndexerFragment (arrayParameter[0]);
    }

    public void UnsafeCallUsingIndexer (string[] arrayParameter)
    {
      RequiresIndexerFragment (arrayParameter[0]);
    }

    public void UnsafeCallWithElementSetUnsafeByIndexer ([Fragment("IndexerFragment")] string[] arrayParameter)
    {
      arrayParameter[1] = UnsafeSource();
      RequiresIndexerFragment (arrayParameter[0]);
    }

    public void UnsafeCallWithSingleVariableSetSafeByIndexer (string[] arrayParameter)
    {
      arrayParameter[1] = SafeIndexerFragmentSource();
      RequiresIndexerFragment (arrayParameter[0]);
    }

    public void SafeCallUsingArray ()
    {
      string[] safeArray = SafeIndexerFragmentArraySource();
      RequiresIndexerFragment (safeArray[0]);
    }

    public void UnsafeCallUsingIndexerArray ()
    {
      string[] unsafeArray = new []{"unsafe"};
      RequiresIndexerFragment (unsafeArray[0]);
    }

    public void SafeLocallyInitializedArray ()
    {
      string[] safe = { "safe", SafeIndexerFragmentSource() };
      RequiresIndexerFragmentArray (safe);
    }
    
    public void UnsafeLocallyInitializedArray ()
    {
      string[] unsafeArray = { "dummy", UnsafeSource() };
      RequiresIndexerFragmentArray (unsafeArray);
    }

    public void UnsafeLocallyInitializedArrayFirstIndexerAssignmentUnsafe()
    {
      string[] unsafeArray = { UnsafeSource(), "dummy" };
      RequiresIndexerFragmentArray(unsafeArray);
    }

    public void SafeLocallyInitializedArrayEqualFragmentTypes()
    {
      string[] safeArray = { SafeIndexerFragmentSource(), SafeIndexerFragmentSource() };
      RequiresIndexerFragmentArray(safeArray);
    }

    public void UnsafeLocallyInitializedArrayDifferentFragmentTypes()
    {
      string[] unSafeArray = { ReturnsHtmlFragment(), SafeIndexerFragmentSource() };
      RequiresIndexerFragmentArray(unSafeArray);
    }

    public void SafeLocallyInitializedBiggerArray()
    {
      string[] safeArray = { "literal", SafeIndexerFragmentSource(), "literal", SafeIndexerFragmentSource(), "literal" };
      RequiresIndexerFragmentArray(safeArray);
    }
    
    [return: Fragment("IndexerFragment")]
    private string SafeIndexerFragmentSource ()
    {
      return "safe";
    }

    [return: Fragment("IndexerFragment")]
    private string[] SafeIndexerFragmentArraySource ()
    {
      return new[] {"safe"};
    }

    private void RequiresIndexerFragment ([Fragment ("IndexerFragment")] string indexerFragment)
    {
      DummyMethod (indexerFragment);
    }

    private void RequiresIndexerFragmentArray ([Fragment ("IndexerFragment")] string[] safe)
    {
      DummyMethod (safe[0]);
    }
  }
}
