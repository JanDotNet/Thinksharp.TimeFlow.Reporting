using System;

namespace Thinksharp.TimeFlow.Reporting
{
  public class HeaderRecord : Record
  {
    public HeaderRecord(string header, string? key = null)
      : base(key ?? Guid.NewGuid().ToString(), header)
    { 
    }
  }
}