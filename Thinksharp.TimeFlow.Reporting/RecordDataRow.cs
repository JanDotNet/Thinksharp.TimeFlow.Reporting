using Thinksharp.TimeFlow.Reporting.Iterator;

namespace Thinksharp.TimeFlow.Reporting
{
  public class RecordDataRow<TRecord> : Row where TRecord : Record
  {
    public RecordDataRow(TRecord record)
    {
      Format = record.Format;
      Record = record;
    }

    public TRecord Record { get; }

    internal override string? GetValueFormat() => this.Record.ValueFormat;
  }
}