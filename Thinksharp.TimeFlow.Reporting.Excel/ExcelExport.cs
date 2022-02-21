using ClosedXML.Excel;
using System;

namespace Thinksharp.TimeFlow.Reporting.Excel
{
  public static class ExcelExport
  {
    public static void ExportToExcel(Report report, TimeFrame timeframe, string path, string sheetName = "Report")
    {
      var iterator = report.CreateReportIterator(timeframe);

      using (var workbook = new XLWorkbook())
      {
        var worksheet = workbook.Worksheets.Add(sheetName);
        foreach (var col in iterator.EnumerateColumns())
        {
          foreach (var row in iterator.EnumerateHeaderRows())
          {
            WriteToExcelCell(worksheet, col, row);
          }

          foreach (var row in iterator.EnumerateDataRows())
          {
            WriteToExcelCell(worksheet, col, row);
          }

          worksheet.Column(col.Number).AdjustToContents();
        }
        workbook.SaveAs(path);
      }
    }

    private static void WriteToExcelCell(IXLWorksheet worksheet, Iterator.Column col, Iterator.Row row)
    {
      var value = col.GetCellValue(row);
      var valueFormat = col.GetValueFormat(row);
      var format = col.GetFormat(row);

      var xCell = worksheet.Cell(row.Number, col.Number);
      xCell.Style.Alignment.Horizontal = format.HorizontalAlignment.ToAlignmentHorizontal();
      xCell.Style.Fill.BackgroundColor = format.Background.ToXLColor();
      xCell.Style.Font.FontColor = format.Foreground.ToXLColor();
      xCell.Style.Font.Bold = format.Bold;

      if (value is string)
      {
        xCell.Value = value;
      }
      if (value is decimal)
      {
        xCell.Value = value;
        xCell.Style.NumberFormat.Format = valueFormat;        
      }
      else if (value is DateTimeOffset dto)
      {
        xCell.Value = dto.DateTime;
        xCell.Style.DateFormat.Format = valueFormat;
      }
    }
  }
}
