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
using System.Runtime.Serialization;
using InjectionCop.Utilities.ReSharperAnnotations;

namespace InjectionCop.Utilities
{
  /// <summary>
  /// This exception is thrown if an argument is empty although it must have a content.
  /// </summary>
  [Serializable]
  public class ArgumentEmptyException : ArgumentException
  {
    public ArgumentEmptyException ([InvokerParameterName] string paramName)
      : base (FormatMessage (paramName), paramName)
    {
    }

    public ArgumentEmptyException (SerializationInfo info, StreamingContext context)
        : base (info, context)
    {
    }

    private static string FormatMessage (string paramName)
    {
      return string.Format ("Parameter '{0}' cannot be empty.", paramName);
    }
  }
}
