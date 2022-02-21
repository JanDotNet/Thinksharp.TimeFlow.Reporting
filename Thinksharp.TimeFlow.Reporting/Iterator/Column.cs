namespace Thinksharp.TimeFlow.Reporting.Iterator
{
  // Reporting

  public abstract class Column
  {
    internal Column(int number, Format? format)
    {
      Number = number;
      Format = format;
    }

    /// <summary>
    /// 1-based number of the column.
    /// </summary>
    public int Number { get; set; }

    public Format? Format { get; }

    public abstract object? GetCellValue(Row row);

    public Format GetFormat(Row row) => Format ?? row.Format;

    public abstract string? GetValueFormat(Row row);
  }
}