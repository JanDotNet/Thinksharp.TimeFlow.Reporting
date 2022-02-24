using System;

namespace Thinksharp.TimeFlow.Reporting.Iterator.Vertical
{
  internal class VerticalTimePointAxisColumn : VerticalColumn
  {
    private readonly TimeFrame timeFrame;
    private readonly TimePointAxis axis;

    public VerticalTimePointAxisColumn(int number, TimeFrame timeFrame, TimePointAxis axis, Format format) : base(number, format, axis.Format)
    {
      this.timeFrame = timeFrame;
      this.axis = axis;
    }

    public override object? GetCellValue(Row row)
    {
      switch (row)
      {
        case VerticalHeaderRow r:
          return axis.Header;
        case VerticalSummaryRow r:
          switch (axis.TimePointType)
          {
            case TimePointType.Start:
              return timeFrame.Start;
            case TimePointType.End:
              return timeFrame.End;
            default:
              throw new NotSupportedException($"TimePointType '{axis.TimePointType}' is not supported.");
          }
        case VerticalTimePointRow r:
          switch (axis.TimePointType)
          {
            case TimePointType.Start:
              return r.TimePoint;
            case TimePointType.End:
              return timeFrame.AddPeriodTo(r.TimePoint);
            default:
              throw new NotSupportedException($"TimePointType '{axis.TimePointType}' is not supported.");
          }
        default:
          throw new InvalidOperationException($"Row type '{row.GetType().Name} is not supported.");
      }
    }

    public override string? GetValueFormat(Row row)
    {
      switch (row)
      {
        case VerticalHeaderRow r:
          return null;
        default:
          return axis.TimePointFormat;
      }
    }
  }
}