using BotDetect.Domain.Entities;
using BotDetect.Domain.Repositories;
using BotDetect.Infrastructure.Data;

namespace BotDetect.Infrastructure.Repositories;

public class BotRatingRepository : IBotRatingRepository
{
    private readonly BotDetectDbContext _db;

    public BotRatingRepository(BotDetectDbContext db) => _db = db;

    public BotRating? GetById(int id) =>
        _db.BotRatings.SingleOrDefault(r => r.RatingId == id);

    public IEnumerable<BotRating> GetAll() =>
        _db.BotRatings.ToList();
}
