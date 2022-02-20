using System;
using System.Collections.Generic;
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

namespace Thinksharp.TimeFlow.WpfApp
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void Render(ReportOrientation orientation)
    {
      var tf = new TimeFrame();
      tf["ts1"] = TimeSeries.Factory.FromValue(10, new DateTime(2022, 01, 01), new DateTime(2023, 01, 02), Period.Day);
      tf["ts2"] = TimeSeries.Factory.FromValue(10, new DateTime(2022, 01, 01), new DateTime(2023, 01, 02), Period.Day);

      var report = new Report();
      report.Orientation = orientation;
      report.ColumnHeaderFormat = new Format();
      report.Axes.Add(new TimePointAxis("Date", TimePointType.Start, "yyyy-MM-dd"));
      report.Axes.Add(new TimePointAxis("Start", TimePointType.Start, "HH:mm"));
      report.Axes.Add(new TimePointAxis("End", TimePointType.End, "HH:mm"));

      var ts1Record = new TimeSeriesRecord("ts1", "Meine Zeitreihe TS1", "0.00");
      ts1Record.AggregationFormula.Add("sum", "SUM");

      var ts2Record = new TimeSeriesRecord("ts2", "Meine Zeitreihe TS2");
      ts2Record.AggregationFormula.Add("sum", "SUM");

      var header = new HeaderRecord("my Header");
      header.Format.Background = ReportColor.Blue;
      header.Format.Bold = true;

      report.Body.Add(ts1Record);
      report.Body.Add(ts2Record);
      report.Body.Add(header);

      report.Summary.Add(new Summary("sum", "Sum"));

      Report.Report = report;
      Report.TimeFrame = tf;
    }

    private void Button_VerticalClick(object sender, RoutedEventArgs e)
    {
      this.Render(ReportOrientation.Vertical);
    }
    private void Button_HorizontalClick(object sender, RoutedEventArgs e)
    {
      this.Render(ReportOrientation.Horizontal);
    }
  }
}
