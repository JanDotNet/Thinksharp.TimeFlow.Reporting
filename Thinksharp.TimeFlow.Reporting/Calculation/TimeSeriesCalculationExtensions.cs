using System;
using System.Collections.Generic;
using System.Linq;
using ThinkSharp.FormulaParsing;

namespace Thinksharp.TimeFlow.Reporting.Calculation
{

  internal static class TimeSeriesCalculationExtensions
  {
    public static void CheckIfTimeSeriesExist(this TimeFrame timeFrame, IEnumerable<TimeSeriesRecord> records)
    {
      foreach (var record in records)
      {
        if (timeFrame[record.Key] == null)
        {
          throw new ReportGenerationException($"Can not find time series '{record.Header}' (Key: '{record.Key}').");
        }
      }
    }

    public static void ExtendWithCalculatedTimeSeries(this TimeFrame timeFrame, IEnumerable<CalculatedTimeSeriesRecord> calculatedTimeSeriesRecords)
    {
      var parser = FormulaParser.CreateBuilder()
        .ConfigureValidationBehavior(b => b.DisableVariableNameValidation())
        .Build();
      
      var calculatedTimeSeriesList = ParseFormulas(calculatedTimeSeriesRecords, parser);      

      while (calculatedTimeSeriesList.Count > 0)
      {
        var cts = calculatedTimeSeriesList.Dequeue();

        var existingTimeSeries = new HashSet<string>(timeFrame.EnumerateNames());
        var notExistingDependencies = cts.DependentVariables.Where(variableName => !existingTimeSeries.Contains(variableName)).ToArray();

        if (notExistingDependencies.Length > 0)
        {
          // abort condition
          if (cts.TimeFrameLengthWhenLastChecked == timeFrame.Count)
          {
            throw new ReportGenerationException($"Unable to calculate formula: '{cts.Record.Formula}' because the dependent time series '{string.Join(", ", notExistingDependencies)}' are not available.");
          }
          cts.TimeFrameLengthWhenLastChecked = timeFrame.Count;
          calculatedTimeSeriesList.Enqueue(cts);
          continue;
        }

        var calculatedTimeSeries = TimeSeries.Factory.FromGenerator(timeFrame.Start, timeFrame.End, timeFrame.Frequency, tp =>
        {
          var variables = new Dictionary<string, double>();
          foreach (var ts in timeFrame.Where(ts => cts.DependentVariables.Contains(ts.Key)))
          {
            variables[ts.Key] = (double)(ts.Value[tp] ?? 0M);
          }

          var result = parser.Evaluate(cts.ParsedFormula, variables);

          if (!result.Success)
          {
            throw new ReportGenerationException($"Error while evaluating formula '{cts.Record.Formula}'. Error: {result.Error.Message}.");
          }

          return (decimal)result.Value;
        });

        timeFrame.Add(cts.Record.Key, calculatedTimeSeries);
      }
    }

    private static Queue<CalculatedTimeSeries> ParseFormulas(IEnumerable<CalculatedTimeSeriesRecord> calculatedTimeSeries, IFormulaParser parser)
    {
      var calculatedFormulas = new List<CalculatedTimeSeries>();
      foreach (var calculatedRecord in calculatedTimeSeries)
      {
        var parsedFormula = parser.Parse(calculatedRecord.Formula);

        if (!parsedFormula.Success)
        {
          throw new ReportGenerationException($"Invalid formula: '{calculatedRecord.Format}'. Error: {parsedFormula.Error.Message}");
        }

        var variables = new List<string>();
        parsedFormula.Value.Visit(new CollectVariablesVisitor(variables));

        calculatedFormulas.Add(new CalculatedTimeSeries(calculatedRecord, parsedFormula.Value, variables));
      }

      return new Queue<CalculatedTimeSeries>(calculatedFormulas);
    }

    public static IEnumerable<SummaryResult> CalculateSummary(this TimeFrame timeFrame, IEnumerable<Record> records)
    {
      var parser = FormulaParser.CreateBuilder().ConfigureValidationBehavior(b =>
      {
        b.DisableVariableNameValidation();
        b.DisableFunctionNameValidation();
      }).Build();

      var visitor = new EvaluateTimeSeriesVisitor(timeFrame, Report.Config.TimeSeriesFunctions);

      foreach (var record in records)
      {
        foreach (var summaryKeyFormula in record.SummaryFormula)
        {
          var summaryKey = summaryKeyFormula.Key;
          var formula = summaryKeyFormula.Value.Trim();

          TimeSeries aggTimeSeries;
          var function = Report.Config.TimeSeriesFunctions.FirstOrDefault(f => f.Name == formula);
          if (function != null)
          {
            aggTimeSeries = function.Evaluate(timeFrame, new[] { timeFrame[record.Key] });
          }
          else
          {
            var node = parser.Parse(formula);
            if (!node.Success)
            {
              throw new ReportGenerationException($"Unable to parse formula '{formula}': {node.Error.Message}.");
            }

            aggTimeSeries = node.Value.Visit(visitor);
          }
          if (aggTimeSeries.Count > 1)
          {
            throw new ReportGenerationException($"Summary formula ('{formula}') requires an aggregation functions to get one single value!");
          }

          yield return new SummaryResult(summaryKey, record.Key, aggTimeSeries.FirstOrDefault()?.Value);
        }
      }
    }
  }
}
