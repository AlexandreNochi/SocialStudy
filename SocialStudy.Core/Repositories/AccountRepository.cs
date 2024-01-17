using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialStudy.Core.DTOs.Account;
using SocialStudy.Core.Entities;
using SocialStudy.Core.Interfaces.Repositories;

namespace SocialStudy.Core.Repositories;

public class AccountRepository : IAccountRepository
{
  private readonly string _connectionString;
  public AccountRepository(IConfiguration config)
  {
    _connectionString = config.GetConnectionString("SocialStudy");
  }

  public async Task<Account> AccountInfo(int accountId)
  {
    string queryString = "SELECT * FROM Accounts Where Id = @Id";

    using (SqlConnection connectionString = new SqlConnection(_connectionString))
    {
      connectionString.Open();
      SqlCommand command = new SqlCommand(queryString, connectionString);
      command.Parameters.Add("@Id", SqlDbType.Int).Value = accountId;

      SqlDataReader reader = await command.ExecuteReaderAsync();
      Account account = null;

      if (reader.Read())
        account = GetAccountReader(reader);

      reader.Close();

      return account;
    }
  }

  public async Task<IEnumerable<Account>> AccountListFromGroup(int groupId)
  {
    string queryString = "SELECT * FROM Accounts Where GroupId = @groupId";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      connection.Open();
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@groupId", SqlDbType.Int).Value = groupId;

      SqlDataReader reader = await command.ExecuteReaderAsync();
      List<Account> accounts = new();

      while (reader.Read())
        accounts.Add(GetAccountReader(reader));

      reader.Close();

      if (!accounts.IsNullOrEmpty())
        return accounts;
    }
    return null;
  }

  public async Task<int> AddAccount(AccountDTO account)
  {
    int accountId = 0;
    string queryString = "INSERT INTO Accounts (GroupId, Name, Status, Token, Added, Created, Updated) " +
                         "VALUES (@GroupId, @Name, @Status, @Token, @Added, @Created, @Updated);" +
                         "SELECT CAST(scope_identity() AS int)";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@GroupId", SqlDbType.Int).Value = account.GroupId;
      command.Parameters.Add("@Name", SqlDbType.VarChar).Value = account.Name;
      command.Parameters.Add("@Status", SqlDbType.Bit).Value = account.Status;
      command.Parameters.Add("@Token", SqlDbType.VarChar).Value = account.Token;
      command.Parameters.Add("@Added", SqlDbType.DateTime).Value = account.Added;
      command.Parameters.Add("@Created", SqlDbType.DateTime).Value = account.Created;
      command.Parameters.Add("@Updated", SqlDbType.DateTime).Value = account.Updated;

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

  public async Task<bool> UpdateAccount(int id, AccountDTO account)
  {
    string queryString = "UPDATE Accounts SET " +
                         "Name = @Name, Status = @Status, Token = @Token, " +
                         "Added = @Added, Created = @Created, Updated = @Updated;" +
                         "WHERE Id = @Id";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
      command.Parameters.Add("@GroupId", SqlDbType.Int).Value = account.GroupId;
      command.Parameters.Add("@Name", SqlDbType.VarChar).Value = account.Name;
      command.Parameters.Add("@Status", SqlDbType.Bit).Value = account.Status;
      command.Parameters.Add("@Token", SqlDbType.VarChar).Value = account.Token;
      command.Parameters.Add("@Added", SqlDbType.DateTime).Value = account.Added;
      command.Parameters.Add("@Created", SqlDbType.DateTime).Value = account.Created;
      command.Parameters.Add("@Updated", SqlDbType.DateTime).Value = account.Updated;

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

  public async Task<bool> DisableAccount(int accountId)
  {
    string queryString = "UPDATE Accounts SET Accounts.Status = @Status WHERE Id = @Id;" +
                 "SELECT CAST(Status AS BIT) FROM Accounts WHERE Id = @Id";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@Id", SqlDbType.Int).Value = accountId;
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

  public async Task<bool> EnableAccount(int accountId)
  {
    string queryString = "UPDATE Accounts SET Accounts.Status = @Status WHERE Id = @Id;" +
                 "SELECT CAST(Status AS BIT) FROM Accounts WHERE Id = @Id";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@Id", SqlDbType.Int).Value = accountId;
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

  public async Task<bool> RemoveAccount(int accountId)
  {
    string queryString = "DELETE FROM Accounts Where Id = @Id";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
      connection.Open();
      SqlCommand command = new SqlCommand(queryString, connection);
      command.Parameters.Add("@Id", SqlDbType.Int).Value = accountId;

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

  private Account GetAccountReader(SqlDataReader reader)
  {
    Account account = new(reader.GetInt32("Id"),
                          reader.GetInt32("GroupId"),
                          reader.GetString("Name"),
                          reader.GetBoolean("Status"),
                          reader.GetString("Token"),
                          reader.GetDateTime("Added"),
                          reader.GetDateTime("Created"),
                          reader.GetDateTime("Updated")
                        );
    return account;
  }

  private AccountDTO GetAccountDTOReader(SqlDataReader reader)
  {
    AccountDTO account = new(
                          reader.GetInt32("GroupId"),
                          reader.GetString("Name"),
                          reader.GetBoolean("Status"),
                          reader.GetString("Token"),
                          reader.GetDateTime("Added"),
                          reader.GetDateTime("Created"),
                          reader.GetDateTime("Updated")
                        );
    return account;
  }
}