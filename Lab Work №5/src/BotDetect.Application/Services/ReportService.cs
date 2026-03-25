using BotDetect.Application.Dto;
using BotDetect.Domain.Entities;
using BotDetect.Domain.Repositories;

namespace BotDetect.Application.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reports;
    private readonly IAnalysisHistoryRepository _history;

    public ReportService(IReportRepository reports, IAnalysisHistoryRepository history)
    {
        _reports = reports;
        _history = history;
    }

    public int CreateReport(CreateReportRequest request, int userId)
    {
        var history = _history.GetById(request.HistoryId)
            ?? throw new KeyNotFoundException("История анализа не найдена");

        if (history.UserId != userId)
            throw new UnauthorizedAccessException("Доступ запрещён");

        var report = new Report
        {
            UserId = userId,
            ReportDate = DateTime.UtcNow,
            FileFormat = request.FileFormat
        };

        _reports.Save(report);
        return report.ReportId;
    }

    public ReportDto? GetReport(int reportId, int userId)
    {
        var report = _reports.GetById(reportId);
        if (report == null || report.UserId != userId)
            return null;

        return new ReportDto(
            report.ReportId,
            report.UserId,
            report.ReportDate,
            report.FileFormat,
            report.ExportedResults.Select(e => new ExportedResultDto(
                e.ExportId,
                e.FileLocation,
                e.Format,
                e.ExportDate
            ))
        );
    }
}
