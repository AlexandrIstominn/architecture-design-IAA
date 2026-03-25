using BotDetect.Application.Dto;

namespace BotDetect.Application.Services;

public interface IAccountService
{
    AccountDto? GetAccount(int accountId);
    AccountDto CreateAccount(string username, string socialNetwork);
    AccountDto? UpdateAccount(int accountId, UpdateAccountRequest request);
    bool DeleteAccount(int accountId);
}
