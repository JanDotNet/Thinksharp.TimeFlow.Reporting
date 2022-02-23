using System.Collections.Generic;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace Thinksharp.TimeFlow.Reporting.Calculation
{
  internal class CalculatedTimeSeries
  {
    public CalculatedTimeSeries(CalculatedTimeSeriesRecord record, Node parsedFormula, List<string> dependentVariables)
    {
      Record = record;
      ParsedFormula = parsedFormula;
      DependentVariables = dependentVariables;
    }

    public CalculatedTimeSeriesRecord Record { get; }
    public Node ParsedFormula { get; }
    public List<string> DependentVariables { get; }
    public int TimeFrameLengthWhenLastChecked { get; set; } = -1;
  }
}
