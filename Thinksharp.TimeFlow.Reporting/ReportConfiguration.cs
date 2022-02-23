using System;
using System.Collections.Generic;
using System.Linq;
using Thinksharp.TimeFlow.Reporting.Calculation;

namespace Thinksharp.TimeFlow.Reporting
{
  public interface IReportConfiguration
  {
    void ClearFunctions();
    void AddFunction(string name, Func<TimeFrame, TimeSeries[], TimeSeries> evaluator);
  }

  internal class ReportConfiguration : IReportConfiguration
  {
    private readonly List<TimeSeriesFunction> timeSeriesFunctions = new List<TimeSeriesFunction>();
    public ReportConfiguration()
    {
      this.InitializeFunctions();
    }

    private void InitializeFunctions()
    {
      ((IReportConfiguration)this).AddFunction("max", (tf, parameters) =>
      {
        if (parameters.Length == 1)
        {
          return parameters[0];
        }

        var ts = parameters[0];

        for (int i = 1; i < parameters.Length; i++)
        {
          ts = ts.JoinFull(parameters[i], (l, r) => l == null ? r
            : r == null ? l
            : l.Value < r.Value ? r.Value : l.Value);
        }

        return ts;
      });

      ((IReportConfiguration)this).AddFunction("min", (tf, parameters) =>
      {
        if (parameters.Length == 1)
        {
          return parameters[0];
        }

        var ts = parameters[0];

        for (int i = 1; i < parameters.Length; i++)
        {
          ts = ts.JoinFull(parameters[i], (l, r) => l == null ? r
            : r == null ? l
            : l.Value < r.Value ? l.Value : r.Value);
        }

        return ts;
      });

      ((IReportConfiguration)this).AddFunction("MAX", (tf, parameters) =>
      { 
        var agg = parameters.SelectMany(ts => ts.Values).Where(v => v.HasValue).DefaultIfEmpty(0).Max();
        return TimeSeries.Factory.FromValue(agg, tf.Start, 1, tf.Frequency, tf.TimeZone);
      });

      ((IReportConfiguration)this).AddFunction("MIN", (tf, parameters) =>
      {
        var agg = parameters.SelectMany(ts => ts.Values).Where(v => v.HasValue).DefaultIfEmpty(0).Min();
        return TimeSeries.Factory.FromValue(agg, tf.Start, 1, tf.Frequency, tf.TimeZone);
      });

      ((IReportConfiguration)this).AddFunction("SUM", (tf, parameters) =>
      {
        var agg = parameters.Select(ts => ts.Values.Where(v => v.HasValue).Select(v => v.Value).Sum()).Sum();
        return TimeSeries.Factory.FromValue(agg, tf.Start, 1, tf.Frequency, tf.TimeZone);
      });

      ((IReportConfiguration)this).AddFunction("AVERAGE", (tf, parameters) =>
        {
          var agg = parameters.SelectMany(ts => ts.Values).Where(v => v.HasValue).Average();
          return TimeSeries.Factory.FromValue(agg, tf.Start, 1, tf.Frequency, tf.TimeZone);
        });

      ((IReportConfiguration)this).AddFunction("SUMPRODUCT", (tf, parameters) =>
        {
          if (parameters.Length != 2)
          {
            throw new ArgumentException("Function 'SUMPRODUCT' requires 2 arguments.");
          }

          var prod = parameters[0] * parameters[1];
          var agg = prod.Values.Where(v => v.HasValue).Select(v => v.Value).Sum();
          return TimeSeries.Factory.FromValue(agg, tf.Start, 1, tf.Frequency, tf.TimeZone);
        });
    }

    internal IEnumerable<TimeSeriesFunction> TimeSeriesFunctions => this.timeSeriesFunctions;

    void IReportConfiguration.AddFunction(string name, Func<TimeFrame, TimeSeries[], TimeSeries> evaluator)
    {
      timeSeriesFunctions.Add(new TimeSeriesFunction(name, evaluator));
    }

    void IReportConfiguration.ClearFunctions()
    {
      this.timeSeriesFunctions.Clear();
    }
  }
}
