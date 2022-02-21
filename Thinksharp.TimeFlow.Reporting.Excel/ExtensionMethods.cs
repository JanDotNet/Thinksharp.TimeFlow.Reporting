using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Thinksharp.TimeFlow.Reporting.Excel
{
  internal static class ExtensionMethods
  {
    public static XLAlignmentHorizontalValues ToAlignmentHorizontal(this HorizontalAlignment alignment)
    {
      switch (alignment)
      {
        case HorizontalAlignment.Left:
          return XLAlignmentHorizontalValues.Left;
        case HorizontalAlignment.Right:
          return XLAlignmentHorizontalValues.Right;
        case HorizontalAlignment.Center:
          return XLAlignmentHorizontalValues.Center;
        default:
          throw new NotSupportedException($"Alignment {alignment} is not supported.");
      }
    }

    public static XLColor ToXLColor(this ReportColor color)
    {
      return XLColor.FromArgb(color.A, color.R, color.G, color.B);
    }
  }
}
