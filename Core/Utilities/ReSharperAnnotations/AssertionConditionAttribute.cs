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

namespace InjectionCop.Utilities.ReSharperAnnotations
{
  /// <summary>
  /// Indicates the condition parameter of the assertion method. 
  /// The method itself should be marked by <see cref="AssertionConditionType"/> attribute.
  /// The mandatory argument of the attribute is the assertion type.
  /// </summary>
  /// <seealso cref="AssertionMethodAttribute"/>
  [AttributeUsage (AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
  public sealed class AssertionConditionAttribute : Attribute
  {
    private readonly AssertionConditionType myConditionType;

    /// <summary>
    /// Initializes new instance of AssertionConditionAttribute
    /// </summary>
    /// <param name="conditionType">Specifies condition type</param>
    public AssertionConditionAttribute (AssertionConditionType conditionType)
    {
      myConditionType = conditionType;
    }

    /// <summary>
    /// Gets condition type
    /// </summary>
    public AssertionConditionType ConditionType
    {
      get { return myConditionType; }
    }
  }
}