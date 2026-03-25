namespace BotDetect.Domain.Entities;

public class Account
{
    public int AccountId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string SocialNetwork { get; set; } = string.Empty; // vk, tg, ig, x
    public string Status { get; set; } = string.Empty;

    public ICollection<AnalysisResult> AnalysisResults { get; set; } = [];
}
