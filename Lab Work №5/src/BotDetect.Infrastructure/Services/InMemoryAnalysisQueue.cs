using BotDetect.Domain.Services;
using System.Collections.Concurrent;

namespace BotDetect.Infrastructure.Services;

public class InMemoryAnalysisQueue : IAnalysisQueue
{
    private readonly ConcurrentQueue<int> _queue = new();

    public void Enqueue(int historyId) => _queue.Enqueue(historyId);

    public int? Dequeue() => _queue.TryDequeue(out var id) ? id : null;
}
