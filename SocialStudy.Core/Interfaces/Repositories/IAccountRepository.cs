using SocialStudy.Core.DTOs.Account;
using SocialStudy.Core.Entities;

namespace SocialStudy.Core.Interfaces.Repositories;

public interface IAccountRepository
{
  Task<Account> AccountInfo(int accountId);
  Task<IEnumerable<Account>> AccountListFromGroup(int groupId);
  Task<int> AddAccount(AccountDTO account);
  Task<bool> UpdateAccount(int id, AccountDTO account);
  Task<bool> EnableAccount(int accountId);
  Task<bool> DisableAccount(int accountId);
  Task<bool> RemoveAccount(int accountId);
}