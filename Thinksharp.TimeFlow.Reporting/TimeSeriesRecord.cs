using System.Collections.Generic;

namespace Thinksharp.TimeFlow.Reporting
{
  public class TimeSeriesRecord : Record
  {
    public TimeSeriesRecord(string key, string header, string? valueFormat = null)
      : base(key, header, valueFormat)
    {
    }

    public Dictionary<string, string> AggregationFormula { get; } = new Dictionary<string, string>();
  }
}