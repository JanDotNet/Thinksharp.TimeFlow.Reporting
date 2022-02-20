using System.Collections.Generic;

namespace Thinksharp.TimeFlow.Reporting.Iterator
{
  public interface IReportIterator
  {
    IEnumerable<Column> EnumerateColumns();
    IEnumerable<Row> EnumerateHeaderRows();
    IEnumerable<Row> EnumerateDataRows();
  }
}