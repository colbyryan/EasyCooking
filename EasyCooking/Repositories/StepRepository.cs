using EasyCooking.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyCooking.Repositories
{
    public class StepRepository : IStepRepository
    {
        private readonly IConfiguration _config;

        public StepRepository(IConfiguration config)
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

        public List<Step> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT * 
                                    FROM Step
                                   ";

                    var reader = cmd.ExecuteReader();

                    List<Step> steps = new List<Step>();
                    while (reader.Read())
                    {
                        var Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        var RecipeId = reader.GetInt32(reader.GetOrdinal("RecipeId"));
                        var Content = reader.GetString(reader.GetOrdinal("Content"));
                        var StepOrder = reader.GetInt32(reader.GetOrdinal("StepOrder"));
                        Step step = new Step
                        {
                            Id = Id,
                            RecipeId = RecipeId,
                            Content = Content,
                            StepOrder = StepOrder
                        };
                        steps.Add(step);
                    }
                    reader.Close();

                    return steps;
                }
            }
        }

        public Step GetById(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT Id, Content, RecipeId, StepOrder
                                    FROM Step
                                    WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", Id);

                    var reader = cmd.ExecuteReader();

                    Step step = null;
                    if (reader.Read())
                    {
                        step = new Step
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            RecipeId = reader.GetInt32(reader.GetOrdinal("RecipeId")),
                            StepOrder = reader.GetInt32(reader.GetOrdinal("StepOrder"))
                        };
                    }
                    reader.Close();

                    return step;
                }
            }
        }
        public List<Step> GetAllByRecipeId(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT *
                                    FROM 
                                    Step
                                    WHERE RecipeId = @id";

                    cmd.Parameters.AddWithValue("@id", Id);

                    var reader = cmd.ExecuteReader();

                    List<Step> steps = new List<Step>();
                    while (reader.Read())
                    {
                        var Content = reader.GetString(reader.GetOrdinal("Content"));
                        var RecipeId = reader.GetInt32(reader.GetOrdinal("RecipeId"));
                        var StepOrder = reader.GetInt32(reader.GetOrdinal("StepOrder"));
                        var StepId = reader.GetInt32(reader.GetOrdinal("Id"));
                        Step step = new Step
                        {
                            Id = StepId,
                            Content = Content,
                            RecipeId = RecipeId,
                            StepOrder = StepOrder
                        };
                        steps.Add(step);
                    }
                    reader.Close();

                    return steps;
                }
            }

        }
        public void Add(Step step)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO
                                        Step (Content, RecipeId, StepOrder) 
                                        OUTPUT INSERTED.ID
                                        VALUES(@content, @recipeId, @stepOrder)";

                    cmd.Parameters.AddWithValue("@content", step.Content);
                    cmd.Parameters.AddWithValue("@recipeId", step.RecipeId);
                    cmd.Parameters.AddWithValue("@stepOrder", step.StepOrder);

                    step.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public void UpdateStep(Step step)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Step 
                            SET 
                            Content = @content
                            WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", step.Id);
                    cmd.Parameters.AddWithValue("@content", step.Content);

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
                    cmd.CommandText = "DELETE FROM Step WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }

            }
        }
    }
}
