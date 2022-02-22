using System;

namespace Thinksharp.TimeFlow.Reporting.Iterator.Vertical
{
  internal class VerticalHeaderColumn : VerticalColumn
  {
    public VerticalHeaderColumn(int number, HeaderRecord record, Format format) : base(number, format, record.Format)
    {
      Record = record;
    }
    public HeaderRecord Record { get; }

    public override object? GetCellValue(Row row)
    {
      switch (row)
      {
        case VerticalHeaderRow r:
          return Record.Header;
        case VerticalTimePointRow r:
          return null;
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