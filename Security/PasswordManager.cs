using Dapper;
using ThoughtKeeper.Database;
using System.Text.RegularExpressions;
using System.Configuration;

namespace ThoughtKeeper.Security
{
    public class PasswordManager : IPasswordManager
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, ConfigurationManager.AppSettings["passwordSalt"]);
        }

        public bool VerifyPassword(string username, string password)
        {
            var dbPasswordHash = string.Empty;
            var currentPasswordHash = HashPassword(password);

            using (var connection = DatabaseContext.GetDbConnection())
            {
                const string query = "SELECT PasswordHash FROM Users WHERE Username = @Username";
                var param = new { Username = username };
                dbPasswordHash = connection.QuerySingleOrDefault<string>(query, param);
            }

            return dbPasswordHash == currentPasswordHash;
        }

        public bool IsPasswordValid(string password)
        {
            var regex = new Regex("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$");
            return regex.IsMatch(password);
        }
    }
}
