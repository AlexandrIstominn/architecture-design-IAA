using BotDetect.Domain.Entities;

namespace BotDetect.Domain.Repositories;

public interface IReportRepository
{
    Report? GetById(int id);
    void Save(Report report);
    IEnumerable<Report> GetByUser(int userId);
}
