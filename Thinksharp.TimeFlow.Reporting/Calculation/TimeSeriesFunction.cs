using System;

namespace Thinksharp.TimeFlow.Reporting.Calculation
{
  internal class TimeSeriesFunction
  {
    private readonly Func<TimeSeries[], TimeSeries> evaluationFunction;

    public TimeSeriesFunction(string name, Func<TimeFrame, TimeSeries[], TimeSeries> evaluationFunction)
    {
      Name = name; 
      Evaluate = evaluationFunction;
    }

    public string Name { get; }

    public Func<TimeFrame, TimeSeries[], TimeSeries> Evaluate { get; }
  }
}
