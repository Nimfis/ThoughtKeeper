using Dapper;
using System;
using System.Data.SqlClient;
using ThoughtKeeper;
using ThoughtKeeper.Database;
using ThoughtKeeper.Security;

public class UserService : IUserService
{
    private readonly IPasswordManager _passwordManager;

    public UserService(IPasswordManager passwordManager)
    {
        _passwordManager = passwordManager;
    }


    public bool DoesUsernameExist(string username)
    {
        using (var connection = DatabaseContext.GetDbConnection())
        {
            const string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username";
            return connection.ExecuteScalar<bool>(query, new { Username = username });
        }
    }

    public void CreateUser(string username, string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password cannot be empty.");
        }

        var hashedPassword = _passwordManager.HashPassword(password);

        using (var connection = DatabaseContext.GetDbConnection())
        {
            const string insertQuery = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
            connection.Execute(insertQuery, new { Username = username, PasswordHash = hashedPassword });
        }
    }
    public UserDTO GetUserById(int userId)
    {
        using (var connection = DatabaseContext.GetDbConnection())
        {
            const string query = "SELECT UserId, Username, PasswordHash FROM Users WHERE UserId = @UserId";
            return connection.QuerySingleOrDefault<UserDTO>(query, new { UserId = userId });
        }
    }

    public UserDTO GetUserByUsername(string username)
    {
        using (var connection = DatabaseContext.GetDbConnection())
        {
            connection.Open();
            var command = new SqlCommand("SELECT * FROM Users WHERE Username = @username", connection);
            command.Parameters.AddWithValue("@username", username);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new UserDTO
                    {
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                    };
                }
            }
        }
        return null;
    }
}