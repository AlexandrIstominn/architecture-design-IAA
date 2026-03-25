using BotDetect.Domain.Entities;

namespace BotDetect.Domain.Repositories;

public interface IAnalysisResultRepository
{
    IEnumerable<AnalysisResult> GetByHistory(int historyId);
    void Save(AnalysisResult result);
}
