using System;
using ThoughtKeeper.DTO;
using ThoughtKeeper.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace ThoughtKeeper.Services
{
    internal class CategoryService : ICategoryService
    {
        private readonly string _connectionString;

        public CategoryService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<CategoryDTO>("SELECT * FROM Categories").ToList();
            }
        }
    }
}
