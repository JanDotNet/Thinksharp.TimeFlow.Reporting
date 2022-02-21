using Thinksharp.TimeFlow.Reporting.Iterator;

namespace Thinksharp.TimeFlow.Reporting
{
  public class RecordDataRow : Row
  {
    public RecordDataRow(int number, Record record) : base(number)
    {
      this.Record = record;
    }

    public Record Record { get; }
  }

  public class RecordDataRow<TRecord> : RecordDataRow where TRecord : Record
  {
    public RecordDataRow(int number, TRecord record)
      : base(number, record)
    {
      Format = record.Format;
      Record = record;
    }

    public new TRecord Record { get; }

    internal override string? GetValueFormat() => this.Record.ValueFormat;
  }
}