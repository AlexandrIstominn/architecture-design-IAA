namespace BotDetect.Domain.Entities;

public class Report
{
    public int ReportId { get; set; }
    public int UserId { get; set; }
    public DateTime ReportDate { get; set; }
    public string FileFormat { get; set; } = string.Empty;

    public User User { get; set; } = null!;
    public ICollection<ExportedResult> ExportedResults { get; set; } = [];
}
