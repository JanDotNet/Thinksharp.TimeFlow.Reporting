using System.Collections.Generic;

namespace Thinksharp.TimeFlow.Reporting
{
  public class CalculatedRecord : Record
  {
    public CalculatedRecord(string key, string header, string formula)
      : base(key, header)
    {
      Formula = formula;
    }

    public string Formula { get; }

    public Dictionary<string, string> AggrgeationFormula { get; } = new Dictionary<string, string>();
  }
}