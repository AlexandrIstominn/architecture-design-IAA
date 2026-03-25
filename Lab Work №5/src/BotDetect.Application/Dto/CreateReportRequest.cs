namespace BotDetect.Application.Dto;

public record CreateReportRequest(int HistoryId, string FileFormat = "pdf");
