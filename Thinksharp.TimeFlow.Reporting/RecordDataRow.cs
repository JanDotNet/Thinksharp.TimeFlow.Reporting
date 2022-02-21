using Thinksharp.TimeFlow.Reporting.Iterator;

namespace Thinksharp.TimeFlow.Reporting
{
  public class RecordDataRow : Row
  {
    public RecordDataRow(Record record)
    {
      this.Record = record;
    }

    public Record Record { get; }
  }

  public class RecordDataRow<TRecord> : RecordDataRow where TRecord : Record
  {
    public RecordDataRow(TRecord record)
      : base(record)
    {
      Format = record.Format;
      Record = record;
    }

    public new TRecord Record { get; }

    internal override string? GetValueFormat() => this.Record.ValueFormat;
  }
}