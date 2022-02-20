using System;

namespace Thinksharp.TimeFlow.Reporting.Iterator.Vertical
{
  public class VerticalTimePointRow : Row
  {
    public VerticalTimePointRow(DateTimeOffset offset)
    {
      TimePoint = offset;
    }

    public DateTimeOffset TimePoint { get; }
  }
}