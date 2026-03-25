using BotDetect.Domain.Entities;
using BotDetect.Domain.Repositories;
using BotDetect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BotDetect.Infrastructure.Repositories;

public class AnalysisHistoryRepository : IAnalysisHistoryRepository
{
    private readonly BotDetectDbContext _db;

    public AnalysisHistoryRepository(BotDetectDbContext db) => _db = db;

    public AnalysisHistory? GetById(int id) =>
        _db.AnalysisHistories.SingleOrDefault(h => h.HistoryId == id);

    public IEnumerable<AnalysisHistory> GetByUser(int userId) =>
        _db.AnalysisHistories.Where(h => h.UserId == userId).OrderByDescending(h => h.AnalysisDate).ToList();

    public void Save(AnalysisHistory history)
    {
        _db.Update(history);
        _db.SaveChanges();
    }
}
