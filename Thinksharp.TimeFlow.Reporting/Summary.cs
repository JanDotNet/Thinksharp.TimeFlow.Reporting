namespace Thinksharp.TimeFlow.Reporting
{
  public class Summary
  {
    public Summary(string key, string header, string? valueFormat = null)
    {
      Key = key;
      Header = header;
      ValueFormat = valueFormat;
    }

    public Format Format { get; } = Format.Default();
    public string Header { get; }
    public string Key { get; }
    public string? ValueFormat { get; }
  }
}