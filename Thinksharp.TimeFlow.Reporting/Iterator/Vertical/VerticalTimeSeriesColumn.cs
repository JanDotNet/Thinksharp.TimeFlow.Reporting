using System;

namespace Thinksharp.TimeFlow.Reporting.Iterator.Vertical
{
  public class VerticalTimeSeriesColumn : Column
  {
    private readonly TimeFrame timeFrame;

    public VerticalTimeSeriesColumn(TimeFrame timeFrame, TimeSeriesRecord record) : base(record.Format)
    {
      this.timeFrame = timeFrame;
      Record = record;
    }
    public TimeSeriesRecord Record { get; }

    public override object? GetCellValue(Row row)
    {
      switch (row)
      {
        case VerticalHeaderRow r:
          return Record.Header;
        case VerticalTimePointRow r:
          return timeFrame[Record.Key][r.TimePoint];
        default:
          throw new InvalidOperationException($"Row type '{row.GetType().Name} is not supported.");
      }
    }

    public override string? GetValueFormat(Row row)
    {
      return Record.ValueFormat ?? row?.GetValueFormat();
    }
  }
}