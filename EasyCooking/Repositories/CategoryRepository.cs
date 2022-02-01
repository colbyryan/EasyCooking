
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
                                    SELECT Id, Name
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
        public void CreateCategory(Category category)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Category (
                            Name )
                        OUTPUT INSERTED.ID
                        VALUES (
                            @Name)";
                    cmd.Parameters.AddWithValue("@Name", category.Name);
                    category.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public Category GetCategoryById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name
                                        FROM Category
                                        WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    Category category = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (category == null)
                        {
                            category = new Category()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
                    }
                    reader.Close();
                    return category;
                }
            }
        }

        public void Update(Category category)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = @"UPDATE Category
                                        SET Name = @name
                                        WHERE id = @id";

                    cmd.Parameters.AddWithValue("@name", category.Name);
                    cmd.Parameters.AddWithValue("@id", category.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Category WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
