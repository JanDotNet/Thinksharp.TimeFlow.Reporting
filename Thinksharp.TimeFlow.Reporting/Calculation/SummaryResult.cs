namespace Thinksharp.TimeFlow.Reporting.Calculation
{
  internal class SummaryResult
  {
    public SummaryResult(string summaryKey, string recordKey, object? value)
    {
      SummaryKey = summaryKey;
      RecordKey = recordKey;
      Value = value;
    }

    public string SummaryKey { get; }
    public string RecordKey { get; }
    public object? Value { get; }
  }
}
