
using EasyCooking.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace EasyCooking.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly IConfiguration _config;

        public RecipeRepository(IConfiguration config)
        {
            _config = config;
        }

        private string GetNullableString(SqlDataReader reader, string column)
        {
            var ordinal = reader.GetOrdinal(column);
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }
            return reader.GetString(ordinal);
        }


        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        public List<Recipe> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                   select r.Id, r.Title, r.UserProfileId, r.CategoryId, r.ImageUrl, r.Creator, r.Description, r.PrepTime, r.CookTime, r.ServingAmount, r.VideoUrl, c.Id, c.Name from recipe r
                                   left join Category c on r.CategoryId = c.Id
                                   ";

                    var reader = cmd.ExecuteReader();

                    List<Recipe> recipe = new List<Recipe>();
                    while (reader.Read())
                    {
                        var Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        var Title = reader.GetString(reader.GetOrdinal("Title"));
                        var UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId"));
                        int? CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? null : reader.GetInt32(reader.GetOrdinal("CategoryId"));
                        var ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        var VideoUrl = GetNullableString(reader, "VideoUrl");
                        var Creator = reader.GetString(reader.GetOrdinal("Creator"));
                        var Description = reader.GetString(reader.GetOrdinal("Description"));
                        var PrepTime = reader.GetInt32(reader.GetOrdinal("PrepTime"));
                        var CookTime = reader.GetInt32(reader.GetOrdinal("CookTime"));
                        var ServingAmount = reader.GetString(reader.GetOrdinal("ServingAmount"));
                        var Name = reader.GetString(reader.GetOrdinal("Name"));
                        Recipe r = new Recipe
                        {
                            Id = Id,
                            Title = Title, 
                            UserProfileId = UserProfileId,
                            CategoryId = CategoryId,
                            ImageUrl = ImageUrl,
                            VideoUrl = VideoUrl,
                            Creator = Creator,
                            Description = Description, 
                            PrepTime = PrepTime,
                            CookTime = CookTime,
                            ServingAmount = ServingAmount,
                            CategoryName = Name,
                        };
                        recipe.Add(r);
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
                                    SELECT Recipe.Id, Title, UserProfileId, CategoryId, ImageUrl, VideoUrl, Creator, Description, PrepTime, CookTime, ServingAmount, c.Name
                                    FROM Recipe
                                    LEFT JOIN Category c on Recipe.CategoryId = c.Id
                                    WHERE Recipe.Id = @id";

                    cmd.Parameters.AddWithValue("@id", Id);

                    var reader = cmd.ExecuteReader();

                    Recipe recipe = null;
                    if (reader.Read())
                    {
                        recipe = new Recipe
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            ImageUrl = GetNullableString(reader, "ImageUrl"),
                            VideoUrl = GetNullableString(reader, "VideoUrl"),
                            Creator = reader.GetString(reader.GetOrdinal("Creator")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            PrepTime = reader.GetInt32(reader.GetOrdinal("PrepTime")),
                            CookTime = reader.GetInt32(reader.GetOrdinal("CookTime")),
                            ServingAmount = reader.GetString(reader.GetOrdinal("ServingAmount")),
                        };
                        if (!reader.IsDBNull(reader.GetOrdinal("CategoryId")))
                            {
                            recipe.CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"));
                            recipe.Category = new Category
                            {
                                Id = (int)recipe.CategoryId,
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
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
                    cmd.Parameters.AddWithValue("@categoryId", recipe.CategoryId == null ? DBNull.Value : recipe.CategoryId);
                    cmd.Parameters.AddWithValue("@imageUrl", recipe.ImageUrl == null ? DBNull.Value : recipe.ImageUrl);
                    cmd.Parameters.AddWithValue("@videoUrl", recipe.VideoUrl == null ? DBNull.Value : recipe.VideoUrl);
                    cmd.Parameters.AddWithValue("@creator", recipe.Creator);
                    cmd.Parameters.AddWithValue("@description", recipe.Description);
                    cmd.Parameters.AddWithValue("@prepTime", recipe.PrepTime);
                    cmd.Parameters.AddWithValue("@cookTime", recipe.CookTime);
                    cmd.Parameters.AddWithValue("@servingAmount", recipe.ServingAmount);

                    recipe.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public void UpdateRecipe(Recipe recipe)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Recipe 
                            SET 
                                Title = @title,
                                UserProfileId = @userProfileId,
                                CategoryId = @categoryId,
                                ImageUrl = @imageUrl,
                                VideoUrl = @videoUrl,
                                Creator = @creator,
                                Description = @description,
                                PrepTime = @prepTime,
                                CookTime = @cookTime,
                                ServingAmount = @servingAmount
                            WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", recipe.Id);
                    cmd.Parameters.AddWithValue("@title", recipe.Title);
                    cmd.Parameters.AddWithValue("@userProfileId", recipe.UserProfileId);
                    cmd.Parameters.AddWithValue("@categoryId", recipe.CategoryId == null ? DBNull.Value : recipe.CategoryId);
                    cmd.Parameters.AddWithValue("@imageUrl", recipe.ImageUrl == null ? DBNull.Value : recipe.ImageUrl);
                    cmd.Parameters.AddWithValue("@videoUrl", recipe.VideoUrl == null ? DBNull.Value : recipe.VideoUrl);
                    cmd.Parameters.AddWithValue("@creator", recipe.Creator);
                    cmd.Parameters.AddWithValue("@description", recipe.Description);
                    cmd.Parameters.AddWithValue("@prepTime", recipe.PrepTime);
                    cmd.Parameters.AddWithValue("@cookTime", recipe.CookTime);
                    cmd.Parameters.AddWithValue("@servingAmount", recipe.ServingAmount);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Remove(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Recipe WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }

            }
        }

    }
}
