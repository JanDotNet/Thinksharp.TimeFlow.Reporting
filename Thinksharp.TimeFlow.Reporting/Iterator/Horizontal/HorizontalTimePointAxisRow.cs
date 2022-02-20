namespace Thinksharp.TimeFlow.Reporting.Iterator.Horizontal
{
  public class HorizontalTimePointAxisRow : Row
  {
    public HorizontalTimePointAxisRow(TimePointAxis axis)
    {
      Axis = axis;
    }

    public TimePointAxis Axis { get; }

    internal override string? GetValueFormat() => Axis.TimePointFormat;
  }
}