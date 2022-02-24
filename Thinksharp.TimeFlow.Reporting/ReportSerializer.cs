using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Thinksharp.TimeFlow.Reporting
{
  internal static class ReportSerializer
  {
    public static string ToXml(this Report report)
    {
      var xml = new XDocument(ToXElement(report));

      return xml.ToString();
    }

    public static Report FromXml(string xml)
    {
      var xDocument = XDocument.Parse(xml);

      var xReport = xDocument.Element("Report");
      var orientation = xReport.Attribute("Orientation").ToOrientation();
      var report = xReport.ToReport();

      return report;
    }

    private static Report ToReport(this XElement xReport)
    {
      var report = new Report();
      report.Orientation = xReport.Attribute("Orientation").ToOrientation();
      report.RowHeaderFormat.FromXElement(xReport.Element("RowHeaderFormat"));
      report.ColumnHeaderFormat.FromXElement(xReport.Element("ColumnHeaderFormat"));
      report.Axes.AddRange(xReport.Element("Axis").ToTimePointAxes());
      report.Body.AddRange(xReport.Element("Body").ToRecords());
      report.Summary.AddRange(xReport.Element("Summaries").ToSummaries());

      return report; ;
    }

    // XElement => Obj

    private static ReportOrientation ToOrientation(this XAttribute a)
      => (ReportOrientation)Enum.Parse(typeof(ReportOrientation), a.Value);

    private static void FromXElement(this Format format, XElement e)
    {
      if (e == null)
      {
        return;
      }

      var background = e.Attribute("Background")?.Value;      
      if (background != null)
        format.Background = ReportColor.FromHexCode(background);
      
      var foreground = e.Attribute("Foreground")?.Value;
      if (foreground != null)
        format.Foreground = ReportColor.FromHexCode(foreground);

      var bold = e.Attribute("Bold")?.Value;
      if (bold != null)
        format.Bold = bool.Parse(bold);

      var horizontalAlignment = e.Attribute("HorizontalAlignment")?.Value;
      if (horizontalAlignment != null)
        format.HorizontalAlignment = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), horizontalAlignment);
    }

    private static IEnumerable<Record> ToRecords(this XElement xElement)
    {
      foreach (var e in xElement.Elements())
      {
        switch (e.Name.ToString())
        {
          case "HeaderRecord":
            var header = e.Attribute("Header").Value;
            var key = e.Attribute("Key").Value;
            var valueFormat = e.Attribute("ValueFormat")?.Value;
            var r = (Record)new HeaderRecord(header, key);
            foreach (var pair in e.ToKeyValuePairs("SummaryFormula"))
              r.SummaryFormula.Add(pair.Key, pair.Value);
            r.Format.FromXElement(e);
            yield return r;
            break;
          case "TimeSeriesRecord":
            header = e.Attribute("Header").Value;
            key = e.Attribute("Key").Value;
            valueFormat = e.Attribute("ValueFormat")?.Value;
            r = new TimeSeriesRecord(key, header, valueFormat);
            foreach (var pair in e.ToKeyValuePairs("SummaryFormula"))
              r.SummaryFormula.Add(pair.Key, pair.Value);
            r.Format.FromXElement(e);
            yield return r;
            break;
          case "CalculatedTimeSeriesRecord":
            header = e.Attribute("Header").Value;
            key = e.Attribute("Key").Value;
            var formula = e.Attribute("Formula").Value;
            valueFormat = e.Attribute("ValueFormat")?.Value;
            r = new CalculatedTimeSeriesRecord(key, header, formula, valueFormat);
            foreach (var pair in e.ToKeyValuePairs("SummaryFormula"))
              r.SummaryFormula.Add(pair.Key, pair.Value);
            r.Format.FromXElement(e);
            yield return r;
            break;
          default:
            throw new NotSupportedException($"Unable to deserialize element '{e.Name}'.");
        }
      }
    }

    private static IEnumerable<TimePointAxis> ToTimePointAxes(this XElement e)
    {
      if (e == null)
      {
        yield break;
      }

      foreach (var xElement in e.Elements("TimePointAxis"))
      {
        var header = xElement.Attribute("Header").Value;
        var timePointFormat = xElement.Attribute("TimePointFormat")?.Value;
        var timePointType = (TimePointType)Enum.Parse(typeof(TimePointType), xElement.Attribute("TimePointType").Value);
        var axis = new TimePointAxis(header, timePointType, timePointFormat);
        axis.Format.FromXElement(e);

        yield return axis;
      }
    }

    private static IEnumerable<Summary> ToSummaries(this XElement xElement)
    {
      if (xElement == null)
      {
        yield break;
      }

      foreach (var e in xElement.Elements("Summary"))
      {
        var key = e.Attribute("Key").Value;
        var header = e.Attribute("Header").Value;
        var valueFormat = e.Attribute("ValueFormat")?.Value;
        var summary = new Summary(key, header, valueFormat);
        summary.Format.FromXElement(e);

        yield return summary;
      }
    }

    private static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(this XElement xElement, string name)
    {
      foreach (var e in xElement.Elements(name))
      {
        var key = e.Attribute("SummaryKey").Value;
        var value = e.Attribute("Formula").Value;
        yield return new KeyValuePair<string, string>(key, value);
      }
    }

    // Obj => XElement
    private static XElement ToXElement(Record record)
    {
      switch (record)
      {
        case HeaderRecord r:
          return new XElement("HeaderRecord",
                  new XAttribute("Key", r.Key),
                  new XAttribute("Header", r.Header),
                  r.ValueFormat.ToAttribute("ValueFormat"),
                  r.Format.ToAttributes(),
                  r.SummaryFormula.ToElements("SummaryFormula"));
        case TimeSeriesRecord r:
          return new XElement("TimeSeriesRecord",
                  new XAttribute("Key", r.Key),
                  new XAttribute("Header", r.Header),
                  r.ValueFormat.ToAttribute("ValueFormat"),
                  r.Format.ToAttributes(),
                  r.SummaryFormula.ToElements("SummaryFormula"));
        case CalculatedTimeSeriesRecord r:
          return new XElement("CalculatedTimeSeriesRecord",
                  new XAttribute("Key", r.Key),
                  new XAttribute("Header", r.Header),
                  new XAttribute("Formula", r.Formula),
                  r.ValueFormat.ToAttribute("ValueFormat"),
                  r.Format.ToAttributes(),
                  r.SummaryFormula.ToElements("SummaryFormula"));
        default:
          throw new NotSupportedException($"Serializing record type '{record.GetType().Name}' is not supported.");
      }
    }

    private static IEnumerable<XElement> ToElements(this Dictionary<string, string> dict, string name)
    {
      return dict.Select(x => new XElement(name,
                            new XAttribute("SummaryKey", x.Key),
                            new XAttribute("Formula", x.Value)));
    }

    private static XElement ToXElement(Summary summary)
      => new XElement("Summary",
        new XAttribute("Key", summary.Key),
        new XAttribute("Header", summary.Header),
        ToAttributes(summary.Format),
        summary.ValueFormat.ToAttribute("ValueFormat"));

    private static XElement ToXElement(Report report)
      => new XElement("Report",
          new XAttribute("Version", "1"),
          new XAttribute("Orientation", report.Orientation),
          new XElement("RowHeaderFormat", ToAttributes(report.RowHeaderFormat)),
          new XElement("ColumnHeaderFormat", ToAttributes(report.ColumnHeaderFormat)),
          new XElement("Axis", report.Axes.Select(ToXElement)),
          new XElement("Body", report.Body.Select(ToXElement)),
          new XElement("Summaries", report.Summary.Select(ToXElement))
          );

    private static XElement ToXElement(TimePointAxis axis)
      => new XElement("TimePointAxis",
        new XAttribute("Header", axis.Header),
        axis.TimePointFormat.ToAttribute("TimePointFormat"),
        new XAttribute("TimePointType", axis.TimePointType),
        ToAttributes(axis.Format));

    private static XAttribute? ToAttribute(this string? str, string name) => str != null ? new XAttribute(name, str) : null;

    private static IEnumerable<XAttribute> ToAttributes(this Format format)
    {
      if (format.HasBackgroundModified)
        yield return new XAttribute("Background", format.Background.ToHexCode());
      if (format.HasForegroundModified)
        yield return new XAttribute("Foreground", format.Foreground.ToHexCode());
      if (format.HasBoldModified)
        yield return new XAttribute("Bold", format.Bold);
      if (format.HasHorizontalAlignmentModified)
        yield return new XAttribute("HorizontalAlignment", format.HorizontalAlignment);
    }
  }
}
