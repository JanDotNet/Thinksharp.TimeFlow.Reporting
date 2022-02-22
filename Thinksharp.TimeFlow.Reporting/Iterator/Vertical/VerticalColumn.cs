namespace Thinksharp.TimeFlow.Reporting.Iterator.Vertical
{
  internal abstract class VerticalColumn : Column
  {
    internal VerticalColumn(int number, Format columnFormat, Format? format) : base(number, columnFormat, format)
    {
    }

    public override Format GetFormat(Row row)
    {
      switch (row)
      {
        case VerticalHeaderRow r:
          return this.ColumnFormat;
        default:
          return this.Format ?? row.Format;
      }
    }
  }
}
