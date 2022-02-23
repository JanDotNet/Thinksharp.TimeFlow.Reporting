using System.Collections.Generic;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace Thinksharp.TimeFlow.Reporting.Calculation
{
  internal class CollectVariablesVisitor : NodeVisitor<int>
  {
    private readonly List<string> variables;

    public CollectVariablesVisitor(List<string> variables)
    {
      this.variables = variables;
    }

    public override int Visit(VariableNode node)
    {
      variables.Add(node.Name);

      return base.Visit(node);
    }
  }
}
