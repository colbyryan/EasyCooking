
using EasyCooking.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EasyCooking.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly IConfiguration _config;

        public RecipeRepository(IConfiguration config)
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
        public Recipe GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT * 
                                    FROM Recipe
                                   ";

                    Recipe recipe = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        recipe = new Recipe
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            VideoUrl = reader.GetString(reader.GetOrdinal("VideoUrl")),
                            Creator = reader.GetString(reader.GetOrdinal("Creator")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            PrepTime = reader.GetInt32(reader.GetOrdinal("PrepTime")),
                            CookTime = reader.GetInt32(reader.GetOrdinal("CookTime")),
                            ServingAmount = reader.GetString(reader.GetOrdinal("ServingAmount")),
                        };
                    }
                    reader.Close();

                    return recipe;
                }
            }
        }

        public Recipe GetById(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT Title, UserProfileId, CategoryId, ImageUrl, VideoUrl, Creator, Description, PrepTime, CookTime, ServingAmount
                                    FROM Recipe
                                    WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", Id);

                    Recipe recipe = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        recipe = new Recipe
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            VideoUrl = reader.GetString(reader.GetOrdinal("VideoUrl")),
                            Creator = reader.GetString(reader.GetOrdinal("Creator")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            PrepTime = reader.GetInt32(reader.GetOrdinal("PrepTime")),
                            CookTime = reader.GetInt32(reader.GetOrdinal("CookTime")),
                            ServingAmount = reader.GetString(reader.GetOrdinal("ServingAmount")),
                        };
                    }
                    reader.Close();

                    return recipe;
                }
            }
        }

        public void Add(Recipe recipe)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO
                                        Recipe (Title, UserProfileId, CategoryId, ImageUrl, VideoUrl, Creator, Description, PrepTime, CookTime, ServingAmount) 
                                        OUTPUT INSERTED.ID
                                        VALUES(@title, @userProfileId, @categoryId, @imageUrl, @videoUrl, @creator, @description, @prepTime, @cookTime, @servingAmount)";

                    cmd.Parameters.AddWithValue("@title", recipe.Title);
                    cmd.Parameters.AddWithValue("@userProfileId", recipe.UserProfileId);
                    cmd.Parameters.AddWithValue("@categoryId", recipe.CategoryId);
                    cmd.Parameters.AddWithValue("@imageUrl", recipe.ImageUrl);
                    cmd.Parameters.AddWithValue("@videoUrl", recipe.VideoUrl);
                    cmd.Parameters.AddWithValue("@creator", recipe.Creator);
                    cmd.Parameters.AddWithValue("@description", recipe.Description);
                    cmd.Parameters.AddWithValue("@prepTime", recipe.PrepTime);
                    cmd.Parameters.AddWithValue("@cookTime", recipe.CookTime);
                    cmd.Parameters.AddWithValue("@servingAmount", recipe.ServingAmount);

                    recipe.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
