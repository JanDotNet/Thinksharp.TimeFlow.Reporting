namespace Thinksharp.TimeFlow.Reporting.Iterator.Horizontal
{
  internal class HorizontalSummaryColumn : Column
  {
    public HorizontalSummaryColumn(int number, Summary summary, Format? format) : base(number, format)
    {
      Summary = summary;
    }

    public Summary Summary { get; }

    public override object GetCellValue(Row row)
    {
      // TODO
      return Summary.Header;
    }

    public override string? GetValueFormat(Row row) => Summary.ValueFormat ?? row?.GetValueFormat();
  }
}