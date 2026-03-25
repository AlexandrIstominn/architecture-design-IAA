using BotDetect.Domain.Repositories;
using BotDetect.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BotDetect.Api.Controllers;

/// <summary>API для BotDetectionService — получение задач из очереди и сохранение результатов</summary>
[ApiController]
[Route("api/bot-detection")]
public class BotDetectionController : ControllerBase
{
    private readonly IAnalysisQueue _queue;
    private readonly IAnalysisHistoryRepository _historyRepository;
    private readonly IAnalysisResultRepository _resultRepository;
    private readonly IBotRatingRepository _ratingRepository;

    public BotDetectionController(
        IAnalysisQueue queue,
        IAnalysisHistoryRepository historyRepository,
        IAnalysisResultRepository resultRepository,
        IBotRatingRepository ratingRepository)
    {
        _queue = queue;
        _historyRepository = historyRepository;
        _resultRepository = resultRepository;
        _ratingRepository = ratingRepository;
    }

    /// <summary>Получение следующей задачи из очереди (GET)</summary>
    [HttpGet("dequeue")]
    public IActionResult DequeueTask()
    {
        var historyId = _queue.Dequeue();
        if (historyId == null)
            return NoContent();

        var history = _historyRepository.GetById(historyId.Value);
        if (history == null)
            return NoContent();

        var results = _resultRepository.GetByHistory(historyId.Value);
        if (!results.Any())
            return NoContent();

        return Ok(new
        {
            HistoryId = history.HistoryId,
            UserId = history.UserId,
            SocialNetwork = history.SocialNetwork,
            Accounts = results.Select(r => new
            {
                r.AccountId,
                r.ResultId,
                r.Account.Username
            })
        });
    }

    /// <summary>Сохранение результата анализа (POST)</summary>
    [HttpPost("results")]
    public IActionResult SaveResult([FromBody] SaveResultRequest request)
    {
        var result = _resultRepository.GetByHistory(request.HistoryId)
            .FirstOrDefault(r => r.ResultId == request.ResultId);

        if (result == null)
            return NotFound();

        var rating = _ratingRepository.GetById(request.RatingId);
        if (rating == null)
            return BadRequest("Invalid rating");

        result.RatingId = request.RatingId;
        result.Status = request.Status;
        _resultRepository.Save(result);

        var history = _historyRepository.GetById(request.HistoryId)!;
        var allResults = _resultRepository.GetByHistory(request.HistoryId);
        if (allResults.All(r => r.Status != "Pending"))
        {
            history.Status = "Completed";
            _historyRepository.Save(history);
        }

        return Ok();
    }
}

public record SaveResultRequest(int HistoryId, int ResultId, int RatingId, string Status);
