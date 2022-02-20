using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Thinksharp.TimeFlow.Reporting
{
  public enum ReportOrientation
  {
    Horizontal,
    Vertical,
  }

  public enum HorizontalAlignment
  {
    Left,
    Center,
    Right,
  }

  public enum TimePointType
  {
    Start,
    End,
  }

  public enum ReportIteratorType
  {
    Decimal,
    DateTime,
    Text,
  }

  public class ReportColor
  {
    private static readonly Regex colorHexCodeRegex = new Regex("^#(?<a>[0-9A-Fa-f]{2})?(?<r>[0-9A-Fa-f]{2})(?<g>[0-9A-Fa-f]{2})(?<b>[0-9A-Fa-f]{2})$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private ReportColor(byte a, byte r, byte g, byte b)
    {
      A = a;
      R = r;
      G = g;
      B = b;
    }

    public static ReportColor FromArgb(byte a, byte r, byte g, byte b) => new ReportColor(a, r, g, b);

    public static ReportColor FromHexCode(string colorCode)
    {
      var match = colorHexCodeRegex.Match(colorCode);
      if (!match.Success)
      {
        throw new FormatException($"String '{colorCode}' is not a valid color hex code. Accepted formats are '#FFFFFF' (#rrggbb) or '#FFFFFFFF' (#aarrggbb)");
      }

      var hasA = match.Groups["a"].Length > 0;
      var a = hasA ? Convert.ToByte(match.Groups["a"].Value, 16) : (byte)255;
      var r = Convert.ToByte(match.Groups["r"].Value, 16);
      var g = Convert.ToByte(match.Groups["g"].Value, 16);
      var b = Convert.ToByte(match.Groups["b"].Value, 16);

      return new ReportColor(a, r, g, b);
    }

    public byte A { get; }
    public byte R { get; }
    public byte G { get; }
    public byte B { get; }

    public static ReportColor Black { get; } = FromHexCode("#000000");
    public static ReportColor White { get; } = FromHexCode("#FFFFFF");

    public static ReportColor Blue { get; } = FromHexCode("#5B9BD5");
  }

  public class Report
  {
    public ReportOrientation Orientation { get; set; } = ReportOrientation.Vertical;
    public Format? ColumnHeaderFormat { get; set; }
    public List<TimePointAxis> Axes { get; } = new List<TimePointAxis>();
    public List<Record> Body { get; } = new List<Record>();
    public List<Summary> Summary { get; } = new List<Summary>();
  }

  public class Format
  {
    public bool Bold { get; set; }
    public HorizontalAlignment HorizontalAlignment { get; set; }
    public ReportColor Background { get; set; } = ReportColor.White;
    public ReportColor Foreground { get; set; } = ReportColor.Black;

    public static Format Default() => new Format();
  }

  public abstract class Record
  {
    public Record(string key, string header, string? valueFormat = null)
    {
      Header = header;
      ValueFormat = valueFormat;
      Key = key;
    }
    public Format Format { get; } = Format.Default();
    public string Header { get; }
    public string Key { get; }
    public string? ValueFormat { get; }
  }

  public class TimeSeriesRecord : Record
  {
    public TimeSeriesRecord(string key, string header, string? valueFormat = null)
      : base(key, header, valueFormat)
    {
    }

    public Dictionary<string, string> AggregationFormula { get; } = new Dictionary<string, string>();
  }

  public class CalculatedRecord : Record
  {
    public CalculatedRecord(string key, string header, string formula)
      : base(key, header)
    {
      Formula = formula;
    }

    public string Formula { get; }

    public Dictionary<string, string> AggrgeationFormula { get; } = new Dictionary<string, string>();
  }

  public class HeaderRecord : Record
  {
    public HeaderRecord(string header)
      : base(Guid.NewGuid().ToString(), header)
    { 
    }
  }

  public class Summary
  {
    public Summary(string key, string header, string? valueFormat = null)
    {
      Key = key;
      Header = header;
      ValueFormat = valueFormat;
    }

    public Format Format { get; } = Format.Default();
    public string Header { get; }
    public string Key { get; }
    public string? ValueFormat { get; }
  }

  public class TimePointAxis
  {
    public TimePointAxis(string header, TimePointType timePointType, string? timePointFormat = null)
    {
      Header = header;
      TimePointType = timePointType;
      TimePointFormat = timePointFormat;
    }

    public string Header { get; }
    public TimePointType TimePointType { get; }
    public string? TimePointFormat { get; }
  }

  // Reporting

  public abstract class Column
  {
    public Column(Format? format, ReportIteratorType type)
    {
      Format = format;
      Type = type;
    }

    public Format? Format { get; }
    public ReportIteratorType Type { get; }

    public abstract object? GetCellValue(Row row);

    public Format GetFormat(Row row) => Format ?? row.Format;

    public abstract string? GetValueFormat(Row row);
  }

  public class TimeSeriesColumn : Column
  {
    public TimeSeriesColumn(Format? format, ReportIteratorType type) : base(format, type)
    {

    }

    public override object GetCellValue(Row row)
    {
      switch (row)
      {
        case TimePointAxisRow axisRow:
          return axisRow.Axis.Header;
        case RecordDataRow<TimeSeriesRecord> r:
          return r.Record.Header;
        case RecordDataRow<HeaderRecord> r:
          return r.Record.Header;
        default:
          throw new InvalidOperationException($"{row.GetType().Name} is not supported.");
      }
    }

    public override string? GetValueFormat(Row row)
    {
      switch (row)
      {
        case TimePointAxisRow axisRow:
          return axisRow.Axis.TimePointFormat;
        case RecordDataRow<TimeSeriesRecord> r:
          return r.Record.ValueFormat;
        default:
          return null;
      }
    }
  }

  public class SummaryColumn : Column
  {
    public SummaryColumn(Summary summary, Format? format, ReportIteratorType type) : base(format, type)
    {
      Summary = summary;
    }

    public Summary Summary { get; }

    public override object GetCellValue(Row row)
    {
      // TODO
      return this.Summary.Header;
    }

    public override string? GetValueFormat(Row row) => this.Summary.ValueFormat ?? row?.GetValueFormat();
  }

  public class TimePointAxisColumn : Column
  {
    private readonly TimeFrame timeFrame;
    private readonly DateTimeOffset end;

    public TimePointAxisColumn(Format? format, ReportIteratorType type) : base(format, type)
    {
    }

    public TimePointAxisColumn(TimeFrame timeFrame, DateTimeOffset timePoint, ReportIteratorType dataType) : base(null, dataType)
    {
      this.timeFrame = timeFrame;
      this.TimePoint = timePoint;
      this.end = timeFrame.AddPeriodTo(timePoint);
    }

    public DateTimeOffset TimePoint { get; }

    public override object? GetCellValue(Row row)
    {
      switch (row)
      {
        case TimePointAxisRow axisRow:
          switch (axisRow.Axis.TimePointType)
          {
            case TimePointType.Start:
              return TimePoint;
            case TimePointType.End:
              return this.end;
            default:
              throw new NotSupportedException($"TimePointType '{axisRow.Axis.TimePointType}' is not supported.");
          }
        case RecordDataRow<TimeSeriesRecord> r:
          var ts = this.timeFrame[r.Record.Key];
          if (ts == null)
          {
            throw new ReportGenerationException($"Can not find time series to key '{r.Record.Key}'.");
          }
          return ts[this.TimePoint];
        case RecordDataRow<HeaderRecord> r:
          return null;
        default:
          throw new InvalidOperationException($"Row type '{row.GetType().Name} is not supported.");
      }
    }

    public override string? GetValueFormat(Row row) => row?.GetValueFormat();
  }

  public class Row
  {
    public virtual Format Format { get; protected set; } = Format.Default();

    internal virtual string? GetValueFormat() => null;
  }

  public class TimePointAxisRow : Row
  {
    public TimePointAxisRow(TimePointAxis axis)
    {
      Axis = axis;
    }

    public TimePointAxis Axis { get; }

    internal override string? GetValueFormat() => this.Axis.TimePointFormat;
  }

  public class RecordDataRow<TRecord> : Row where TRecord : Record
  {
    public RecordDataRow(TRecord record)
    {
      Format = record.Format;
      Record = record;
    }

    public TRecord Record { get; }

    internal override string? GetValueFormat() => this.Record.ValueFormat;
  }

  public class HorizontalReportIterator
  {
    private readonly Report report;
    private readonly TimeFrame timeFrame;

    public HorizontalReportIterator(Report report, TimeFrame timeFrame)
    {
      this.report = report;
      this.timeFrame = timeFrame;
    }

    public IEnumerable<Column> EnumerateColumns()
    {
      // time series name headers
      yield return new TimeSeriesColumn(report.ColumnHeaderFormat, ReportIteratorType.Text);

      // summary header
      foreach (var summary in report.Summary)
      {
        yield return new SummaryColumn(summary, report.ColumnHeaderFormat, ReportIteratorType.Text);
      }

      // time series data columns
      foreach (var timePoint in this.timeFrame.EnumerateTimePoints())
      {
        yield return new TimePointAxisColumn(this.timeFrame, timePoint, ReportIteratorType.Decimal);
      }
    }

    public IEnumerable<Row> EnumerateHeaderRows()
    {
      foreach (var axis in report.Axes)
      {
        yield return new TimePointAxisRow(axis);
      }
    }

    public IEnumerable<Row> EnumerateDataRows()
    {
      foreach (var record in report.Body)
      {
        switch (record)
        {
          case TimeSeriesRecord r:
            yield return new RecordDataRow<TimeSeriesRecord>(r);
            break;
          case CalculatedRecord r:
            yield return new RecordDataRow<CalculatedRecord>(r);
            break;
          case HeaderRecord r:
            yield return new RecordDataRow<HeaderRecord>(r);
            break;
          default:
            throw new InvalidOperationException($"Reord type '{record.GetType().Name}' is not supported.");
        };
      }
    }
  }


  public class TimePointAxisColumn : Column 
  {
    public TimePointAxisColumn(Format? format) : base(format, ReportIteratorType)
  }

  public class VerticalReportIterator
  {
    private readonly Report report;
    private readonly TimeFrame timeFrame;

    public VerticalReportIterator(Report report, TimeFrame timeFrame)
    {
      this.report = report;
      this.timeFrame = timeFrame;
    }

    public IEnumerable<Column> EnumerateColumns()
    {
      foreach (var axis in report.Axes)
      {
        return new TimePointC
      }

      //// time series name headers
      //yield return new TimeSeriesColumn(report.ColumnHeaderFormat, ReportIteratorType.Text);

      //// summary header
      //foreach (var summary in report.Summary)
      //{
      //  yield return new SummaryColumn(summary, report.ColumnHeaderFormat, ReportIteratorType.Text);
      //}

      //// time series data columns
      //foreach (var timePoint in this.timeFrame.EnumerateTimePoints())
      //{
      //  yield return new TimePointColumn(this.timeFrame, timePoint, ReportIteratorType.Decimal);
      //}
    }

    public IEnumerable<Row> EnumerateHeaderRows()
    {
      foreach (var axis in report.Axes)
      {
        yield return new TimePointAxisRow(axis);
      }
    }

    public IEnumerable<Row> EnumerateDataRows()
    {
      foreach (var record in report.Body)
      {
        switch (record)
        {
          case TimeSeriesRecord r:
            yield return new RecordDataRow<TimeSeriesRecord>(r);
            break;
          case CalculatedRecord r:
            yield return new RecordDataRow<CalculatedRecord>(r);
            break;
          case HeaderRecord r:
            yield return new RecordDataRow<HeaderRecord>(r);
            break;
          default:
            throw new InvalidOperationException($"Reord type '{record.GetType().Name}' is not supported.");
        };
      }
    }
  }

  public class ReportGenerationException : Exception
  {
    public ReportGenerationException(string message, Exception? innerException = null) : base(message, innerException)
    { }
  }
}