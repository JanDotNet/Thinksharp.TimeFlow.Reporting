namespace Thinksharp.TimeFlow.Reporting.Iterator.Horizontal
{
  internal abstract class HorizontalColumn : Column
  {
    internal HorizontalColumn(int number, Format columnFormat, Format format) : base(number, columnFormat, format)
    {
    }

    public override Format GetFormat(Row row)
    {
      switch (row)
      {
        case HorizontalTimePointAxisRow r:
          return this.ColumnFormat;
        default:
          return this.Format.Merge(row.Format);
      }
    }
  }
}
