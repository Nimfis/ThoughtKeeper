using Dapper;
using System;
using System.Data.SqlClient;
using ThoughtKeeper;

public class UserService : IUserService
{
    private readonly string _connectionString;

    public UserService(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }


    public bool DoesUsernameExist(string username)
    {
        using (var connection = new SqlConnection(_connectionString))
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

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        using (var connection = new SqlConnection(_connectionString))
        {
            const string insertQuery = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
            int rowsAffected = connection.Execute(insertQuery, new { Username = username, PasswordHash = hashedPassword });
        }
    }
    public UserDTO GetUserById(int userId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            const string query = "SELECT UserId, Username, PasswordHash FROM Users WHERE UserId = @UserId";
            return connection.QuerySingleOrDefault<UserDTO>(query, new { UserId = userId });
        }
    }


    public string GetPasswordHash(string username)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            const string query = "SELECT PasswordHash FROM Users WHERE Username = @Username";
            var param = new { Username = username };
            var passwordHash = connection.QuerySingleOrDefault<string>(query, param);
            return passwordHash; 
        }
    }

    public bool VerifyPassword(string username, string password)
    {
        var passwordHash = GetPasswordHash(username);
        return passwordHash != null && BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    public UserDTO GetUserByUsername(string username)
    {
        using (var connection = new SqlConnection(_connectionString))
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