namespace Thinksharp.TimeFlow.Reporting.Iterator
{
  // Reporting

  public abstract class Column
  {
    public Column(Format? format)
    {
      Format = format;
    }

    public Format? Format { get; }

    public abstract object? GetCellValue(Row row);

    public Format GetFormat(Row row) => Format ?? row.Format;

    public abstract string? GetValueFormat(Row row);
  }
}