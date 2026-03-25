using BotDetect.Application.Dto;
using BotDetect.Domain.Entities;
using BotDetect.Domain.Repositories;
using BotDetect.Domain.Services;

namespace BotDetect.Application.Services;

public class AnalysisService : IAnalysisService
{
    private readonly IAnalysisHistoryRepository _history;
    private readonly IAccountRepository _accounts;
    private readonly IAnalysisResultRepository _results;
    private readonly IBotRatingRepository _ratings;
    private readonly IAnalysisQueue _queue;

    public AnalysisService(
        IAnalysisHistoryRepository history,
        IAccountRepository accounts,
        IAnalysisResultRepository results,
        IBotRatingRepository ratings,
        IAnalysisQueue queue)
    {
        _history = history;
        _accounts = accounts;
        _results = results;
        _ratings = ratings;
        _queue = queue;
    }

    public int EnqueueAnalysis(RunAnalysisRequest request, int userId)
    {
        var accounts = _accounts.GetOrCreateByUsernames(request.SocialNetwork, request.AccountIds).ToList();
        var defaultRating = _ratings.GetById(1) ?? throw new InvalidOperationException("Рейтинг по умолчанию не найден");

        var history = new AnalysisHistory
        {
            UserId = userId,
            SocialNetwork = request.SocialNetwork,
            AnalysisDate = DateTime.UtcNow,
            Status = "Pending"
        };

        _history.Save(history);

        foreach (var account in accounts)
        {
            _results.Save(new AnalysisResult
            {
                HistoryId = history.HistoryId,
                AccountId = account.AccountId,
                RatingId = defaultRating.RatingId,
                Status = "Pending",
                AnalysisDate = DateTime.UtcNow
            });
        }

        _queue.Enqueue(history.HistoryId);
        return history.HistoryId;
    }
}
