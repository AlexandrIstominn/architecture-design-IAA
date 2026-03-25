namespace BotDetect.Domain.Entities;

public class ExportedResult
{
    public int ExportId { get; set; }
    public int ReportId { get; set; }
    public string FileLocation { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public DateTime ExportDate { get; set; }

    public Report Report { get; set; } = null!;
}
