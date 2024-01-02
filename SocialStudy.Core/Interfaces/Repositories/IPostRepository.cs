using SocialStudy.Core.Entities;

namespace SocialStudy.Core.Interfaces.Repositories;

public interface IPostRepository
{
  Task<Post> PostInfo(int postId);
  Task<int> AddPost(Post post);
  Task<bool> UpdatePost(Post post);
  Task<bool> RemovePost(int postId);
  Task<bool> EnablePost(int postId);
  Task<bool> DisablePost(int postId);
  Task<IEnumerable<Post>> PostInfoList(IEnumerable<int> postIdList);
  Task<IEnumerable<Post>> PostInfoList(int pageId, int amount);
  Task<IEnumerable<Post>> PostInfoList(int amount);
}