using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using SocialStudy.Core.Entities;
using SocialStudy.Core.Interfaces.Repositories;

namespace SocialStudy.Core.Repositories;
public class PageRepository : IPageRepository
{
  private readonly string _connectionString;

  public PageRepository(string connectionString)
  {
    _connectionString = connectionString;
  }

  public async Task<int> AddPage(Page page)
  {
    int accountId;
    string queryString = "INSERT INTO Pages (Name, Status, Link, Added, Created, Updated) " +
                         "VALUES (@Name, @Status, @Link, @Added, @Created, @Updated);" +
                         "SELECT CAST(scope_identity() AS int)";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@Name", SqlDbType.VarChar).Value = page.Name;
      command.Parameters.Add("@Status", SqlDbType.Bit).Value = page.Status;
      command.Parameters.Add("@Link", SqlDbType.VarChar).Value = page.Link;
      command.Parameters.Add("@Added", SqlDbType.DateTime).Value = page.Added;
      command.Parameters.Add("@Created", SqlDbType.DateTime).Value = page.Created;
      command.Parameters.Add("@Updated", SqlDbType.DateTime).Value = page.Updated;

      try
      {
        connection.Open();
        accountId = (int)await command.ExecuteScalarAsync();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return 0;
      }

      connection.Close();

      return accountId;
    }
  }

  public async Task<bool> DisablePage(int pageId)
  {
    string queryString = "UPDATE Pages SET Pages.Status = @Status WHERE Id = @Id;" +
                  "SELECT CAST(Status AS BIT) FROM Pages WHERE Id = @Id";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@Id", SqlDbType.Int).Value = pageId;
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

  public async Task<bool> EnablePage(int pageId)
  {
    string queryString = "UPDATE Pages SET Pages.Status = @Status WHERE Id = @Id;" +
                  "SELECT CAST(Status AS BIT) FROM Pages WHERE Id = @Id";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@Id", SqlDbType.Int).Value = pageId;
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

  public async Task<IEnumerable<int>> PageIdList()
  {
    string queryString = "SELECT Id FROM Pages;";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      connection.Open();
      SqlCommand command = new SqlCommand(queryString, connection);

      SqlDataReader reader = await command.ExecuteReaderAsync();
      var pages = new List<int>();

      while (reader.Read())
        pages.Add(reader.GetInt32("Id"));

      reader.Close();

      if (!pages.IsNullOrEmpty())
        return pages;
    }
    return null;
  }

  public async Task<Page> PageInfo(int pageId)
  {
    string queryString = "SELECT * FROM Pages Where Id = @Id";

    using (SqlConnection connectionString = new SqlConnection(_connectionString))
    {
      connectionString.Open();
      SqlCommand command = new SqlCommand(queryString, connectionString);
      command.Parameters.Add("@Id", SqlDbType.Int).Value = pageId;

      SqlDataReader reader = await command.ExecuteReaderAsync();
      Page page = null;

      if (reader.Read())
        page = GetPageReader(reader);

      reader.Close();

      return page;
    }
  }

  public async Task<IEnumerable<Page>> PageInfoList(int amount)
  {
    string queryString = "SELECT TOP @amount * FROM Pages;";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      connection.Open();
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@amount", SqlDbType.Int).Value = amount;

      SqlDataReader reader = await command.ExecuteReaderAsync();
      List<Page> pages = new();

      while (reader.Read())
        pages.Add(GetPageReader(reader));

      reader.Close();

      if (!pages.IsNullOrEmpty())
        return pages;
    }
    return null;
  }

  public async Task<IEnumerable<Page>> PageInfoList(List<int> pageIdList)
  {
    var ids = string.Join(",", pageIdList.Select(x => x.ToString()).ToArray());
    string queryString = "SELECT * FROM Pages Where Id in (" + ids + ")";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      connection.Open();
      SqlCommand command = new SqlCommand(queryString, connection);

      SqlDataReader reader = await command.ExecuteReaderAsync();
      List<Page> pages = new();

      while (reader.Read())
        pages.Add(GetPageReader(reader));

      reader.Close();

      if (!pages.IsNullOrEmpty())
        return pages;
    }
    return null;
  }

  public async Task<IEnumerable<int>> PostIdList(int pageId)
  {
    string queryString = "SELECT Id FROM Posts WHERE PageId = @pageId;";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      connection.Open();
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@pageId", SqlDbType.Int).Value = pageId;

      SqlDataReader reader = await command.ExecuteReaderAsync();
      var posts = new List<int>();

      while (reader.Read())
        posts.Add(reader.GetInt32("Id"));

      reader.Close();

      if (!posts.IsNullOrEmpty())
        return posts;
    }
    return null;
  }

  public async Task<bool> RemovePage(int pageId)
  {
    string queryString = "DELETE FROM Pages Where Id = @Id";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      connection.Open();
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@Id", SqlDbType.Int).Value = pageId;

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

  public async Task<bool> UpdatePage(Page page)
  {
    string queryString = "UPDATE Accounts SET " +
                         "Name = @Name, Status = @Status, Link = @Link, " +
                         "Added = @Added, Created = @Created, Updated = @Updated;" +
                         "WHERE Id = @Id";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@Id", SqlDbType.Int).Value = page.Id;
      command.Parameters.Add("@Name", SqlDbType.VarChar).Value = page.Name;
      command.Parameters.Add("@Status", SqlDbType.Bit).Value = page.Status;
      command.Parameters.Add("@Link", SqlDbType.VarChar).Value = page.Link;
      command.Parameters.Add("@Added", SqlDbType.DateTime).Value = page.Added;
      command.Parameters.Add("@Created", SqlDbType.DateTime).Value = page.Created;
      command.Parameters.Add("@Updated", SqlDbType.DateTime).Value = page.Updated;

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

  private Page GetPageReader(SqlDataReader reader)
  {
    Page page = new(reader.GetInt32("Id"),
                    reader.GetString("Name"),
                    reader.GetBoolean("Status"),
                    reader.GetString("Link"),
                    reader.GetDateTime("Added"),
                    reader.GetDateTime("Created"),
                    reader.GetDateTime("Updated")
                  );
    return page;
  }
}