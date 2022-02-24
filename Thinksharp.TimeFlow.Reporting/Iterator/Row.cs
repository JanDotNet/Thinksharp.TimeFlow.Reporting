namespace Thinksharp.TimeFlow.Reporting.Iterator
{
  public abstract class Row
  {
    internal Row(int number)
    {
      this.Number = number;
    }
    public int Number { get; }

    public Format Format { get; protected set; } = Format.Default();

    internal virtual string? GetValueFormat() => null;
  }
}