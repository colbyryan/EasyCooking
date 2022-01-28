
using EasyCooking.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace EasyCooking.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
            private readonly IConfiguration _config;

            public CategoryRepository(IConfiguration config)
            {
                _config = config;
            }

            public SqlConnection Connection
            {
                get
                {
                    return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                }
            }
        public List<Category> GetAll()
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                                    SELECT * 
                                    FROM Category
                                   ";

                        var reader = cmd.ExecuteReader();

                        List<Category> category = new List<Category>();
                        while (reader.Read())
                        {
                            var Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            var Name = reader.GetString(reader.GetOrdinal("Name"));
                            Category c = new Category
                            {
                                Id = Id,
                                Name = Name
                            };
                            category.Add(c);
                        }
                        reader.Close();

                        return category;
                    }
                }
            }
    }
}
