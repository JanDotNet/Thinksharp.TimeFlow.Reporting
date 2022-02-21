namespace Thinksharp.TimeFlow.Reporting.Iterator.Horizontal
{
  public class HorizontalTimePointAxisRow : Row
  {
    public HorizontalTimePointAxisRow(int number, TimePointAxis axis) : base(number)
    {
      Axis = axis;
    }

    public TimePointAxis Axis { get; }

    internal override string? GetValueFormat() => Axis.TimePointFormat;
  }
}