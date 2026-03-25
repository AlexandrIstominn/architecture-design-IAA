using BotDetect.Application.Dto;
using BotDetect.Domain.Repositories;

namespace BotDetect.Application.Services;

public class AnalysisResultService : IAnalysisResultService
{
    private readonly IAnalysisHistoryRepository _history;
    private readonly IAnalysisResultRepository _results;

    public AnalysisResultService(
        IAnalysisHistoryRepository history,
        IAnalysisResultRepository results)
    {
        _history = history;
        _results = results;
    }

    public AnalysisSummaryDto GetSummary(int historyId, int userId)
    {
        var history = _history.GetById(historyId)
            ?? throw new KeyNotFoundException("Задача анализа не найдена");

        if (history.UserId != userId)
            throw new UnauthorizedAccessException("Доступ запрещён");

        var results = _results.GetByHistory(historyId);
        var accountDtos = results.Select(r => new AccountResultDto(
            r.Account.AccountId,
            r.Account.Username,
            r.Account.SocialNetwork,
            r.BotRating.RatingValue,
            r.BotRating.Description,
            r.Status
        ));

        return new AnalysisSummaryDto(
            history.HistoryId,
            history.Status,
            history.AnalysisDate,
            accountDtos
        );
    }
}
