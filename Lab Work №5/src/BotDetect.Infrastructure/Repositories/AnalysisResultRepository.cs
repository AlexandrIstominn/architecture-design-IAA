using BotDetect.Domain.Entities;
using BotDetect.Domain.Repositories;
using BotDetect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BotDetect.Infrastructure.Repositories;

public class AnalysisResultRepository : IAnalysisResultRepository
{
    private readonly BotDetectDbContext _db;

    public AnalysisResultRepository(BotDetectDbContext db) => _db = db;

    public IEnumerable<AnalysisResult> GetByHistory(int historyId) =>
        _db.AnalysisResults
            .Include(r => r.Account)
            .Include(r => r.BotRating)
            .Where(r => r.HistoryId == historyId)
            .ToList();

    public void Save(AnalysisResult result)
    {
        if (result.AnalysisDate.Kind == DateTimeKind.Unspecified)
        result.AnalysisDate = DateTime.SpecifyKind(result.AnalysisDate, DateTimeKind.Utc);

        if (result.ResultId == 0)
            _db.AnalysisResults.Add(result);
        else
            _db.Update(result);
        _db.SaveChanges();
    }
}
