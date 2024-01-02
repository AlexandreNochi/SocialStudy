using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using SocialStudy.Core.Entities;
using SocialStudy.Core.Interfaces.Repositories;

namespace SocialStudy.Core.Repositories;

public class PostRepository : IPostRepository
{
  private readonly string _connectionString;
  public PostRepository(string connection)
  {
    _connectionString = connection;
  }

  public async Task<int> AddPost(Post post)
  {
    string queryString = "INSERT INTO Posts (PageId, Name, Status, Link, Added, Created, Updated) " +
                         "VALUES (@PageId, @Name, @Status, @Link, @Added, @Created, @Updated);" +
                         "SELECT CAST(scope_identity() AS int)";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@PageId", SqlDbType.Int).Value = post.PageId;
      command.Parameters.Add("@Name", SqlDbType.VarChar).Value = post.Name;
      command.Parameters.Add("@Status", SqlDbType.Bit).Value = post.Status;
      command.Parameters.Add("@Link", SqlDbType.VarChar).Value = post.Link;
      command.Parameters.Add("@Added", SqlDbType.DateTime).Value = post.Added;
      command.Parameters.Add("@Created", SqlDbType.DateTime).Value = post.Created;
      command.Parameters.Add("@Updated", SqlDbType.DateTime).Value = post.Updated;

      try
      {
        connection.Open();
        return (int)await command.ExecuteScalarAsync();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return 0;
      }
    }
  }

  public async Task<bool> UpdatePost(Post post)
  {
    string queryString = "UPDATE Posts SET " +
                         "Name = @Name, Status = @Status, Link = @Link, " +
                         "Added = @Added, Created = @Created, Updated = @Updated;" +
                         "WHERE Id = @Id";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@Id", SqlDbType.Int).Value = post.Id;
      command.Parameters.Add("@Name", SqlDbType.VarChar).Value = post.Name;
      command.Parameters.Add("@Status", SqlDbType.Bit).Value = post.Status;
      command.Parameters.Add("@Link", SqlDbType.VarChar).Value = post.Link;
      command.Parameters.Add("@Added", SqlDbType.DateTime).Value = post.Added;
      command.Parameters.Add("@Created", SqlDbType.DateTime).Value = post.Created;
      command.Parameters.Add("@Updated", SqlDbType.DateTime).Value = post.Updated;

      try
      {
        connection.Open();
        await command.ExecuteNonQueryAsync();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return false;
      }
      return true;
    }
  }

  public async Task<Post> PostInfo(int postId)
  {
    string queryString = "SELECT * FROM Posts Where Id = @Id";

    using (SqlConnection connectionString = new SqlConnection(_connectionString))
    {
      connectionString.Open();
      SqlCommand command = new SqlCommand(queryString, connectionString);
      command.Parameters.Add("@Id", SqlDbType.Int).Value = postId;

      SqlDataReader reader = await command.ExecuteReaderAsync();
      Post post = null;

      if (reader.Read())
        post = GetPostReader(reader);

      reader.Close();

      return post;
    }
  }

  public async Task<bool> RemovePost(int postId)
  {
    string queryString = "DELETE FROM Posts WHERE Id = @Id";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      connection.Open();
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@Id", SqlDbType.Int).Value = postId;

      try
      {
        await command.ExecuteNonQueryAsync();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return false;
      }

      return true;
    }
  }

  public async Task<bool> EnablePost(int postId)
  {
    string queryString = "UPDATE Posts SET Posts.Status = @Status WHERE Id = @Id;" +
                 "SELECT CAST(Status AS BIT) FROM Posts WHERE Id = @Id";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@Id", SqlDbType.Int).Value = postId;
      command.Parameters.Add("@Status", SqlDbType.Bit).Value = true;

      try
      {
        connection.Open();
        return (bool)await command.ExecuteScalarAsync();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return false;
      }
    }
  }

  public async Task<bool> DisablePost(int postId)
  {
    string queryString = "UPDATE Posts SET Posts.Status = @Status WHERE Id = @Id;" +
                  "SELECT CAST(Status AS BIT) FROM Posts WHERE Id = @Id";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@Id", SqlDbType.Int).Value = postId;
      command.Parameters.Add("@Status", SqlDbType.Bit).Value = false;

      try
      {
        connection.Open();
        return (bool)await command.ExecuteScalarAsync();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return false;
      }
    }
  }

  public async Task<IEnumerable<Post>> PostInfoList(IEnumerable<int> postIdList)
  {
    var ids = string.Join(",", postIdList.Select(x => x.ToString()).ToArray());
    string queryString = "SELECT * FROM Accounts Where Id in (" + ids + ")";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      connection.Open();
      SqlCommand command = new SqlCommand(queryString, connection);

      SqlDataReader reader = await command.ExecuteReaderAsync();
      List<Post> posts = new();

      while (reader.Read())
        posts.Add(GetPostReader(reader));

      reader.Close();

      if (!posts.IsNullOrEmpty())
        return posts;
    }
    return null;
  }

  public async Task<IEnumerable<Post>> PostInfoList(int pageId, int amount)
  {
    string queryString = "SELECT Top @amount * FROM Accounts Where PageId = @pageId ";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      connection.Open();
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@pageId", SqlDbType.Int).Value = pageId;
      command.Parameters.Add("@amount", SqlDbType.Int).Value = amount;

      SqlDataReader reader = await command.ExecuteReaderAsync();
      List<Post> posts = new();

      while (reader.Read())
        posts.Add(GetPostReader(reader));

      reader.Close();

      if (!posts.IsNullOrEmpty())
        return posts;
    }
    return null;
  }

  public async Task<IEnumerable<Post>> PostInfoList(int amount)
  {
    string queryString = "SELECT Top @amount * FROM Accounts;";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      connection.Open();
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@amount", SqlDbType.Int).Value = amount;

      SqlDataReader reader = await command.ExecuteReaderAsync();
      List<Post> posts = new();

      while (reader.Read())
        posts.Add(GetPostReader(reader));

      reader.Close();

      if (!posts.IsNullOrEmpty())
        return posts;
    }
    return null;
  }

  private Post GetPostReader(SqlDataReader reader)
  {
    Post post = new(reader.GetInt32("Id"),
                    reader.GetInt32("PageId"),
                    reader.GetString("Name"),
                    reader.GetBoolean("Status"),
                    reader.GetString("Link"),
                    reader.GetDateTime("Added"),
                    reader.GetDateTime("Created"),
                    reader.GetDateTime("Updated")
                  );
    return post;
  }
}