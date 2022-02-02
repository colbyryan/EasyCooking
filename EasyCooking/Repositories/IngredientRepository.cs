using EasyCooking.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyCooking.Repositories
{
    public class IngredientRepository :  IIngredientRepository
    {
        private readonly IConfiguration _config;

        public IngredientRepository(IConfiguration config)
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

        public List<Ingredient> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT * 
                                    FROM Ingredient
                                   ";

                    var reader = cmd.ExecuteReader();

                    List<Ingredient> ingredients = new List<Ingredient>();
                    while (reader.Read())
                    {
                        var Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        var Content = reader.GetString(reader.GetOrdinal("Content"));
                        Ingredient i = new Ingredient
                        {
                            Id = Id,
                            Content = Content,
                        };
                        ingredients.Add(i);
                    }
                    reader.Close();

                    return ingredients;
                }
            }
        }

        public Ingredient GetById(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT Id, Content, RecipeId
                                    FROM Ingredient
                                    WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", Id);

                    var reader = cmd.ExecuteReader();

                    Ingredient ingredient = null;
                    if (reader.Read())
                    {
                        ingredient = new Ingredient
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            RecipeId = reader.GetInt32(reader.GetOrdinal("RecipeId"))
                        };
                    }
                    reader.Close();

                    return ingredient;
                }
            }
        }
        public List<Ingredient> GetAllByRecipeId(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT *
                                    FROM 
                                    Ingredient
                                    WHERE RecipeId = @id";

                    cmd.Parameters.AddWithValue("@id", Id);

                    var reader = cmd.ExecuteReader();

                    List<Ingredient> ingredients = new List<Ingredient>();
                    while (reader.Read())
                    {
                        var Content = reader.GetString(reader.GetOrdinal("Content"));
                        var RecipeId = reader.GetInt32(reader.GetOrdinal("RecipeId"));
                        var IngredientId = reader.GetInt32(reader.GetOrdinal("Id"));
                        Ingredient i = new Ingredient
                        {
                            Id = IngredientId,
                            Content = Content,
                            RecipeId = RecipeId,
                        };
                        ingredients.Add(i);
                    }
                    reader.Close();

                    return ingredients;
                }
            }
            
        }
        public void Add(Ingredient ingredient)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO
                                        Ingredient (Content, RecipeId) 
                                        OUTPUT INSERTED.ID
                                        VALUES(@content, @recipeId)";

                    cmd.Parameters.AddWithValue("@content", ingredient.Content);
                    cmd.Parameters.AddWithValue("@recipeId", ingredient.RecipeId);

                    ingredient.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public void UpdateIngredient(Ingredient ingredient)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Ingredient 
                            SET 
                                Content = @content
                            WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", ingredient.Id);
                    cmd.Parameters.AddWithValue("@content", ingredient.Content);

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
                    cmd.CommandText = "DELETE FROM Ingredient WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }

            }
        }
    }
}
