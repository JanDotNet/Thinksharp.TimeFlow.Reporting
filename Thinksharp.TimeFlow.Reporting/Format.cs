namespace Thinksharp.TimeFlow.Reporting
{
  public class Format
  {
    public bool Bold { get; set; }
    public HorizontalAlignment HorizontalAlignment { get; set; }
    public ReportColor Background { get; set; } = ReportColor.White;
    public ReportColor Foreground { get; set; } = ReportColor.Black;

    public static Format Default() => new Format();
  }
}