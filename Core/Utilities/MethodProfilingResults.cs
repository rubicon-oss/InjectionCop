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
using System.Collections.Generic;

namespace InjectionCop.Utilities
{
  public class MethodProfilingResults
  {
    private List<KeyValuePair<string, TimeSpan>> _results;
    private int _outputEntries;

    public MethodProfilingResults (int outputEntries)
    {
      _outputEntries = outputEntries;
      _results = new List<KeyValuePair<string, TimeSpan>>();
    }

    public void Add (string fullName, TimeSpan elapsed)
    {
      _results.Add (new KeyValuePair<string, TimeSpan> (fullName, elapsed));
    }

    public void Write ()
    {
      SortResults();
      Console.WriteLine ("### BEGIN PROFILING RESULTS ###");
      for (int i = 0; i < _outputEntries && i < _results.Count; i++)
      {
        Console.WriteLine (Convert (_results[i]));
      }
      Console.WriteLine ("### END PROFILING RESULTS ###");
    }

    private string Convert (KeyValuePair<string, TimeSpan> entry)
    {
      return entry.Value.TotalSeconds + "s: " + entry.Key;
    }

    private void SortResults ()
    {
      _results.Sort (
          (pairA, pairB) => pairA.Value < pairB.Value
                                ? 1
                                : pairA.Value == pairB.Value
                                      ? 0
                                      : -1);
    }
  }
}
