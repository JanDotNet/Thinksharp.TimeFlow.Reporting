using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Thinksharp.TimeFlow.Reporting.Iterator;

namespace Thinksharp.TimeFlow.Reporting.Wpf
{
  internal class DataGridReportColumn : DataGridTextColumn
  {
    private readonly Column column;

    public DataGridReportColumn(Column column)
    {
      this.column = column ?? throw new ArgumentNullException(nameof(column));
    }
    protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
    {
      var rowViewModel = dataItem as RowViewModel;
      var element = base.GenerateElement(cell, dataItem);
      var textBlock = element as TextBlock;
      var binding = (Binding)this.Binding;

      if (element == null || rowViewModel == null || binding == null)
      {
        return element;
      }

      var newBinding = new Binding(binding.Path.Path);
      newBinding.StringFormat = this.column.GetValueFormat(rowViewModel.Row);
      textBlock.SetBinding(TextBlock.TextProperty, newBinding);  

      return textBlock;
    }
  }
}
