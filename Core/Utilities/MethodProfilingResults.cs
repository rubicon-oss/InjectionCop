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
using System.Text;

namespace InjectionCop.Utilities
{
  public class MethodProfilingResults
  {
    private const int c_MaxEntries = 10;
    private const double c_Epsilon = 1.0;
    
    private readonly List<KeyValuePair<string, TimeSpan>> _results;
    

    public MethodProfilingResults ()
    {
      _results = new List<KeyValuePair<string, TimeSpan>>();
    }

    public void Add (string fullName, TimeSpan elapsed)
    {
      if (elapsed.TotalMilliseconds > c_Epsilon)
      {
        _results.Add (new KeyValuePair<string, TimeSpan>(fullName, elapsed));
        SortResults();
        TrimResults();
      }
    }
    
    public override string ToString ()
    {
      StringBuilder profilingResults = new StringBuilder ("--- PROFILING RESULTS ---");
      profilingResults.Append (System.Environment.NewLine);
      foreach (var entry in _results)
      {
        profilingResults.Append (Convert(entry));
          profilingResults.Append (System.Environment.NewLine);
      }
      profilingResults.Append ("--- END PROFILING RESULTS ---");
      profilingResults.Append (System.Environment.NewLine);
      return profilingResults.ToString();
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

    private void TrimResults ()
    {
      if (_results.Count > c_MaxEntries)
      {
        _results.RemoveRange (c_MaxEntries, _results.Count - c_MaxEntries);
      }
    }

    private string Convert (KeyValuePair<string, TimeSpan> entry)
    {
      return entry.Value.TotalSeconds + " s: " + entry.Key;
    }
  }
}
