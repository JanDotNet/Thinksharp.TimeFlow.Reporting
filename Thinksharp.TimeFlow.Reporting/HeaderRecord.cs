using System;

namespace Thinksharp.TimeFlow.Reporting
{
  public class HeaderRecord : Record
  {
    public HeaderRecord(string header)
      : base(Guid.NewGuid().ToString(), header)
    { 
    }
  }
}