using SocialStudy.Core.Entities;

namespace SocialStudy.Core.Interfaces.Repositories;

public interface IPageRepository
{
  Task<int> AddPage(Page page);
  Task<Page> PageInfo(int pageId);
  Task<IEnumerable<Page>> PageInfoList(int amount);
  Task<IEnumerable<Page>> PageInfoList(List<int> pageIdList);
  Task<bool> EnablePage(int pageId);
  Task<bool> DisablePage(int pageId);
  Task<bool> UpdatePage(Page page);
  Task<bool> RemovePage(int pageId);
  Task<IEnumerable<int>> PageIdList();
  Task<IEnumerable<int>> PostIdList(int pageId);
}