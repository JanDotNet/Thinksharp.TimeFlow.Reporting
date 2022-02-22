namespace Thinksharp.TimeFlow.Reporting
{
  public class Format
  {
    private bool bold = false;
    private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;
    private ReportColor background = ReportColor.White;
    private ReportColor foreground = ReportColor.Black;

    internal bool HasBoldModified { get; set; }
    internal bool HasHorizontalAlignmentModified { get; set; }
    internal bool HasBackgroundModified { get; set; }
    internal bool HasForegroundModified { get; set; }

    internal bool HasAnyModified => HasBackgroundModified || HasBoldModified || HasForegroundModified || HasHorizontalAlignmentModified;

    internal void Reset()
    {
      HasBoldModified = false;
      HasHorizontalAlignmentModified = false;
      HasBackgroundModified = false;
      HasForegroundModified = false;
    }

    public Format Merge(Format other)
    {
      if (!this.HasAnyModified)
      {
        return other;
      }

      if (!other.HasAnyModified)
      {
        return this;
      }

      return new Format
      {
        Bold = this.HasBoldModified ? this.Bold : other.Bold,
        HorizontalAlignment = this.HasHorizontalAlignmentModified ? this.HorizontalAlignment : other.HorizontalAlignment,
        Background = this.HasBackgroundModified ? this.Background : other.Background,
        Foreground = this.HasForegroundModified ? this.Foreground : other.Foreground,
      };
    }

    public bool Bold
    {
      get { return bold; }
      set 
      { 
        bold = value;
        HasBoldModified = true;
      }
    }
    public HorizontalAlignment HorizontalAlignment
    {
      get { return horizontalAlignment; }
      set
      {
        horizontalAlignment = value;
        HasHorizontalAlignmentModified = true;
      }
    }
    public ReportColor Background
    {
      get { return background; }
      set
      {
        background = value;
        HasBackgroundModified = true;
      }
    }
    public ReportColor Foreground
    {
      get { return foreground; }
      set
      {
        foreground = value;
        HasForegroundModified = true;
      }
    }

    public static Format Default() => new Format();
  }
}