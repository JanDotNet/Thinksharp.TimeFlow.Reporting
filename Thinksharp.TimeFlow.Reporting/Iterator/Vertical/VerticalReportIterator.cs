using System;
using System.Collections.Generic;

namespace Thinksharp.TimeFlow.Reporting.Iterator.Vertical
{
  public class VerticalReportIterator : IReportIterator
  {
    private readonly Report report;
    private readonly TimeFrame timeFrame;

    public VerticalReportIterator(Report report, TimeFrame timeFrame)
    {
      this.report = report;
      this.timeFrame = timeFrame;
    }

    public IEnumerable<Column> EnumerateColumns()
    {
      foreach (var axis in report.Axes)
      {
        yield return new VerticalTimePointAxisColumn(timeFrame, axis, report.ColumnHeaderFormat);
      }

      foreach (var record in report.Body)
      {
        switch (record)
        {
          case TimeSeriesRecord r:
            yield return new VerticalTimeSeriesColumn(timeFrame, r);
            break;
          case HeaderRecord r:
            yield return new VerticalHeaderColumn(r);
            break;
          default:
            throw new NotSupportedException($"Record type {record.GetType().Name} is not supported.");
        }
      }
    }

    public IEnumerable<Row> EnumerateHeaderRows()
    {
      yield return new VerticalHeaderRow();
    }

    public IEnumerable<Row> EnumerateDataRows()
    {
      foreach (var timePoint in timeFrame.EnumerateTimePoints())
      {
        yield return new VerticalTimePointRow(timePoint);
      }
    }
  }
}