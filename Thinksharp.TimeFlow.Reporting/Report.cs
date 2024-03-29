﻿using System;
using System.Collections.Generic;
using System.Linq;
using Thinksharp.TimeFlow.Reporting.Calculation;
using Thinksharp.TimeFlow.Reporting.Iterator;
using Thinksharp.TimeFlow.Reporting.Iterator.Horizontal;
using Thinksharp.TimeFlow.Reporting.Iterator.Vertical;

namespace Thinksharp.TimeFlow.Reporting
{
  public class Report
  {
    public static IReportConfiguration Configuration { get; } = new ReportConfiguration();
    internal static ReportConfiguration Config => (ReportConfiguration)Configuration;

    public ReportOrientation Orientation { get; set; } = ReportOrientation.Vertical;
    public Format ColumnHeaderFormat { get; } = Format.Default();
    public Format RowHeaderFormat { get; } = Format.Default();
    public List<TimePointAxis> Axes { get; } = new List<TimePointAxis>();
    public List<Record> Body { get; } = new List<Record>();
    public List<Summary> Summary { get; } = new List<Summary>();
    public string? Title { get; set; }
    public string? SubTitle { get; set; }

    public string ToXml() => ReportSerializer.ToXml(this);

    public static Report FromXml(string xml) => ReportSerializer.FromXml(xml);

    public IReportIterator CreateReportIterator(TimeFrame timeFrame)
    {
      timeFrame = timeFrame.Copy();

      timeFrame.CheckIfTimeSeriesExist(this.Body.OfType<TimeSeriesRecord>());
      timeFrame.ExtendWithCalculatedTimeSeries(Body.OfType<CalculatedTimeSeriesRecord>());
      var summary = timeFrame.CalculateSummary(this.Body);

      switch (Orientation)
      {
        case ReportOrientation.Vertical:
          return new VerticalReportIterator(this, timeFrame, summary.ToArray());
        case ReportOrientation.Horizontal:
          return new HorizontalReportIterator(this, timeFrame, summary.ToArray());
        default:
          throw new NotSupportedException($"Orientation '{Orientation}' is not supported for report iteration.");
      }
    }
  }
}