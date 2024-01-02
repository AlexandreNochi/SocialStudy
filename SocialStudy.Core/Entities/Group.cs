namespace SocialStudy.Core.Entities;

public class Group
{
  public int Id { get; set; }
  public string Name { get; set; }
  public bool Status { get; set; }
  public DateTime Created { get; set; }
  public DateTime Updated { get; set; }
  public List<Account>? Accounts { get; set; }
  public List<Page>? Pages { get; set; }

  public Group(int id, string name, bool status, DateTime created, DateTime updated)
  {
    Id = id;
    Name = name;
    Status = status;
    Created = created;
    Updated = updated;
  }
}