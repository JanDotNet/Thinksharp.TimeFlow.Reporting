using System;

namespace Thinksharp.TimeFlow.Reporting.Iterator.Vertical
{
  public class VerticalRecordColumn : Column
  {
    private readonly TimeFrame timeFrame;

    public VerticalRecordColumn(int number, TimeFrame timeFrame, Record record) : base(number, record.Format)
    {
      this.timeFrame = timeFrame;
      Record = record;
    }
    public Record Record { get; }

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