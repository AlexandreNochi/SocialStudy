using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using SocialStudy.Core.DTOs.Account;
using SocialStudy.Core.Entities;
using SocialStudy.Core.Interfaces.Repositories;

namespace SocialStudy.Api.Controllers;

[ApiController, Route("Accounts")]
public class AccountsController : ControllerBase
{
  private readonly IAccountRepository _repository;
  private readonly ILogger<AccountsController> _logger;

  public AccountsController(IAccountRepository repository, ILogger<AccountsController> logger)
  {
    _repository = repository;
    _logger = logger;
  }

  [HttpGet("AccountInfo")]
  public async Task<Account> AccountInfo(int id)
  {
    try
    {
      return await _repository.AccountInfo(id);
    }
    catch (Exception ex)
    {
      _logger.LogInformation(ex.Message);
      return null;
    }
  }

  [HttpPost("AddAccount")]
  public async Task<int> CreateAccount(AccountDTO account)
  {
    try
    {
      return await _repository.AddAccount(account);
    }
    catch (Exception ex)
    {
      _logger.LogInformation(ex.Message);
      return 0;
    }
  }

  [HttpGet("GetAccountListFromGroup")]
  public async Task<List<Account>> GetAccountListFromGroup(int groupId)
  {
    try
    {
      var ac = new List<Account>();
      ac.AddRange(await _repository.AccountListFromGroup(groupId));

      return ac;
    }
    catch (Exception ex)
    {
      _logger.LogInformation(ex.Message);
      return null;
    }
  }

  [HttpPut("UpdateAccount")]
  public async Task<bool> UpdateAccount(int id, AccountDTO account)
  {
    try
    {
      return await _repository.UpdateAccount(id, account);
    }
    catch (Exception ex)
    {
      _logger.LogInformation(ex.Message);
      return false;
    }
  }

  [HttpDelete("DeleteAccount")]
  public async Task<bool> DeleteAccount(int id)
  {
    try
    {
      return await _repository.RemoveAccount(id);
    }
    catch (Exception ex)
    {
      _logger.LogInformation(ex.Message);
      return false;
    }
  }
}