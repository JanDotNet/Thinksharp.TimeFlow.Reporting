namespace Thinksharp.TimeFlow.Reporting.Iterator.Vertical
{
  internal abstract class VerticalColumn : Column
  {
    internal VerticalColumn(int number, Format columnFormat, Format format) : base(number, columnFormat, format)
    {
    }

    public override Format GetFormat(Row row)
    {
      switch (row)
      {
        case VerticalSummaryRow r:
          return r.Format.Merge(this.Format);
        case VerticalHeaderRow r:
          return this.ColumnFormat;
        default:
          return this.Format.Merge(row.Format);
      }
    }
  }
}
