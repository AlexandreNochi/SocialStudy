using SocialStudy.Core.DTOs.Account;
using SocialStudy.Core.DTOs.Page;

namespace SocialStudy.Core.DTOs.Group;

public class GroupDTO
{
  public int Id { get; set; }
  public string Name { get; set; }
  public bool Status { get; set; }
  public DateTime Created { get; set; }
  public DateTime Updated { get; set; }
#nullable enable
  public List<AccountDTO>? Accounts { get; set; }
#nullable enable
  public List<PageDTO>? Pages { get; set; }

  public GroupDTO(string name, bool status, DateTime created, DateTime updated)
  {
    Name = name;
    Status = status;
    Created = created;
    Updated = updated;
  }
}