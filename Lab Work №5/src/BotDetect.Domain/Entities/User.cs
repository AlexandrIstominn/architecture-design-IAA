namespace BotDetect.Domain.Entities;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public ICollection<AnalysisHistory> AnalysisHistories { get; set; } = [];
    public ICollection<Report> Reports { get; set; } = [];
}
