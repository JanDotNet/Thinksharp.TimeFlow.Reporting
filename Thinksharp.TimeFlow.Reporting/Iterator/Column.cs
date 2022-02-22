namespace Thinksharp.TimeFlow.Reporting.Iterator
{
  public abstract class Column
  {
    internal Column(int number, Format columnFormat, Format format)
    {
      Number = number;
      Format = format;
      ColumnFormat = columnFormat;
    }

    /// <summary>
    /// 1-based number of the column.
    /// </summary>
    public int Number { get; set; }

    protected Format ColumnFormat { get; }

    protected Format Format { get; }

    public abstract object? GetCellValue(Row row);

    public abstract Format GetFormat(Row row);

    public Format GetFormat() => this.Format;

    public Format GetColumnFormat() => this.ColumnFormat;

    public abstract string? GetValueFormat(Row row);
  }
}