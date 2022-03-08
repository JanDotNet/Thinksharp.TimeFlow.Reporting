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

    private DataTemplate CreateTemplate(Binding binding)
    {
      DataTemplate cellTemplate = new DataTemplate(); // create a datatemplate
      FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBlock));
      factory.SetBinding(TextBlock.TextProperty, binding);
      factory.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
      cellTemplate.VisualTree = factory;

      return cellTemplate;
    }

    public void UpdateView(Report report, TimeFrame timeFrame)
    {
      if (report == null || timeFrame == null)
      {
        return;
      }

      var dataGrid = this.GetTemplateChild("DataGrid") as DataGrid;

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

          if (colDataFormat.HasHorizontalAlignmentModified)
            column.CellStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, colDataFormat.HorizontalAlignment.ToTextAlignment()));

          if (colDataFormat.HasBoldModified && colDataFormat.Bold)
            column.CellStyle.Setters.Add(new Setter(DataGridCell.FontWeightProperty, FontWeights.Bold));

          var trigger = new Trigger() { Property = DataGridCell.IsSelectedProperty, Value = false, };

          if (colDataFormat.HasBackgroundModified)
          {
            trigger.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(colDataFormat.Background.ToWpfColor())));
            trigger.Setters.Add(new Setter(DataGridCell.BorderBrushProperty, new SolidColorBrush(colDataFormat.Background.ToWpfColor())));
          }
          if (colDataFormat.HasForegroundModified)
            trigger.Setters.Add(new Setter(DataGridCell.ForegroundProperty, new SolidColorBrush(colDataFormat.Foreground.ToWpfColor())));

          if (trigger.Setters.Count > 0)
            column.CellStyle.Triggers.Add(trigger);
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
