namespace SocialStudy.Core.Entities;

public class Account
{
  public int Id { get; set; }
  public int GroupId { get; set; }
  public string Name { get; set; }
  public bool Status { get; set; }
  public string Token { get; set; }
  public DateTime Added { get; set; }
  public DateTime Created { get; set; }
  public DateTime Updated { get; set; }

  public Account(int id, string name, string token)
  {
    Id = id;
    Name = name;
    Token = token;
    Added = DateTime.Now;
    Created = DateTime.Now;
    Updated = DateTime.Now;
  }

  public Account(int id, int groupId, string name, bool status, string token, DateTime added, DateTime created, DateTime updated)
  {
    Id = id;
    GroupId = groupId;
    Name = name;
    Status = status;
    Token = token;
    Added = added;
    Created = created;
    Updated = updated;
  }

  public Account(int groupId, string name, string token, DateTime added)
  {
    Id = 0;
    GroupId = groupId;
    Name = name;
    Token = token;
    Added = added;
    Created = DateTime.Now;
    Updated = DateTime.Now;
  }
}