using System;

namespace Thinksharp.TimeFlow.Reporting.Iterator.Horizontal
{
  internal class HorizontalTimeSeriesColumn : HorizontalColumn
  {
    public HorizontalTimeSeriesColumn(int number, Format columnFormat, Format format) : base(number, columnFormat, format)
    {

    }

    public override object? GetCellValue(Row row)
    {
      switch (row)
      {
        case HorizontalTimePointAxisRow axisRow:
          return null;
        case RecordDataRow r:
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
        case RecordDataRow r:
          return r.Record.ValueFormat;
        default:
          return null;
      }
    }
  }
}