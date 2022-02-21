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
      var colNum = 1;
      foreach (var axis in report.Axes)
      {
        yield return new VerticalTimePointAxisColumn(colNum++, timeFrame, axis, report.ColumnHeaderFormat);
      }

      foreach (var record in report.Body)
      {
        switch (record)
        {
          case TimeSeriesRecord r:
            yield return new VerticalRecordColumn(colNum++, timeFrame, r);
            break;
          case CalculatedTimeSeriesRecord r:
            yield return new VerticalRecordColumn(colNum++, timeFrame, r);
            break;
          case HeaderRecord r:
            yield return new VerticalHeaderColumn(colNum++, r);
            break;
          default:
            throw new NotSupportedException($"Record type {record.GetType().Name} is not supported.");
        }
      }
    }

    public IEnumerable<Row> EnumerateHeaderRows()
    {
      var rowNum = 1;
      yield return new VerticalHeaderRow(rowNum);
    }

    public IEnumerable<Row> EnumerateDataRows()
    {
      var rowNum = 2;
      foreach (var timePoint in timeFrame.EnumerateTimePoints())
      {
        yield return new VerticalTimePointRow(rowNum++, timePoint);
      }
    }
  }
}