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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Field
{
  internal class FieldSample : ParserSampleBase
  {
    [SqlFragment]
    private string _safeField = "safe";

    private string _unsafeField = "dummy";

    [Fragment ("AnyType")]
    private readonly string _otherSafeField = StaticUnsafeSource();

    public void CallWithSafeField ()
    {
      RequiresSqlFragment (_safeField);
    }

    public void CallWithUnsafeField ()
    {
      RequiresSqlFragment (_unsafeField);
    }

    public void CallWithWrongFragmentType ()
    {
      RequiresSqlFragment (ReturnsHtmlFragment());
    }
    
    public void NestedUnsafeCall ()
    {
      int i = 0;
      while (i++ < 5)
      {
        RequiresSqlFragment(SafeSourceRequiresSqlFragment (_unsafeField));
      }
    }

    public void NestedSafeCall ()
    {
      int i = 0;
      while (i++ < 5)
      {
        RequiresSqlFragment(SafeSourceRequiresSqlFragment ("safe"));
      }
    }

    public void NestedCallWithWrongFragmentType ()
    {
      int i = 0;
      while (i++ < 5)
      {
        RequiresSqlFragment(SafeSourceRequiresSqlFragment (ReturnsHtmlFragment()));
      }
    }

    public void UnsafeFieldAssignment ()
    {
      _safeField = UnsafeSource();
    }

    public void SafeFieldAssignment ()
    {
      _safeField = SafeSource();
    }

    public void SafeFieldAssignmentWithLiteral ()
    {
      _safeField = "safe";
    }

    public void WrongFragmentTypeFieldAssignment ()
    {
      _safeField = ReturnsHtmlFragment();
    }

    public static string StaticUnsafeSource ()
    {
      return "unsafe";
    }

    public string Dummy ()
    {
      return _unsafeField + _otherSafeField;
    }
  }
}
