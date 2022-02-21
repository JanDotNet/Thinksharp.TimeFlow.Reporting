using System;

namespace Thinksharp.TimeFlow.Reporting.Iterator.Vertical
{
  public class VerticalTimePointRow : Row
  {
    public VerticalTimePointRow(int number, DateTimeOffset offset) : base(number)
    {
      TimePoint = offset;
    }

    public DateTimeOffset TimePoint { get; }
  }
}