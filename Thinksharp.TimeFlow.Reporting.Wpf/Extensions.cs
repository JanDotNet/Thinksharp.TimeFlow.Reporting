using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Thinksharp.TimeFlow.Reporting.Wpf
{
  internal static class Extensions
  {
    public static Color ToWpfColor(this ReportColor c)
    {
      return Color.FromArgb(c.A, c.R, c.G, c.B);
    }
  }
}
