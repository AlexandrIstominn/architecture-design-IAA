using BotDetect.Application.Dto;

namespace BotDetect.Application.Services;

public interface IAnalysisService
{
    int EnqueueAnalysis(RunAnalysisRequest request, int userId);
}
