public class PostInfoDTO
{
  public int PageId { get; set; }
  public string Name { get; set; }
  public bool Status { get; set; }
  public string Link { get; set; }
  public DateTime Added { get; set; }
#nullable enable
  public DateTime? Created { get; set; }
  public DateTime Updated { get; set; }

  public PostInfoDTO(int pageId, string name, string link, DateTime? created = null, bool status = true)
  {
    PageId = pageId;
    Name = name;
    Link = link;
    Status = status;
    Added = DateTime.Now;
    Created = created;
    Updated = DateTime.Now;
  }
}