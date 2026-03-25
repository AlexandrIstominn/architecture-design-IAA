using BotDetect.Domain.Entities;

namespace BotDetect.Domain.Repositories;

public interface IAnalysisHistoryRepository
{
    AnalysisHistory? GetById(int id);
    void Save(AnalysisHistory history);
    IEnumerable<AnalysisHistory> GetByUser(int userId);
}
