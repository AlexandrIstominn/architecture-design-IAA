namespace BotDetect.Domain.Services;

public interface IAnalysisQueue
{
    void Enqueue(int historyId);
    int? Dequeue();
}
