using BotDetect.Application.Dto;
using BotDetect.Domain.Entities;
using BotDetect.Domain.Repositories;

namespace BotDetect.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accounts;

    public AccountService(IAccountRepository accounts)
    {
        _accounts = accounts;
    }

    public AccountDto? GetAccount(int accountId)
    {
        var account = _accounts.GetById(accountId);
        return account == null ? null : ToDto(account);
    }

    public AccountDto CreateAccount(string username, string socialNetwork)
    {
        var account = new Account
        {
            Username = username,
            SocialNetwork = socialNetwork,
            Status = "Active"
        };
        _accounts.Save(account);
        return ToDto(account);
    }

    public AccountDto? UpdateAccount(int accountId, UpdateAccountRequest request)
    {
        var account = _accounts.GetById(accountId);
        if (account == null) return null;

        if (request.Username != null) account.Username = request.Username;
        if (request.Status != null) account.Status = request.Status;

        _accounts.Save(account);
        return ToDto(account);
    }

    public bool DeleteAccount(int accountId)
    {
        var account = _accounts.GetById(accountId);
        if (account == null) return false;

        account.Status = "Deleted";
        _accounts.Save(account);
        return true;
    }

    private static AccountDto ToDto(Account a) =>
        new(a.AccountId, a.Username, a.SocialNetwork, a.Status);
}
