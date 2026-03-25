using BotDetect.Application.Dto;
using BotDetect.Application.Services;
using BotDetect.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BotDetect.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly IReportRepository _reportRepository;

    public ReportsController(IReportService reportService, IReportRepository reportRepository)
    {
        _reportService = reportService;
        _reportRepository = reportRepository;
    }

    /// <summary>Создание отчёта (POST)</summary>
    [HttpPost]
    public IActionResult CreateReport([FromBody] CreateReportRequest request, [FromHeader(Name = "X-User-Id")] int userId = 1)
    {
        try
        {
            var reportId = _reportService.CreateReport(request, userId);
            return CreatedAtAction(nameof(GetReport), new { id = reportId }, new { reportId });
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

    /// <summary>Получение отчёта (GET)</summary>
    [HttpGet("{id:int}")]
    public IActionResult GetReport(int id, [FromHeader(Name = "X-User-Id")] int userId = 1)
    {
        var report = _reportService.GetReport(id, userId);
        return report == null ? NotFound() : Ok(report);
    }

    /// <summary>Список отчётов пользователя (GET)</summary>
    [HttpGet]
    public IActionResult GetUserReports([FromHeader(Name = "X-User-Id")] int userId = 1)
    {
        var reports = _reportRepository.GetByUser(userId)
            .Select(r => new { r.ReportId, r.ReportDate, r.FileFormat });
        return Ok(reports);
    }
}
