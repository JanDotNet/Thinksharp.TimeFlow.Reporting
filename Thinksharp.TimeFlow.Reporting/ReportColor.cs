using System;
using System.Text.RegularExpressions;

namespace Thinksharp.TimeFlow.Reporting
{
  public class ReportColor
  {
    private static readonly Regex colorHexCodeRegex = new Regex("^#(?<a>[0-9A-Fa-f]{2})?(?<r>[0-9A-Fa-f]{2})(?<g>[0-9A-Fa-f]{2})(?<b>[0-9A-Fa-f]{2})$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private ReportColor(byte a, byte r, byte g, byte b)
    {
      A = a;
      R = r;
      G = g;
      B = b;
    }

    public static ReportColor FromArgb(byte a, byte r, byte g, byte b) => new ReportColor(a, r, g, b);

    public string ToHexCode()
    {
      var hasA = this.A != 255;

      var a = this.A.ToString("X2");
      var r = this.R.ToString("X2");
      var g = this.G.ToString("X2");
      var b = this.B.ToString("X2");

      return hasA ? $"#{a}{r}{g}{b}" : $"#{r}{g}{b}";
    }

    public static ReportColor FromHexCode(string colorCode)
    {
      var match = colorHexCodeRegex.Match(colorCode);
      if (!match.Success)
      {
        throw new FormatException($"String '{colorCode}' is not a valid color hex code. Accepted formats are '#FFFFFF' (#rrggbb) or '#FFFFFFFF' (#aarrggbb)");
      }

      var hasA = match.Groups["a"].Length > 0;
      var a = hasA ? Convert.ToByte(match.Groups["a"].Value, 16) : (byte)255;
      var r = Convert.ToByte(match.Groups["r"].Value, 16);
      var g = Convert.ToByte(match.Groups["g"].Value, 16);
      var b = Convert.ToByte(match.Groups["b"].Value, 16);

      return new ReportColor(a, r, g, b);
    }

    public byte A { get; }
    public byte R { get; }
    public byte G { get; }
    public byte B { get; }

    public static ReportColor Black { get; } = FromHexCode("#000000");
    public static ReportColor White { get; } = FromHexCode("#FFFFFF");

    public static ReportColor Blue { get; } = FromHexCode("#5B9BD5");
  }
}