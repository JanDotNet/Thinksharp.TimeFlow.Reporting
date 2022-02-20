namespace Thinksharp.TimeFlow.Reporting.Iterator
{
  public class Row
  {
    public virtual Format Format { get; protected set; } = Format.Default();

    internal virtual string? GetValueFormat() => null;
  }
}