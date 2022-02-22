using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfHorizontalAlignment = System.Windows.HorizontalAlignment;

namespace Thinksharp.TimeFlow.Reporting.Wpf
{
  internal static class Extensions
  {
    public static WpfHorizontalAlignment ToHorizontalAlignment(this HorizontalAlignment alignment)
    {
      switch (alignment)
      {
        case HorizontalAlignment.Left:
          return WpfHorizontalAlignment.Left;
        case HorizontalAlignment.Right:
          return WpfHorizontalAlignment.Right;
        case HorizontalAlignment.Center:
          return WpfHorizontalAlignment.Center;
        default:
          throw new NotSupportedException($"Alignment {alignment} is not supported.");
      }
    }
    public static TextAlignment ToTextAlignment(this HorizontalAlignment alignment)
    {
      switch (alignment)
      {
        case HorizontalAlignment.Left:
          return TextAlignment.Left;
        case HorizontalAlignment.Right:
          return TextAlignment.Right;
        case HorizontalAlignment.Center:
          return TextAlignment.Center;
        default:
          throw new NotSupportedException($"Alignment {alignment} is not supported.");
      }
    }
    public static Color ToWpfColor(this ReportColor c)
    {
      return Color.FromArgb(c.A, c.R, c.G, c.B);
    }
  }
}
