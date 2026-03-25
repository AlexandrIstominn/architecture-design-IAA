using BotDetect.Domain.Entities;

namespace BotDetect.Domain.Repositories;

public interface IBotRatingRepository
{
    BotRating? GetById(int id);
    IEnumerable<BotRating> GetAll();
}
