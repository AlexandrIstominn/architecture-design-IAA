using BotDetect.Domain.Entities;

namespace BotDetect.Domain.Repositories;

public interface IAccountRepository
{
    Account? GetById(int id);
    Account? GetByUsernameAndNetwork(string username, string socialNetwork);
    void Save(Account account);
    IEnumerable<Account> GetOrCreateByUsernames(string socialNetwork, IEnumerable<string> usernames);
}
