using System.Data.Common;

namespace SocialStudy.Core.Entities;

public class Page
{
  public int Id { get; set; }
  public string Name { get; set; }
  public bool Status { get; set; }
  public string Link { get; set; }
  public DateTime Added { get; set; }
  public DateTime? Created { get; set; }
  public DateTime Updated { get; set; }
#nullable enable
  public List<Post>? Posts { get; set; }

  public Page(int id, string name, string link, DateTime? created = null, bool status = true)
  {
    Id = id;
    Name = name;
    Link = link;
    Status = status;
    Added = DateTime.Now;
    Created = created;
    Updated = DateTime.Now;
  }

  public Page(int id, string name, bool status, string link, DateTime added, DateTime created, DateTime updated)
  {
    Id = id;
    Name = name;
    Link = link;
    Status = status;
    Added = added;
    Created = created;
    Updated = updated;
  }
}
