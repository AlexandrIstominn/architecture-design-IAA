using BotDetect.Application.Dto;

namespace BotDetect.Application.Services;

public interface IReportService
{
    int CreateReport(CreateReportRequest request, int userId);
    ReportDto? GetReport(int reportId, int userId);
}
