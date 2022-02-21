using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Thinksharp.TimeFlow.Reporting.Test
{
  [TestClass]
  public partial class ReportingTest
  {
    [TestMethod]
    public void TestReportGeneration_Horizontal()
    {
      var tf = new TimeFrame();
      tf["ts1"] = TimeSeries.Factory.FromValue(10, new DateTime(2022, 01, 01), 3, Period.Day);
      tf["ts2"] = TimeSeries.Factory.FromValue(100, new DateTime(2022, 01, 01), 3, Period.Day);

      var report = new Report();
      report.Orientation = ReportOrientation.Horizontal;
      report.ColumnHeaderFormat = new Format();
      report.Axes.Add(new TimePointAxis("Start", TimePointType.Start, "MM-dd-yyyy"));
      report.Axes.Add(new TimePointAxis("End", TimePointType.End, "MM-dd"));

      var ts1Record = new TimeSeriesRecord("ts1", "Meine Zeitreihe TS1", "0.00");
      //ts1Record.AggregationFormula.Add("sum", "SUM");

      var ts2Record = new TimeSeriesRecord("ts2", "Meine Zeitreihe TS2");
      //ts2Record.AggregationFormula.Add("sum", "SUM");

      report.Body.Add(ts1Record);
      report.Body.Add(ts2Record);

      //report.Summary.Add(new Summary("sum", "Sum"));

      var iterator = report.CreateReportIterator(tf);

      var cellsExpected = new List<Cell>();
      cellsExpected.Add(new Cell(1, 1, null, null, report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(1, 2, new DateTimeOffset(new DateTime(2022, 01, 01)), "MM-dd-yyyy", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(1, 3, new DateTimeOffset(new DateTime(2022, 01, 02)), "MM-dd-yyyy", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(1, 4, new DateTimeOffset(new DateTime(2022, 01, 03)), "MM-dd-yyyy", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(2, 1, null, null, report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(2, 2, new DateTimeOffset(new DateTime(2022, 01, 02)), "MM-dd", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(2, 3, new DateTimeOffset(new DateTime(2022, 01, 03)), "MM-dd", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(2, 4, new DateTimeOffset(new DateTime(2022, 01, 04)), "MM-dd", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(3, 1, "Meine Zeitreihe TS1", null, ts1Record.Format));
      cellsExpected.Add(new Cell(3, 2, 10M, "0.00", ts1Record.Format));
      cellsExpected.Add(new Cell(3, 3, 10M, "0.00", ts1Record.Format));
      cellsExpected.Add(new Cell(3, 4, 10M, "0.00", ts1Record.Format));
      cellsExpected.Add(new Cell(4, 1, "Meine Zeitreihe TS2", null, ts2Record.Format));
      cellsExpected.Add(new Cell(4, 2, 100M, null, ts2Record.Format));
      cellsExpected.Add(new Cell(4, 3, 100M, null, ts2Record.Format));
      cellsExpected.Add(new Cell(4, 4, 100M, null, ts2Record.Format));

      List<Cell> cellsActual = CreateActualCells(iterator);

      AssertAreEqual(cellsExpected, cellsActual);
    }

    [TestMethod]
    public void TestReportGeneration_HeaderAndCalculation()
    {
      var tf = new TimeFrame();
      tf["ts1"] = TimeSeries.Factory.FromValue(1, new DateTime(2022, 01, 01), 2, Period.Day);
      tf["ts2"] = TimeSeries.Factory.FromValue(1, new DateTime(2022, 01, 01), 2, Period.Day);

      var report = new Report();
      report.Orientation = ReportOrientation.Horizontal;
      report.ColumnHeaderFormat = new Format();
      report.Axes.Add(new TimePointAxis("Start", TimePointType.Start, "MM-dd-yyyy"));
      report.Axes.Add(new TimePointAxis("End", TimePointType.End, "MM-dd"));

      var ts1Record = new TimeSeriesRecord("ts1", "Meine Zeitreihe TS1", "0.00");
      //ts1Record.AggregationFormula.Add("sum", "SUM");

      var ts2Record = new TimeSeriesRecord("ts2", "Meine Zeitreihe TS2");
      //ts2Record.AggregationFormula.Add("sum", "SUM");

      var header = new HeaderRecord("Sum");

      var ts3Record = new CalculatedTimeSeriesRecord("ts3", "Meine Zeitreihe TS3", "ts1 + ts2", "0.0");

      report.Body.Add(ts1Record);
      report.Body.Add(ts2Record);
      report.Body.Add(header);
      report.Body.Add(ts3Record);

      //report.Summary.Add(new Summary("sum", "Sum"));

      var iterator = report.CreateReportIterator(tf);

      var cellsExpected = new List<Cell>();
      cellsExpected.Add(new Cell(1, 1, null, null, report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(1, 2, new DateTimeOffset(new DateTime(2022, 01, 01)), "MM-dd-yyyy", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(1, 3, new DateTimeOffset(new DateTime(2022, 01, 02)), "MM-dd-yyyy", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(2, 1, null, null, report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(2, 2, new DateTimeOffset(new DateTime(2022, 01, 02)), "MM-dd", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(2, 3, new DateTimeOffset(new DateTime(2022, 01, 03)), "MM-dd", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(3, 1, "Meine Zeitreihe TS1", null, ts1Record.Format));
      cellsExpected.Add(new Cell(3, 2, 1M, "0.00", ts1Record.Format));
      cellsExpected.Add(new Cell(3, 3, 1M, "0.00", ts1Record.Format));
      cellsExpected.Add(new Cell(4, 1, "Meine Zeitreihe TS2", null, ts2Record.Format));
      cellsExpected.Add(new Cell(4, 2, 1M, null, ts2Record.Format));
      cellsExpected.Add(new Cell(4, 3, 1M, null, ts2Record.Format));
      cellsExpected.Add(new Cell(5, 1, "Sum", null, ts2Record.Format));
      cellsExpected.Add(new Cell(5, 2, null, null, ts2Record.Format));
      cellsExpected.Add(new Cell(5, 3, null, null, ts2Record.Format));
      cellsExpected.Add(new Cell(6, 1, "Meine Zeitreihe TS3", null, ts2Record.Format));
      cellsExpected.Add(new Cell(6, 2, 2M, "0.0", ts2Record.Format));
      cellsExpected.Add(new Cell(6, 3, 2M, null, ts2Record.Format));

      List<Cell> cellsActual = CreateActualCells(iterator);

      AssertAreEqual(cellsExpected, cellsActual);
    }

    [TestMethod]
    public void TestReportGeneration_Vertical()
    {
      var tf = new TimeFrame();
      tf["ts1"] = TimeSeries.Factory.FromValue(10, new DateTime(2022, 01, 01), 3, Period.Day);
      tf["ts2"] = TimeSeries.Factory.FromValue(100, new DateTime(2022, 01, 01), 3, Period.Day);

      var report = new Report();
      report.Orientation = ReportOrientation.Vertical;
      report.ColumnHeaderFormat = new Format();
      report.Axes.Add(new TimePointAxis("Start", TimePointType.Start, "MM-dd-yyyy"));
      report.Axes.Add(new TimePointAxis("End", TimePointType.End, "MM-dd"));

      var ts1Record = new TimeSeriesRecord("ts1", "Meine Zeitreihe TS1", "0.00");
      //ts1Record.AggregationFormula.Add("sum", "SUM");

      var ts2Record = new TimeSeriesRecord("ts2", "Meine Zeitreihe TS2");
      //ts2Record.AggregationFormula.Add("sum", "SUM");

      report.Body.Add(ts1Record);
      report.Body.Add(ts2Record);

      //report.Summary.Add(new Summary("sum", "Sum"));

      var iterator = report.CreateReportIterator(tf);

      var cellsExpected = new List<Cell>();
      cellsExpected.Add(new Cell(1, 1, "Start", null, report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(1, 2, "End", null, report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(1, 3, "Meine Zeitreihe TS1", null, report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(1, 4, "Meine Zeitreihe TS2", null, report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(2, 1, new DateTimeOffset(new DateTime(2022, 01, 01)), "MM-dd-yyyy", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(2, 2, new DateTimeOffset(new DateTime(2022, 01, 02)), "MM-dd", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(2, 3, 10M, "0.00", ts1Record.Format));
      cellsExpected.Add(new Cell(2, 4, 100M, null, ts2Record.Format));
      cellsExpected.Add(new Cell(3, 1, new DateTimeOffset(new DateTime(2022, 01, 02)), "MM-dd-yyyy", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(3, 2, new DateTimeOffset(new DateTime(2022, 01, 03)), "MM-dd", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(3, 3, 10M, "0.00", ts1Record.Format));
      cellsExpected.Add(new Cell(3, 4, 100M, null, ts2Record.Format));
      cellsExpected.Add(new Cell(4, 1, new DateTimeOffset(new DateTime(2022, 01, 03)), "MM-dd-yyyy", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(4, 2, new DateTimeOffset(new DateTime(2022, 01, 04)), "MM-dd", report.ColumnHeaderFormat));
      cellsExpected.Add(new Cell(4, 3, 10M, "0.00", ts1Record.Format));
      cellsExpected.Add(new Cell(4, 4, 100M, null, ts2Record.Format));

      List<Cell> cellsActual = CreateActualCells(iterator);

      AssertAreEqual(cellsExpected, cellsActual);
    }

    private static List<Cell> CreateActualCells(Iterator.IReportIterator iterator)
    {
      var cellsActual = new List<Cell>();

      foreach (var col in iterator.EnumerateColumns())
      {
        foreach (var row in iterator.EnumerateHeaderRows())
        {
          var value = col.GetCellValue(row);
          var valueFormat = col.GetValueFormat(row);
          var format = col.GetFormat(row);

          cellsActual.Add(new Cell(row.Number, col.Number, value, valueFormat, format));
        }
      }

      foreach (var col in iterator.EnumerateColumns())
      {
        foreach (var row in iterator.EnumerateDataRows())
        {
          var value = col.GetCellValue(row);
          var valueFormat = col.GetValueFormat(row);
          var format = col.GetFormat(row);

          cellsActual.Add(new Cell(row.Number, col.Number, value, valueFormat, format));
        }
      }

      return cellsActual;
    }

    private void AssertAreEqual(List<Cell> cellsExpected, List<Cell> cellsActual)
    {
      cellsExpected = cellsExpected.OrderBy(c => c.Row).ThenBy(c => c.Col).ToList();
      cellsActual = cellsActual.OrderBy(c => c.Row).ThenBy(c => c.Col).ToList();

      Assert.AreEqual(cellsExpected.Count, cellsActual.Count, "Unexpected number of generated cells");

      for (int i = 0; i < cellsActual.Count; i++)
      {
        Assert.AreEqual(cellsExpected[i], cellsActual[i], $"Cells in idx '{i}' are different:{Environment.NewLine}Expected:{cellsExpected[i]}{Environment.NewLine}Actual:{cellsActual[i]}");
      }
    }
  }
}