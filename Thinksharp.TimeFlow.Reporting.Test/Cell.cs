using System;
using System.Collections.Generic;

namespace Thinksharp.TimeFlow.Reporting.Test
{
  public class Cell
  {
    public Cell(int row, int col, object? value, string? valueFormat, Format format)
    {
      Row = row;
      Col = col;
      Value = value;
      ValueFormat = valueFormat;
      Format = format;
    }

    public int Row { get; }
    public int Col { get; }
    public object? Value { get; }
    public string? ValueFormat { get; }
    public Format Format { get; }

    public override bool Equals(object? obj)
    {
      return obj is Cell cell &&
             Row == cell.Row &&
             Col == cell.Col &&
             EqualityComparer<object?>.Default.Equals(Value, cell.Value);
             //ValueFormat == cell.ValueFormat &&
             //EqualityComparer<Format>.Default.Equals(Format, cell.Format);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Row, Col, Value, ValueFormat);
    }

    public override string ToString()
    {
      return $"Row: {Row}; Col: {Col}; Value: '{string.Format("{0:" + ValueFormat + "}", Value)}'; ValueFormat: '{ValueFormat}'";
    }
  }
}