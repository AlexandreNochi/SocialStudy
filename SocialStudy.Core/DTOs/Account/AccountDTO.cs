namespace SocialStudy.Core.DTOs.Account;

public class AccountDTO
{
  public int GroupId { get; set; }
  public string Name { get; set; }
  public bool Status { get; set; }
  public string Token { get; set; }
  public DateTime Added { get; set; }
  public DateTime Created { get; set; }
  public DateTime Updated { get; set; }

  public AccountDTO(int groupId, string name, bool status, string token)
  {
    GroupId = groupId;
    Name = name;
    Status = status;
    Token = token;
    Added = DateTime.Now;
    Created = DateTime.Now;
    Updated = DateTime.Now;
  }

  public AccountDTO(int groupId, string name, bool status, string token, DateTime added, DateTime created, DateTime updated)
  {
    GroupId = groupId;
    Name = name;
    Status = status;
    Token = token;
    Added = added;
    Created = created;
    Updated = updated;
  }
}