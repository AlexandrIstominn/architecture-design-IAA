namespace BotDetect.Domain.Entities;

public class BotRating
{
    public int RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
    public int RatingValue { get; set; }

    public ICollection<AnalysisResult> AnalysisResults { get; set; } = [];
}
