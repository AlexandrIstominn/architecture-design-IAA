using BotDetect.Application.Dto;
using BotDetect.Application.Services;
using BotDetect.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BotDetect.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly IAnalysisService _analysisService;
    private readonly IAnalysisResultService _resultService;
    private readonly IAnalysisHistoryRepository _historyRepository;

    public AnalysisController(
        IAnalysisService analysisService,
        IAnalysisResultService resultService,
        IAnalysisHistoryRepository historyRepository)
    {
        _analysisService = analysisService;
        _resultService = resultService;
        _historyRepository = historyRepository;
    }

    /// <summary>Запуск анализа аккаунтов (POST)</summary>
    [HttpPost]
    public IActionResult RunAnalysis([FromBody] RunAnalysisRequest request, [FromHeader(Name = "X-User-Id")] int userId = 1)
    {
        var historyId = _analysisService.EnqueueAnalysis(request, userId);
        return CreatedAtAction(nameof(GetAnalysisStatus), new { id = historyId }, new { historyId });
    }

    /// <summary>Получение статуса и результатов анализа (GET)</summary>
    [HttpGet("{id:int}")]
    public IActionResult GetAnalysisStatus(int id, [FromHeader(Name = "X-User-Id")] int userId = 1)
    {
        try
        {
            var summary = _resultService.GetSummary(id, userId);
            return Ok(summary);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    /// <summary>Список анализов пользователя (GET)</summary>
    [HttpGet]
    public IActionResult GetUserAnalyses([FromHeader(Name = "X-User-Id")] int userId = 1)
    {
        var histories = _historyRepository.GetByUser(userId)
            .Select(h => new { h.HistoryId, h.Status, h.AnalysisDate, h.SocialNetwork });
        return Ok(histories);
    }
}
