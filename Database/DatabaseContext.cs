using Dapper;
using System.Linq;
using System.Data.SqlClient;

namespace ThoughtKeeper.Database
{
    internal class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void EnsureDatabaseCreated()
        {
            var builder = new SqlConnectionStringBuilder(_connectionString)
            {
                InitialCatalog = string.Empty
            };

            string serverConnectionString = builder.ToString();
            string databaseName = new SqlConnectionStringBuilder(_connectionString).InitialCatalog;

            using (var connection = new SqlConnection(serverConnectionString))
            {
                connection.Open();

                var databaseExists = connection.Query<string>("SELECT name FROM sys.databases WHERE name = @name", new { name = databaseName }).Any();
                if (!databaseExists)
                {
                    connection.Execute($"CREATE DATABASE [{databaseName}]");
                }
            }
        }

        public void EnsureSchemaCreated()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string createUsersTable = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
                CREATE TABLE Users (
                    UserId INT PRIMARY KEY IDENTITY,
                    Username NVARCHAR(50) NOT NULL,
                    PasswordHash NVARCHAR(255) NOT NULL
                );";

                string createCategoriesTable = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categories' AND xtype='U')
                CREATE TABLE Categories (
                    Id INT PRIMARY KEY IDENTITY,
                    Name NVARCHAR(100) NOT NULL
                );";

                string createNotesTable = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Notes' AND xtype='U')
                CREATE TABLE Notes (
                    Id INT PRIMARY KEY IDENTITY,
                    Title NVARCHAR(200) NOT NULL,
                    Content NVARCHAR(MAX) NOT NULL,
                    DateCreated DATETIME NOT NULL,
                    UserId INT NOT NULL,
                    CategoryId INT NULL,
                    FOREIGN KEY (UserId) REFERENCES Users(UserId),
                    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
                );";

                connection.Execute(createUsersTable);
                connection.Execute(createCategoriesTable);
                connection.Execute(createNotesTable);
            }
        }

        public void SeedData()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var categoriesExist = connection.Query<int>("SELECT COUNT(1) FROM Categories").Single() > 0;
                if (!categoriesExist)
                {
                    string insertCategories = @"
                    INSERT INTO Categories (Name) VALUES
                    ('Obowiązki domowe'),
                    ('Wydatki'),
                    ('Praca');";

                    connection.Execute(insertCategories);
                }
            }
        }
    }
}
