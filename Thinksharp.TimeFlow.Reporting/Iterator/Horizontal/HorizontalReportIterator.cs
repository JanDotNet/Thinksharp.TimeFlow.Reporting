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
      var colNum = 1;
      // time series name headers
      yield return new HorizontalTimeSeriesColumn(colNum++, report.ColumnHeaderFormat);

      // summary header
      foreach (var summary in report.Summary)
      {
        yield return new HorizontalSummaryColumn(colNum++, summary, report.ColumnHeaderFormat);
      }

      // time series data columns
      foreach (var timePoint in timeFrame.EnumerateTimePoints())
      {
        yield return new HorizontalTimePointAxisColumn(colNum++, timeFrame, timePoint);
      }
    }

    public IEnumerable<Row> EnumerateHeaderRows()
    {
      var rowNum = 1;
      foreach (var axis in report.Axes)
      {
        yield return new HorizontalTimePointAxisRow(rowNum++, axis);
      }
    }

    public IEnumerable<Row> EnumerateDataRows()
    {
      var rowNum = report.Axes.Count + 1;
      foreach (var record in report.Body)
      {
        switch (record)
        {
          case TimeSeriesRecord r:
            yield return new RecordDataRow<TimeSeriesRecord>(rowNum++, r);
            break;
          case CalculatedTimeSeriesRecord r:
            yield return new RecordDataRow<CalculatedTimeSeriesRecord>(rowNum++, r);
            break;
          case HeaderRecord r:
            yield return new RecordDataRow<HeaderRecord>(rowNum++, r);
            break;
          default:
            throw new InvalidOperationException($"Reord type '{record.GetType().Name}' is not supported.");
        };
      }
    }
  }
}