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
using InjectionCop.Utilities.ReSharperAnnotations;

namespace InjectionCop.Utilities
{
  /// <summary>
  /// This utility class provides methods for checking arguments.
  /// </summary>
  /// <remarks>
  /// The input value is returned unmodified. This makes it easier to use the argument checks in calls to base class constructors
  /// or property setters:
  /// <code><![CDATA[
  /// class MyType : MyBaseType
  /// {
  ///   public MyType (string name) : base (ArgumentUtility.CheckNotNullOrEmpty ("name", name))
  ///   {
  ///   }
  /// 
  ///   public override Name
  ///   {
  ///     set { base.Name = ArgumentUtility.CheckNotNullOrEmpty ("value", value); }
  ///   }
  /// }
  /// ]]></code>
  /// </remarks>
  public static class ArgumentUtility
  {
    [AssertionMethod]
    public static T CheckNotNull<T> ([InvokerParameterName] string argumentName, [AssertionCondition (AssertionConditionType.IS_NOT_NULL)] T actualValue)
    {
      // ReSharper disable CompareNonConstrainedGenericWithNull
      if (actualValue == null)
          // ReSharper restore CompareNonConstrainedGenericWithNull
        throw new ArgumentNullException (argumentName);

      return actualValue;
    }

    // Copied from Remotion.Linq.Utilities.ArgumentUtility
    [AssertionMethod]
    public static string CheckNotNullOrEmpty ([InvokerParameterName] string argumentName, [AssertionCondition (AssertionConditionType.IS_NOT_NULL)] string actualValue)
    {
      CheckNotNull (argumentName, actualValue);
      if (actualValue.Length == 0)
        throw new ArgumentEmptyException (argumentName);

      return actualValue;
    }
  }
}
