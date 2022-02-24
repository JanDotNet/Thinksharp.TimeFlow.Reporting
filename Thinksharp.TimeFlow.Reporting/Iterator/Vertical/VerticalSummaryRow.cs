using System;
using System.Linq;
using Thinksharp.TimeFlow.Reporting.Calculation;

namespace Thinksharp.TimeFlow.Reporting.Iterator.Vertical
{
  internal class VerticalSummaryRow : Row
  {
    private readonly SummaryResult[] summaryResults;

    public VerticalSummaryRow(int number, Summary summary, SummaryResult[] summaryResults) : base(number)
    {
      Summary = summary;
      this.summaryResults = summaryResults;
      this.Format = summary.Format;
    }

    public Summary Summary { get; }

    public object? GetValue(string recordKey) => this.summaryResults.FirstOrDefault(x => x.SummaryKey == this.Summary.Key && x.RecordKey == recordKey)?.Value;
  }
}