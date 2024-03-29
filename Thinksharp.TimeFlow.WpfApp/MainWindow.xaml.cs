﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Thinksharp.TimeFlow.Reporting;
using Thinksharp.TimeFlow.Reporting.Excel;

namespace Thinksharp.TimeFlow.WpfApp
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private Report report;
    private TimeFrame timeFrame;

    public MainWindow()
    {
      InitializeComponent();
    }

    private void Render(ReportOrientation orientation)
    {
      var tf = new TimeFrame();
      tf["ts1"] = TimeSeries.Factory.FromValue(10, new DateTime(2022, 01, 01), new DateTime(2022, 01, 02), Period.QuarterHour);
      tf["ts2"] = TimeSeries.Factory.FromValue(10, new DateTime(2022, 01, 01), new DateTime(2022, 01, 02), Period.QuarterHour);

      var report = new Report();
      report.Orientation = orientation;
      report.ColumnHeaderFormat.HorizontalAlignment = Reporting.HorizontalAlignment.Center;
      report.ColumnHeaderFormat.Background = ReportColor.Black;
      report.ColumnHeaderFormat.Foreground = ReportColor.White;
      report.ColumnHeaderFormat.Bold = true;
      report.RowHeaderFormat.HorizontalAlignment = Reporting.HorizontalAlignment.Left;
      report.RowHeaderFormat.Bold = true;
      report.Axes.Add(new TimePointAxis("Date", TimePointType.Start, "yyyy-MM-dd"));
      report.Axes.Add(new TimePointAxis("Start", TimePointType.Start, "HH:mm"));
      report.Axes.Add(new TimePointAxis("End", TimePointType.End, "HH:mm"));

      var ts1Record = new TimeSeriesRecord("ts1", "Meine Zeitreihe TS1", "0.00");
      ts1Record.SummaryFormula.Add("sum", "SUM");

      var ts2Record = new TimeSeriesRecord("ts2", "Meine Zeitreihe TS2");
      ts2Record.SummaryFormula.Add("sum", "SUM");

      var ts3Record = new CalculatedTimeSeriesRecord("ts3", "Summe: ", "ts1+ts2");
      ts3Record.Format.Bold = true;
      ts3Record.Format.Background = ReportColor.Blue;
      ts3Record.Format.HorizontalAlignment = Reporting.HorizontalAlignment.Center;
      ts3Record.SummaryFormula.Add("sum", "SUM");

      var header = new HeaderRecord("my Header");
      header.Format.Background = ReportColor.Blue;
      header.Format.Bold = true;

      report.Body.Add(header);
      report.Body.Add(ts1Record);
      report.Body.Add(ts2Record);
      report.Body.Add(ts3Record);


      var summary = new Summary("sum", "Sum", "N3");
      summary.Format.Bold = true;
      report.Summary.Add(summary);

      Report.UpdateView(report, tf);

      this.report = report;
      this.timeFrame = tf;
    }

    private void Button_VerticalClick(object sender, RoutedEventArgs e)
    {
      this.Render(ReportOrientation.Vertical);
    }
    private void Button_HorizontalClick(object sender, RoutedEventArgs e)
    {
      this.Render(ReportOrientation.Horizontal);
    }

    private void Button_ExportClick(object sender, RoutedEventArgs e)
    {
      if (this.report != null && this.timeFrame != null)
      {
        var tempFile = System.IO.Path.GetTempFileName() + ".xlsx";

        ExcelExport.ExportToExcel(this.report, this.timeFrame, tempFile);

        System.Diagnostics.Process.Start(tempFile);
      }
    }
  }
}
