using SocialStudy.Core.Entities;

namespace SocialStudy.Core.Interfaces.Repositories;

public interface IAccountRepository
{
  Task<int> AddAccount(Account account);
  Task<Account> AccountInfo(int accountId);
  Task<bool> UpdateAccount(Account account);
  Task<bool> EnableAccount(int accountId);
  Task<bool> DisableAccount(int accountId);
  Task<bool> RemoveAccount(int accountId);
}