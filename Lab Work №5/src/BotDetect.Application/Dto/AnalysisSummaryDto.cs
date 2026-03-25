namespace BotDetect.Application.Dto;

public record AnalysisSummaryDto(
    int HistoryId,
    string Status,
    DateTime AnalysisDate,
    IEnumerable<AccountResultDto> Results);

public record AccountResultDto(
    int AccountId,
    string Username,
    string SocialNetwork,
    int BotRatingValue,
    string BotRatingDescription,
    string Status);
