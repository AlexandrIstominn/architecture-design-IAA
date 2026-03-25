using BotDetect.Application.Dto;

namespace BotDetect.Application.Services;

public interface IAnalysisResultService
{
    AnalysisSummaryDto GetSummary(int historyId, int userId);
}
