using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Thinksharp.TimeFlow.Reporting.Wpf
{
  public partial class ReportView : Control
    {
        static ReportView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ReportView), new FrameworkPropertyMetadata(typeof(ReportView)));
        }

    public Report Report
    {
      get { return (Report)GetValue(ReportProperty); }
      set { SetValue(ReportProperty, value); }
    }

    public static readonly DependencyProperty ReportProperty =
        DependencyProperty.Register("Report", typeof(Report), typeof(ReportView), new PropertyMetadata(null, ReportProeprtyChangedCallback));

    private static void ReportProeprtyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var repowrtView = d as ReportView;
      repowrtView.UpdateReport();
    }

    public TimeFrame TimeFrame
    {
      get { return (TimeFrame)GetValue(TimeFrameProperty); }
      set { SetValue(TimeFrameProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TimeFrame.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TimeFrameProperty =
        DependencyProperty.Register("TimeFrame", typeof(TimeFrame), typeof(ReportView), new PropertyMetadata(null, TimeFramePropertyChangedCallback));

    private static void TimeFramePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var repowrtView = d as ReportView;
      repowrtView.UpdateReport();
    }

    private DataTemplate CreateTemplate(Binding binding)
    {
      DataTemplate cellTemplate = new DataTemplate(); // create a datatemplate
      FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBlock));
      factory.SetBinding(TextBlock.TextProperty, binding);
      factory.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
      cellTemplate.VisualTree = factory;

      return cellTemplate;
    }

    private void UpdateReport()
    {
      if (this.Report == null || this.TimeFrame == null)
      {
        return;
      }

      var dataGrid = this.GetTemplateChild("DataGrid") as DataGrid;
      var report = this.Report;
      var timeFrame = this.TimeFrame;

      dataGrid.Columns.Clear();

      var iterator = report.CreateReportIterator(timeFrame);

      var colNo = 1;
      foreach (var col in iterator.EnumerateColumns())
      {
        var headerRows = new List<HeaderRow>();
        foreach (var row in iterator.EnumerateHeaderRows())
        {
          var value = col.GetCellValue(row);
          var format = col.GetFormat(row);
          headerRows.Add(new HeaderRow
          {
            Value = string.Format("{0:" + col.GetValueFormat(row) + "}", value),
            FontWeight = format.Bold ? FontWeights.Bold : FontWeights.Normal,
            HorizontalAlignment = format.HorizontalAlignment.ToHorizontalAlignment()
          });
        }

        var colName = "column" + colNo++;
        var colFormat = col.GetColumnFormat();
        var column = new DataGridReportColumn(col);
        var columnHeaderVM = new ColumnViewModel();
        columnHeaderVM.HeaderRows = headerRows.ToArray();
        if (colFormat != null)
        {
          columnHeaderVM.Background = colFormat.Background.ToWpfColor();
          columnHeaderVM.Foreground = colFormat.Foreground.ToWpfColor();
        }

        var colDataFormat = col.GetFormat();
        if (colDataFormat != null)
        {
          column.CellStyle = new Style(typeof(DataGridCell));
          column.CellStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, colDataFormat.HorizontalAlignment.ToTextAlignment()));          
          var trigger = new Trigger() { Property = DataGridCell.IsSelectedProperty, Value = false, };
          trigger.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(colDataFormat.Background.ToWpfColor())));
          trigger.Setters.Add(new Setter(DataGridCell.ForegroundProperty, new SolidColorBrush(colDataFormat.Foreground.ToWpfColor())));
          trigger.Setters.Add(new Setter(DataGridCell.BorderBrushProperty, new SolidColorBrush(colDataFormat.Background.ToWpfColor())));          
          if (colDataFormat.Bold)
          {
            trigger.Setters.Add(new Setter(DataGridCell.FontWeightProperty, FontWeights.Bold));
          }

          column.CellStyle.Triggers.Add(trigger);
          //column.CellStyle.Triggers.Add()
          //column.CellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(colFormat.Background.ToWpfColor())));
          //column.CellStyle.Setters.Add(new Setter(DataGridCell.ForegroundProperty, new SolidColorBrush(colFormat.Foreground.ToWpfColor())));
        }

        column.Header = columnHeaderVM;
        column.Binding = new Binding(colName);
        column.Binding.StringFormat = col.GetValueFormat(null);
        dataGrid.Columns.Add(column);
      }

      var rows = new List<RowViewModel>();
      foreach (var row in iterator.EnumerateDataRows())
      { 
        colNo = 1;
        var dic = new Dictionary<string, object>();
        foreach (var col in iterator.EnumerateColumns())
        {
          var colName = "column" + colNo++;          

          dic[colName] = col.GetCellValue(row);
        }
        var rowVM = new RowViewModel(row, dic);
        rowVM.Background = row.Format.Background.ToWpfColor();
        rowVM.Foreground = row.Format.Foreground.ToWpfColor();
        rowVM.FontWeight = row.Format.Bold ? FontWeights.Bold : FontWeights.Normal;
        rowVM.HorizontalAlignment = row.Format.HorizontalAlignment.ToTextAlignment();

        rows.Add(rowVM);
      }

      dataGrid.ItemsSource = rows;
    }
  }
}
