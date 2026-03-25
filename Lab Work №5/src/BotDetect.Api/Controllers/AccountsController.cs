using BotDetect.Application.Dto;
using BotDetect.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BotDetect.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>Получение аккаунта по ID (GET)</summary>
    [HttpGet("{id:int}")]
    public IActionResult GetAccount(int id)
    {
        var account = _accountService.GetAccount(id);
        return account == null ? NotFound() : Ok(account);
    }

    /// <summary>Создание аккаунта (POST)</summary>
    [HttpPost]
    public IActionResult CreateAccount([FromBody] CreateAccountRequest request)
    {
        var account = _accountService.CreateAccount(request.Username, request.SocialNetwork);
        return CreatedAtAction(nameof(GetAccount), new { id = account.AccountId }, account);
    }

    /// <summary>Обновление аккаунта (PUT)</summary>
    [HttpPut("{id:int}")]
    public IActionResult UpdateAccount(int id, [FromBody] UpdateAccountRequest request)
    {
        var account = _accountService.UpdateAccount(id, request);
        return account == null ? NotFound() : Ok(account);
    }

    /// <summary>Удаление аккаунта (DELETE)</summary>
    [HttpDelete("{id:int}")]
    public IActionResult DeleteAccount(int id)
    {
        return _accountService.DeleteAccount(id) ? NoContent() : NotFound();
    }
}
