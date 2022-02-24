using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Thinksharp.TimeFlow.Reporting.Test
{
  [TestClass]
  public partial class ReportingSerializationTest
  {
    [TestMethod]
    public void TestSerialization()
    {
      var report = new Report();
      report.Orientation = ReportOrientation.Vertical;
      report.ColumnHeaderFormat.HorizontalAlignment = HorizontalAlignment.Center;
      report.ColumnHeaderFormat.Background = ReportColor.Black;
      report.ColumnHeaderFormat.Foreground = ReportColor.White;
      report.ColumnHeaderFormat.Bold = true;
      report.RowHeaderFormat.HorizontalAlignment = HorizontalAlignment.Left;
      report.RowHeaderFormat.Bold = true;
      report.Axes.Add(new TimePointAxis("Date", TimePointType.Start, "yyyy-MM-dd"));
      report.Axes.Add(new TimePointAxis("Start", TimePointType.Start, "HH:mm"));
      report.Axes.Add(new TimePointAxis("End", TimePointType.End, "HH:mm"));

      var ts1Record = new TimeSeriesRecord("ts1", "Meine Zeitreihe TS1", "0.00");
      ts1Record.SummaryFormula.Add("sum", "SUM");

      var ts2Record = new TimeSeriesRecord("ts2", "Meine Zeitreihe TS2");
      ts2Record.SummaryFormula.Add("sum", "SUMPRODUCT(ts1,ts2)/SUM(ts2)");

      var ts3Record = new CalculatedTimeSeriesRecord("ts3", "Summe: ", "ts1+ts2");
      ts3Record.Format.Bold = true;
      ts3Record.Format.Background = ReportColor.Blue;
      ts3Record.Format.HorizontalAlignment = HorizontalAlignment.Center;
      ts3Record.SummaryFormula.Add("sum", "AVERAGE");

      var header = new HeaderRecord("my Header");
      header.Format.Background = ReportColor.Blue;
      header.Format.Bold = true;

      report.Body.Add(header);
      report.Body.Add(ts1Record);
      report.Body.Add(ts2Record);
      report.Body.Add(ts3Record);

      var summary = new Summary("sum", "Sum");
      summary.Format.Bold = true;
      report.Summary.Add(summary);

      var xml = report.ToXml();

      var reportActual = Report.FromXml(xml);
      ReportComparer.AssertAreEqual(report, reportActual);
    }
  }
}