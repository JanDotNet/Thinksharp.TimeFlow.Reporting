using System;

namespace Thinksharp.TimeFlow.Reporting.Iterator
{

  public class ReportGenerationException : Exception
  {
    public ReportGenerationException(string message, Exception? innerException = null) : base(message, innerException)
    { }
  }
}