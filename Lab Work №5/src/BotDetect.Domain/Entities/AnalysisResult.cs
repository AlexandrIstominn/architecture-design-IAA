namespace BotDetect.Domain.Entities;

public class AnalysisResult
{
    public int ResultId { get; set; }
    public int HistoryId { get; set; }
    public int AccountId { get; set; }
    public int RatingId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime AnalysisDate { get; set; }

    public AnalysisHistory AnalysisHistory { get; set; } = null!;
    public Account Account { get; set; } = null!;
    public BotRating BotRating { get; set; } = null!;
}
