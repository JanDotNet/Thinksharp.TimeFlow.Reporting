using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinksharp.TimeFlow.Reporting.Test
{
  internal static class ReportComparer
  {
    public static void AssertAreEqual(Report r1, Report r2)
    {
      AssertAreEqual(r1.ColumnHeaderFormat, r2.ColumnHeaderFormat, "ColumnHeaderFormat");
      AssertAreEqual(r1.RowHeaderFormat, r2.RowHeaderFormat, "RowHeaderFormat");
      Assert.AreEqual(r1.Orientation, r2.Orientation, "Orientation");
      Assert.AreEqual(r1.SubTitle, r2.SubTitle, "SubTitle");
      Assert.AreEqual(r1.Title, r2.Title, "Title");
      AssertAreEqual(r1.Axes, r2.Axes);
      AssertAreEqual(r1.Summary, r2.Summary);
      AssertAreEqual(r1.Body, r2.Body);
    }

    public static void AssertAreEqual(List<Record> a1, List<Record> a2)
    {
      Assert.AreEqual(a1.Count, a2.Count, "Record.Length");
      for (int i = 0; i < a1.Count; i++)
      {
        Assert.AreEqual(a1[i].Header, a2[i].Header, "Record.Header");
        Assert.AreEqual(a1[i].ValueFormat, a2[i].ValueFormat, "Record.ValueFormat");
        Assert.AreEqual(a1[i].Key, a2[i].Key, "Record.Key");
        AssertAreEqual(a1[i].Format, a2[i].Format, "Record.Format");
        AssertAreEqual(a1[i].SummaryFormula, a2[i].SummaryFormula, "Record");

        switch (a1[i], a2[i])
        {
          case (TimeSeriesRecord r1, TimeSeriesRecord r2):            
            break;
          case (CalculatedTimeSeriesRecord r1, CalculatedTimeSeriesRecord r2):
            Assert.AreEqual(r1.Formula, r2.Formula, "Record.Formula");
            break;
          case (HeaderRecord r1, HeaderRecord r2):
            break;
          default:
            Assert.Fail($"Invalid combination of record types: {a1[i].GetType().Name} / {a2[i].GetType().Name}.");
            break;
        }
      }
    }

    public static void AssertAreEqual(Dictionary<string, string> a1, Dictionary<string, string> a2, string context)
    {
      Assert.AreEqual(a1.Count, a2.Count, context + ".Length");
      foreach (var pair in a1)
      {
        Assert.AreEqual(a1[pair.Key], a2[pair.Key], context + "." + pair.Key);
      }
    }

    public static void AssertAreEqual(List<Summary> a1, List<Summary> a2)
    {
      Assert.AreEqual(a1.Count, a2.Count, "Summary.Length");
      for (int i = 0; i < a1.Count; i++)
      {
        Assert.AreEqual(a1[i].Header, a2[i].Header, "Summary.Header");
        Assert.AreEqual(a1[i].ValueFormat, a2[i].ValueFormat, "Summary.ValueFormat");
        Assert.AreEqual(a1[i].Key, a2[i].Key, "Summary.Key");
        AssertAreEqual(a1[i].Format, a2[i].Format, "Summary.Format");
      }
    }

    public static void AssertAreEqual(List<TimePointAxis> a1, List<TimePointAxis> a2)
    {
      Assert.AreEqual(a1.Count, a2.Count, "Axes.Length");
      for (int i = 0; i < a1.Count; i++)
      {
        Assert.AreEqual(a1[i].Header, a2[i].Header, "Axis.Header");
        Assert.AreEqual(a1[i].TimePointFormat, a2[i].TimePointFormat, "Axis.TimePointFormat");
        Assert.AreEqual(a1[i].TimePointType, a2[i].TimePointType, "Axis.TimePointType");
        AssertAreEqual(a1[i].Format, a2[i].Format, "Axis.Format");
      }
    }

    public static void AssertAreEqual(Format r1, Format r2, string context)
    {
      Assert.AreEqual(r1.Background, r2.Background, context + ".Background");
      Assert.AreEqual(r1.Foreground, r2.Foreground, context + ".Foreground");
      Assert.AreEqual(r1.Bold, r2.Bold, context + ".Bold");
      Assert.AreEqual(r1.HorizontalAlignment, r2.HorizontalAlignment, context + ".HorizontalAlignment");
      Assert.AreEqual(r1.HasHorizontalAlignmentModified, r2.HasHorizontalAlignmentModified, context + ".HasHorizontalAlignmentModified");
      Assert.AreEqual(r1.HasBackgroundModified, r2.HasBackgroundModified, context + ".HasBackgroundModified");
      Assert.AreEqual(r1.HasBoldModified, r2.HasBoldModified, context + ".HasBoldModified");
      Assert.AreEqual(r1.HasForegroundModified, r2.HasForegroundModified, context + ".HasForegroundModified");
    }
  }
}
