namespace SocialStudy.Core.DTOs.Page;

public class PageDTO
{
  public string Name { get; set; }
  public bool Status { get; set; }
  public string Link { get; set; }
  public DateTime Added { get; set; }
  public DateTime? Created { get; set; }
  public DateTime Updated { get; set; }
#nullable enable
  public List<PostInfoDTO>? Posts { get; set; }

  public PageDTO(string name, string link, DateTime? created = null, bool status = true)
  {
    Name = name;
    Link = link;
    Status = status;
    Added = DateTime.Now;
    Created = created;
    Updated = DateTime.Now;
  }
}