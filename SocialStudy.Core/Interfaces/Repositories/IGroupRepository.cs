using SocialStudy.Core.Entities;

namespace SocialStudy.Core.Interfaces.Repositories;

public interface IGroupRepository
{
  Task<Group> GroupInfo(int groupId);
  Task<IEnumerable<int>> AccountIdList(int groupId);
  Task<IEnumerable<int>> PageIdList(int groupId);
  Task<bool> UpdateGroup(Group group);
  Task<int> CreateGroup(Group group);
  Task<bool> DeleteGroup(int groupId);
  Task<bool> DisableGroup(int groupId);
  Task<bool> EnableGroup(int groupId);
  Task<bool> DisableAccountInGroup(int groupId, int accountId);
  Task<bool> EnableAccountInGroup(int groupId, int accountId);
  Task<bool> RemoveAccountToGroup(int groupId, int accountId);
  Task<bool> DisableGroupCrawling(int groupId);
  Task<bool> EnableGroupCrawling(int groupId);
  Task<bool> AddPageToGroupCrawling(int groupId, int pageId);
  Task<bool> RemovePageToCrawling(int groupId, int pageId);
  Task<bool> DisablePageToCrawling(int groupId, int pageId);
  Task<bool> EnablePageToCrawling(int groupId, int pageId);
}