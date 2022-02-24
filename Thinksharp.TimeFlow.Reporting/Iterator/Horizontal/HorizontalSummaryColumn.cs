using System;
using System.Linq;
using Thinksharp.TimeFlow.Reporting.Calculation;

namespace Thinksharp.TimeFlow.Reporting.Iterator.Horizontal
{
  internal class HorizontalSummaryColumn : HorizontalColumn
  {
    private readonly SummaryResult[] summaryResults;

    public HorizontalSummaryColumn(int number, Summary summary, Format columnFormat, SummaryResult[] summaryResults) : base(number, columnFormat, summary.Format)
    {
      Summary = summary;
      this.summaryResults = summaryResults;
    }

    public Summary Summary { get; }

    public override object? GetCellValue(Row row)
    {
      switch (row)
      {
        case HorizontalTimePointAxisRow axisRow:
          return this.Summary.Header;
        case RecordDataRow r:
          return this.summaryResults.FirstOrDefault(x => x.SummaryKey == this.Summary.Key && x.RecordKey == r.Record.Key)?.Value;
        default:
          throw new InvalidOperationException($"Row type '{row.GetType().Name} is not supported.");
      }
    }

    public override string? GetValueFormat(Row row) => Summary.ValueFormat ?? row?.GetValueFormat();
  }
}