namespace SocialStudy.Core.Entities;

public class Post
{
  public int Id { get; set; }
  public int PageId { get; set; }
  public string Name { get; set; }
  public bool Status { get; set; }
  public string Link { get; set; }
  public DateTime Added { get; set; }
  public DateTime? Created { get; set; }
  public DateTime Updated { get; set; }

  public Post(int id, int pageId, string name, string link, DateTime? created = null, bool status = true)
  {
    Id = id;
    PageId = pageId;
    Name = name;
    Link = link;
    Status = status;
    Added = DateTime.Now;
    Created = created;
    Updated = DateTime.Now;
  }

  public Post(int id, int pageId, string name, bool status, string link, DateTime added, DateTime created, DateTime updated)
  {
    Id = id;
    PageId = pageId;
    Name = name;
    Link = link;
    Status = status;
    Added = added;
    Created = created;
    Updated = updated;
  }
}