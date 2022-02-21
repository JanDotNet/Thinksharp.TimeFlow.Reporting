using System;
using System.Collections.Generic;

namespace Thinksharp.TimeFlow.Reporting.Iterator.Horizontal
{
  internal class HorizontalReportIterator : IReportIterator
  {
    private readonly Report report;
    private readonly TimeFrame timeFrame;

    public HorizontalReportIterator(Report report, TimeFrame timeFrame)
    {
      this.report = report;
      this.timeFrame = timeFrame;
    }

    public IEnumerable<Column> EnumerateColumns()
    {
      // time series name headers
      yield return new HorizontalTimeSeriesColumn(report.ColumnHeaderFormat);

      // summary header
      foreach (var summary in report.Summary)
      {
        yield return new HorizontalSummaryColumn(summary, report.ColumnHeaderFormat);
      }

      // time series data columns
      foreach (var timePoint in timeFrame.EnumerateTimePoints())
      {
        yield return new HorizontalTimePointAxisColumn(timeFrame, timePoint);
      }
    }

    public IEnumerable<Row> EnumerateHeaderRows()
    {
      foreach (var axis in report.Axes)
      {
        yield return new HorizontalTimePointAxisRow(axis);
      }
    }

    public IEnumerable<Row> EnumerateDataRows()
    {
      foreach (var record in report.Body)
      {
        switch (record)
        {
          case TimeSeriesRecord r:
            yield return new RecordDataRow<TimeSeriesRecord>(r);
            break;
          case CalculatedTimeSeriesRecord r:
            yield return new RecordDataRow<CalculatedTimeSeriesRecord>(r);
            break;
          case HeaderRecord r:
            yield return new RecordDataRow<HeaderRecord>(r);
            break;
          default:
            throw new InvalidOperationException($"Reord type '{record.GetType().Name}' is not supported.");
        };
      }
    }
  }
}