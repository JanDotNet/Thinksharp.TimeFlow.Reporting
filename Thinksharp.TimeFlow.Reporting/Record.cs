namespace Thinksharp.TimeFlow.Reporting
{
  public abstract class Record
  {
    public Record(string key, string header, string? valueFormat = null)
    {
      Header = header;
      ValueFormat = valueFormat;
      Key = key;
    }
    public Format Format { get; } = Format.Default();
    public string Header { get; }
    public string Key { get; }
    public string? ValueFormat { get; }
  }
}