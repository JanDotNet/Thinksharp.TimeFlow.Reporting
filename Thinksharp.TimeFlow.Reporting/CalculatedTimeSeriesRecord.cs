using System.Collections.Generic;

namespace Thinksharp.TimeFlow.Reporting
{
  public class CalculatedTimeSeriesRecord : Record
  {
    public CalculatedTimeSeriesRecord(string key, string header, string formula, string? valueFormat = null)
      : base(key, header, valueFormat)
    {
      Formula = formula;
    }

    public string Formula { get; }
  }
}