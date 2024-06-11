using ThoughtKeeper.DTO;
using ThoughtKeeper.Interfaces;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using ThoughtKeeper.Database;

namespace ThoughtKeeper.Services
{
    internal class CategoryService : ICategoryService
    {
        public IEnumerable<CategoryDTO> GetAll()
        {
            using (var connection = DatabaseContext.GetDbConnection())
            {
                return connection.Query<CategoryDTO>("SELECT * FROM Categories").ToList();
            }
        }

        public string GetCategoryName(int id)
        {
            var categoryName = string.Empty;

            using (var connection = DatabaseContext.GetDbConnection())
            {
                const string query = "SELECT Name FROM Categories WHERE Id = @CategoryId";
                categoryName = connection.ExecuteScalar<string>(query, new { CategoryId = id });
            }

            return categoryName;
        }
    }
}
