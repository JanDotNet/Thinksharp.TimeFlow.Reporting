using System;

namespace Thinksharp.TimeFlow.Reporting.Iterator.Horizontal
{
  public class HorizontalTimeSeriesColumn : Column
  {
    public HorizontalTimeSeriesColumn(Format? format) : base(format)
    {

    }

    public override object? GetCellValue(Row row)
    {
      switch (row)
      {
        case HorizontalTimePointAxisRow axisRow:
          return null;
        case RecordDataRow<TimeSeriesRecord> r:
          return r.Record.Header;
        case RecordDataRow<HeaderRecord> r:
          return r.Record.Header;
        default:
          throw new InvalidOperationException($"{row.GetType().Name} is not supported.");
      }
    }

    public override string? GetValueFormat(Row row)
    {
      switch (row)
      {
        case HorizontalTimePointAxisRow axisRow:
          return null;
        case RecordDataRow<TimeSeriesRecord> r:
          return r.Record.ValueFormat;
        default:
          return null;
      }
    }
  }
}