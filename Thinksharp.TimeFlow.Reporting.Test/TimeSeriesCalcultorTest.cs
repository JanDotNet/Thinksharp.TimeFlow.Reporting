using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thinksharp.TimeFlow.Reporting.Calculation;

namespace Thinksharp.TimeFlow.Reporting.Test
{
  [TestClass]
  public class TimeSeriesCalcultorTest
  {
    [TestMethod]
    [ExpectedException(typeof(ReportGenerationException))]
    public void TestErrouseFormula()
    {
      var tf = new TimeFrame();

      var cr = new CalculatedTimeSeriesRecord("CT", "HEader", "1+");
      new TimeFrame().ExtendWithCalculatedTimeSeries(new[] { cr });
    }

    [TestMethod]
    [ExpectedException(typeof (ReportGenerationException))]
    public void TestMissingTimeSeries()
    {
      var tf = new TimeFrame();
      tf.Add("TS1", TimeSeries.Factory.FromValue(1, new DateTime(2021, 01, 01), 10, Period.Day));
      tf.Add("TS2", TimeSeries.Factory.FromValue(1, new DateTime(2021, 01, 01), 10, Period.Day));

      var cr = new TimeSeriesRecord("TS3", "Header");
      tf.CheckIfTimeSeriesExist(new[] { cr });
    }

    [TestMethod]    
    public void TestCalculatedTimeSeries()
    {
      var tf = new TimeFrame();
      tf.Add("TS1", TimeSeries.Factory.FromValue(1, new DateTime(2021, 01, 01), 10, Period.Day));
      tf.Add("TS2", TimeSeries.Factory.FromValue(1, new DateTime(2021, 01, 01), 10, Period.Day));

      var cr = new CalculatedTimeSeriesRecord("TS3", "Header", "TS1+TS2");
      tf.ExtendWithCalculatedTimeSeries(new[] { cr });

      Assert.IsTrue(tf.Count == 3);
      Assert.IsTrue(tf["TS3"] != null);
      Assert.IsTrue(tf["TS3"].All(x => x.Value == 2M));
    }

    [TestMethod]
    public void TestCalculatedTimeSeries_WithDependencies()
    {
      var tf = new TimeFrame();
      tf.Add("TS1", TimeSeries.Factory.FromValue(1, new DateTime(2021, 01, 01), 10, Period.Day));

      var cr1 = new CalculatedTimeSeriesRecord("TS2", "Header", "TS1+1");
      var cr2 = new CalculatedTimeSeriesRecord("TS3", "Header", "TS1+TS2");
      tf.ExtendWithCalculatedTimeSeries(new[] { cr1, cr2 });

      Assert.IsTrue(tf.Count == 3);
      Assert.IsTrue(tf["TS3"] != null);
      Assert.IsTrue(tf["TS3"].All(x => x.Value == 3M));
    }

    [TestMethod]
    public void TestCalculatedTimeSeries_WithDependenciesInverted()
    {
      var tf = new TimeFrame();
      tf.Add("TS1", TimeSeries.Factory.FromValue(1, new DateTime(2021, 01, 01), 10, Period.Day));

      var cr1 = new CalculatedTimeSeriesRecord("TS2", "Header", "TS1+1");
      var cr2 = new CalculatedTimeSeriesRecord("TS3", "Header", "TS1+TS2");
      tf.ExtendWithCalculatedTimeSeries(new[] { cr2, cr1 });

      Assert.IsTrue(tf.Count == 3);
      Assert.IsTrue(tf["TS3"] != null);
      Assert.IsTrue(tf["TS3"].All(x => x.Value == 3M));
    }

    [TestMethod]
    [ExpectedException(typeof(ReportGenerationException))]
    public void TestCalculatedTimeSeries_WithCircularReference()
    {
      var tf = new TimeFrame();
      tf.Add("TS1", TimeSeries.Factory.FromValue(1, new DateTime(2021, 01, 01), 10, Period.Day));

      var cr1 = new CalculatedTimeSeriesRecord("TS2", "Header", "TS3");
      var cr2 = new CalculatedTimeSeriesRecord("TS3", "Header", "TS4");
      var cr3 = new CalculatedTimeSeriesRecord("TS4", "Header", "TS5");
      var cr4 = new CalculatedTimeSeriesRecord("TS5", "Header", "TS2");
      tf.ExtendWithCalculatedTimeSeries(new[] { cr1, cr2, cr3, cr4 });

      Assert.IsTrue(tf.Count == 3);
      Assert.IsTrue(tf["TS3"] != null);
      Assert.IsTrue(tf["TS3"].All(x => x.Value == 3M));
    }
  }
}
