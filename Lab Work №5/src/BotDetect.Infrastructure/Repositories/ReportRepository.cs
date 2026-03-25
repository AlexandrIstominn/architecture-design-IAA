using BotDetect.Domain.Entities;
using BotDetect.Domain.Repositories;
using BotDetect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BotDetect.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly BotDetectDbContext _db;

    public ReportRepository(BotDetectDbContext db) => _db = db;

    public Report? GetById(int id) =>
        _db.Reports
            .Include(r => r.ExportedResults)
            .SingleOrDefault(r => r.ReportId == id);

    public IEnumerable<Report> GetByUser(int userId) =>
        _db.Reports.Where(r => r.UserId == userId).OrderByDescending(r => r.ReportDate).ToList();

    public void Save(Report report)
    {
        if (report.ReportId == 0)
            _db.Reports.Add(report);
        else
            _db.Update(report);
        _db.SaveChanges();
    }
}
