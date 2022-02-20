using System;
using System.Collections.Generic;
using System.Linq;
using Thinksharp.TimeFlow.Reporting.Iterator;
using Thinksharp.TimeFlow.Reporting.Iterator.Horizontal;
using Thinksharp.TimeFlow.Reporting.Iterator.Vertical;

namespace Thinksharp.TimeFlow.Reporting
{
  public class Report
  {
    public ReportOrientation Orientation { get; set; } = ReportOrientation.Vertical;
    public Format? ColumnHeaderFormat { get; set; }
    public List<TimePointAxis> Axes { get; } = new List<TimePointAxis>();
    public List<Record> Body { get; } = new List<Record>();
    public List<Summary> Summary { get; } = new List<Summary>();
    public string? Title { get; set; }
    public string? SubTitle { get; set; }

    public IReportIterator CreateReportIterator(TimeFrame timeFrame)
    {
      // validate
      foreach (var ts in Body.OfType<TimeSeriesRecord>())
      {
        if (timeFrame[ts.Key] == null)
        {
          throw new ArgumentException($"Time frame contains no time series with key '{ts.Key}'.");
        }

        // todo: check summary
        // todo: check calculated time series
      }

      switch (Orientation)
      {
        case ReportOrientation.Vertical:
          return new VerticalReportIterator(this, timeFrame);
        case ReportOrientation.Horizontal:
          return new HorizontalReportIterator(this, timeFrame);
        default:
          throw new NotSupportedException($"Orientation '{Orientation}' is not supported for report iteration.");
      }
    }
  }
}