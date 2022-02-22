using System.Windows;
using System.Windows.Media;

namespace Thinksharp.TimeFlow.Reporting.Wpf
{
  public class ColumnViewModel : ObservableObject
  {
    private HeaderRow[] headerRows;

    public HeaderRow[] HeaderRows
    {
      get { return headerRows; }
      set { SetValue(ref headerRows, value); }
    }

    public string ValueFormat { get; set; }

    public Color Background { get; set; }
    public Color Foreground { get; set; }    
  }

  public class HeaderRow : ObservableObject
  {
    private object value;

    public object Value
    {
      get { return value; }
      set { SetValue(ref this.value, value); }
    }
    public FontWeight FontWeight { get; set; }
    public System.Windows.HorizontalAlignment HorizontalAlignment { get; set; }
  }
}
