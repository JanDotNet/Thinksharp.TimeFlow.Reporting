using System;
using System.Collections.Generic;
using System.Text;

namespace Thinksharp.TimeFlow.Reporting.Iterator
{
  public class ReportCell
  {
    public ReportCell(object? value, string valueFormat, Format format)
    {
      Value = value;
      ValueFormat = valueFormat;
      Format = format;
    }

    public object? Value { get; }
    public string ValueFormat { get; }
    public Format Format { get; }
  }
}
