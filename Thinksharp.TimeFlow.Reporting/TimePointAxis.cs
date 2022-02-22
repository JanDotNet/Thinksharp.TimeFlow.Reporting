namespace Thinksharp.TimeFlow.Reporting
{
  public class TimePointAxis
  {
    public TimePointAxis(string header, TimePointType timePointType, string? timePointFormat = null)
    {
      Header = header;
      TimePointType = timePointType;
      TimePointFormat = timePointFormat;
    }

    public string Header { get; }

    public Format Format { get; } = Format.Default();
    
    public TimePointType TimePointType { get; }
    
    public string? TimePointFormat { get; }
  }
}