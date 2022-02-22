namespace Thinksharp.TimeFlow.Reporting.Iterator.Horizontal
{
  internal class HorizontalSummaryColumn : HorizontalColumn
  {
    public HorizontalSummaryColumn(int number, Summary summary, Format columnFormat) : base(number, columnFormat, summary.Format)
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