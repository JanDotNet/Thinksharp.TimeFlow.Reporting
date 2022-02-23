using System;
using System.Collections.Generic;
using System.Linq;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace Thinksharp.TimeFlow.Reporting.Calculation
{
  internal class EvaluateTimeSeriesVisitor : NodeVisitor<TimeSeries>
  {
    private readonly TimeFrame timeFrame;
    private readonly IDictionary<string, Func<TimeFrame, TimeSeries[], TimeSeries>> timeSeriesFunctions;

    public EvaluateTimeSeriesVisitor(TimeFrame timeFrame, IEnumerable<TimeSeriesFunction> timeSeriesFunctions)
    {
      this.timeFrame = timeFrame;
      this.timeSeriesFunctions = timeSeriesFunctions.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.Last().Evaluate);
    }

    public override TimeSeries Visit(DecimalNode node)
    {
      return TimeSeries.Factory.FromValue((decimal)node.Value, timeFrame.Start, timeFrame.End, timeFrame.Frequency, timeFrame.TimeZone);
    }

    public override TimeSeries Visit(IntegerNode node)
    {
      return TimeSeries.Factory.FromValue((decimal)node.Value, timeFrame.Start, timeFrame.End, timeFrame.Frequency, timeFrame.TimeZone);
    }

    public override TimeSeries Visit(VariableNode node)
    {
      var ts = timeFrame[node.Name];

      if (ts == null)
      {
        throw new InvalidOperationException($"TimeSeries with name '{node.Name}' is not available.");
      }

      return ts;
    }

    public override TimeSeries Visit(ConstantNode node)
    {
      throw new NotSupportedException("Constants are not supported yet.");
    }

    public override TimeSeries Visit(BinaryOperatorNode node)
    {
      var leftValue = node.LeftNode.Visit(this);
      var rightValue = node.RightNode.Visit(this);

      var res =  leftValue.JoinFull(rightValue, (l, r) =>
      {
        if (l == null || r == null)
        {
          return null;
        }
        var lv = (double)l.Value;
        var rv = (double)r.Value;
        var result = (decimal?)node.BinaryOperator.Evaluate(lv, rv);
        return result;
      });
      return res;
    }

    public override TimeSeries Visit(PowerNode node)
    {
      var tsBase = node.BaseNode.Visit(this);
      var tsExponent = node.ExponentNode.Visit(this);

      return tsBase.JoinFull(tsExponent, (l, r) => l == null || r == null ? null : (decimal?)Math.Pow((double)l.Value, (double)r.Value));
    }

    public override TimeSeries Visit(SignedNode node)
    {
      var value = node.Node.Visit(this);

      return node.Sign == Sign.Minus ? value * -1 : value;
    }

    public override TimeSeries Visit(FunctionNode node)
    {
      var parameters = node.Parameters.Select(n => n.Visit(this)).ToArray();

      return this.timeSeriesFunctions[node.FunctionName](this.timeFrame, parameters);
    }
  }
}
