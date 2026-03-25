namespace BotDetect.Domain.Entities;

public class AnalysisHistory
{
    public int HistoryId { get; set; }
    public int UserId { get; set; }
    public DateTime AnalysisDate { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Running, Completed, Failed
    public string SocialNetwork { get; set; } = string.Empty;

    public User User { get; set; } = null!;
    public ICollection<AnalysisResult> AnalysisResults { get; set; } = [];
}
