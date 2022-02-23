using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Thinksharp.TimeFlow.Reporting.Calculation;

namespace Thinksharp.TimeFlow.Reporting.Test
{
  [TestClass]
  public  class SummaryAggregationTest
  {
    [TestMethod]
    public void TestAggreagation_min_max_MIN_MAX()
    {
      var tf = new TimeFrame();
      tf.Add("TS1", TimeSeries.Factory.FromValues(new decimal?[] { 1, 2, 1, 2, 1 }, new DateTime(2021, 01, 01), Period.Day));
      tf.Add("TS2", TimeSeries.Factory.FromValues(new decimal?[] { 2, 3, 3, 3, 2 }, new DateTime(2021, 01, 01), Period.Day));

      var ts1r = new TimeSeriesRecord("TS1", "TS1");
      var ts2r = new TimeSeriesRecord("TS2", "TS2");
      var ts3r = new CalculatedTimeSeriesRecord("TS3", "TS3","max(ts1, ts2)");
      ts3r.SummaryFormula.Add("Max", "MAX(max(TS1, TS2))");
      var ts4r = new CalculatedTimeSeriesRecord("TS4", "TS4", "min(ts1, ts2)");
      ts4r.SummaryFormula.Add("Min", "MIN(min(TS1, TS2))");

      var result = tf.CalculateSummary(new[] { ts3r, ts4r })
        .OrderBy(r => r.SummaryKey).ThenBy(r => r.RecordKey)
        .ToArray();

      Assert.AreEqual(2, result.Length);

      Assert.AreEqual("Max", result[0].SummaryKey);
      Assert.AreEqual("TS3", result[0].RecordKey);
      Assert.AreEqual(3M, result[0].Value);

      Assert.AreEqual("Min", result[1].SummaryKey);
      Assert.AreEqual("TS4", result[1].RecordKey);
      Assert.AreEqual(1M, result[1].Value);
    }

    [TestMethod]
    public void TestAggreagation_MIN_MAX()
    {
      var tf = new TimeFrame();
      tf.Add("TS1", TimeSeries.Factory.FromValues(new decimal?[] { 1, 2, 1, 2, 1}, new DateTime(2021, 01, 01), Period.Day));
      tf.Add("TS2", TimeSeries.Factory.FromValues(new decimal?[] { 2, 1, 2, 1, 2 }, new DateTime(2021, 01, 01), Period.Day));

      var ts1r = new TimeSeriesRecord("TS1", "TS1");
      ts1r.SummaryFormula.Add("Max", "MAX");
      ts1r.SummaryFormula.Add("Min", "MIN");
      var ts2r = new TimeSeriesRecord("TS2", "TS2");
      ts2r.SummaryFormula.Add("Max", "MAX");
      ts2r.SummaryFormula.Add("Min", "MIN");

      var result = tf.CalculateSummary(new[] { ts1r, ts2r })
        .OrderBy(r => r.SummaryKey).ThenBy(r => r.RecordKey)
        .ToArray();

      Assert.AreEqual(4, result.Length);

      Assert.AreEqual("Max", result[0].SummaryKey);
      Assert.AreEqual("TS1", result[0].RecordKey);
      Assert.AreEqual(2M, result[0].Value);

      Assert.AreEqual("Max", result[1].SummaryKey);
      Assert.AreEqual("TS2", result[1].RecordKey);
      Assert.AreEqual(2M, result[1].Value);

      Assert.AreEqual("Min", result[2].SummaryKey);
      Assert.AreEqual("TS1", result[2].RecordKey);
      Assert.AreEqual(1M, result[2].Value);

      Assert.AreEqual("Min", result[3].SummaryKey);
      Assert.AreEqual("TS2", result[3].RecordKey);
      Assert.AreEqual(1M, result[3].Value);
    }

    [TestMethod]
    public void TestAggreagation_AggFunctionWithoutParameters()
    {
      var tf = new TimeFrame();
      tf.Add("Amount", TimeSeries.Factory.FromValues(new decimal?[] { 1, 2, 3, 4, 5 }, new DateTime(2021, 01, 01), Period.Day));
      tf.Add("Price", TimeSeries.Factory.FromValues(new decimal?[] { 2, 2, 2, 1, 1 }, new DateTime(2021, 01, 01), Period.Day));

      var amountRecord = new TimeSeriesRecord("Amount", "Amount");
      amountRecord.SummaryFormula.Add("Agg", "SUM");
      var priceRecord = new TimeSeriesRecord("Price", "Price");
      priceRecord.SummaryFormula.Add("Agg", "AVERAGE");

      var summary = new Summary("Agg", "Sum");

      var result = tf.CalculateSummary(new[] { amountRecord, priceRecord })
        .OrderBy(r => r.SummaryKey).ThenBy(r => r.RecordKey)
        .ToArray();

      Assert.AreEqual(2, result.Length);

      Assert.AreEqual("Agg", result[0].SummaryKey);
      Assert.AreEqual("Amount", result[0].RecordKey);
      Assert.AreEqual(15M, result[0].Value);

      Assert.AreEqual("Agg", result[1].SummaryKey);
      Assert.AreEqual("Price", result[1].RecordKey);
      Assert.AreEqual(1.6M, result[1].Value);
    }

    [TestMethod]
    public void TestAggreagation()
    {
      var tf = new TimeFrame();
      tf.Add("Amount", TimeSeries.Factory.FromValues(new decimal?[] { 1, 2, 3, 4, 5 }, new DateTime(2021, 01, 01), Period.Day));
      tf.Add("Price", TimeSeries.Factory.FromValues(new decimal?[] { 2, 2, 2, 1, 1 }, new DateTime(2021, 01, 01), Period.Day)) ;

      var amountRecord = new TimeSeriesRecord("Amount", "Amount");
      amountRecord.SummaryFormula.Add("Agg", "SUM(Amount)");
      var priceRecord = new TimeSeriesRecord("Price", "Price");
      priceRecord.SummaryFormula.Add("Agg", "SUMPRODUCT(Price, Amount)/SUM(Amount)");

      var summary = new Summary("Agg", "Sum");

      var result = tf.CalculateSummary(new[] { amountRecord, priceRecord })
        .OrderBy(r => r.SummaryKey).ThenBy(r => r.RecordKey)
        .ToArray();

      Assert.AreEqual(2, result.Length);

      Assert.AreEqual("Agg", result[0].SummaryKey);
      Assert.AreEqual("Amount", result[0].RecordKey);
      Assert.AreEqual(15M, result[0].Value);

      Assert.AreEqual("Agg", result[1].SummaryKey);
      Assert.AreEqual("Price", result[1].RecordKey);
      Assert.AreEqual(1.4M, result[1].Value);
    }
  }
}
