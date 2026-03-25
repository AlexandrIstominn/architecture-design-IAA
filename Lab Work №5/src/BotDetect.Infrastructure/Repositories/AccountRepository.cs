using BotDetect.Domain.Entities;
using BotDetect.Domain.Repositories;
using BotDetect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BotDetect.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly BotDetectDbContext _db;

    public AccountRepository(BotDetectDbContext db) => _db = db;

    public Account? GetById(int id) =>
        _db.Accounts.SingleOrDefault(a => a.AccountId == id);

    public Account? GetByUsernameAndNetwork(string username, string socialNetwork) =>
        _db.Accounts.SingleOrDefault(a => a.Username == username && a.SocialNetwork == socialNetwork);

    public void Save(Account account)
    {
        if (account.AccountId == 0)
            _db.Accounts.Add(account);
        else
            _db.Update(account);
        _db.SaveChanges();
    }

    public IEnumerable<Account> GetOrCreateByUsernames(string socialNetwork, IEnumerable<string> usernames)
    {
        var result = new List<Account>();
        foreach (var username in usernames.Distinct())
        {
            var account = GetByUsernameAndNetwork(username, socialNetwork);
            if (account == null)
            {
                account = new Account { Username = username, SocialNetwork = socialNetwork, Status = "Active" };
                Save(account);
            }
            result.Add(account);
        }
        return result;
    }
}
