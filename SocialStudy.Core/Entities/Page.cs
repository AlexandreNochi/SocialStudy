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
}
