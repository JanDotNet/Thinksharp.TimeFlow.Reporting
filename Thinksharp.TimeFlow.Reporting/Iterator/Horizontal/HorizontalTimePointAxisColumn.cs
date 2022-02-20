using System;

namespace Thinksharp.TimeFlow.Reporting.Iterator.Horizontal
{
  public class HorizontalTimePointAxisColumn : Column
  {
    private readonly TimeFrame timeFrame;
    private readonly DateTimeOffset end;

    public HorizontalTimePointAxisColumn(Format? format) : base(format)
    {
    }

    public HorizontalTimePointAxisColumn(TimeFrame timeFrame, DateTimeOffset timePoint) : base(null)
    {
      this.timeFrame = timeFrame;
      TimePoint = timePoint;
      end = timeFrame.AddPeriodTo(timePoint);
    }

    public DateTimeOffset TimePoint { get; }

    public override object? GetCellValue(Row row)
    {
      switch (row)
      {
        case HorizontalTimePointAxisRow axisRow:
          switch (axisRow.Axis.TimePointType)
          {
            case TimePointType.Start:
              return TimePoint;
            case TimePointType.End:
              return end;
            default:
              throw new NotSupportedException($"TimePointType '{axisRow.Axis.TimePointType}' is not supported.");
          }
        case RecordDataRow<TimeSeriesRecord> r:
          var ts = timeFrame[r.Record.Key];
          if (ts == null)
          {
            throw new ReportGenerationException($"Can not find time series to key '{r.Record.Key}'.");
          }
          return ts[TimePoint];
        case RecordDataRow<HeaderRecord> r:
          return null;
        default:
          throw new InvalidOperationException($"Row type '{row.GetType().Name} is not supported.");
      }
    }

    public override string? GetValueFormat(Row row) => row?.GetValueFormat();
  }
}