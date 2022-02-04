using EasyCooking.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EasyCooking.Repositories
{
    public class FavoriteRepository : BaseRepository, IFavoriteRepository
    {
        public FavoriteRepository(IConfiguration config) : base(config) { }

        public void Add(Favorites favorites)
        {

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Favorites (RecipeId, UserProfileId)
                                        OUTPUT INSERTED.ID
                                        VALUES (@recipeId, @userPRofileId)";
                    cmd.Parameters.AddWithValue("@recipeId", favorites.RecipeId);
                    cmd.Parameters.AddWithValue("@userProfileId", favorites.UserProfileId);

                    int id = (int)cmd.ExecuteScalar();

                    favorites.Id = id;
                }
            }
        }
        public void Delete(int recipeId, int userId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Favorites 
                                        WHERE Recipeid = @recipeId
                                        AND UserProfileId = @userProfileId";
                    cmd.Parameters.AddWithValue("@recipeId", recipeId);
                    cmd.Parameters.AddWithValue("@userProfileId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public bool IsSubscribed(int userId, int recipeId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT * FROM Favorites
                    WHERE UserProfileId = @userId
                    AND RecipeId = @recipeId
                    ";
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@recipeId", recipeId);

                    using var reader = cmd.ExecuteReader();

                    return reader.Read();
                }
            }
        }
        public Favorites GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT *
                                        FROM Favorites
                                        WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    var favorite = new Favorites();

                    if (reader.Read())
                    {
                        favorite = new Favorites()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("userProfileId")),
                            RecipeId = reader.GetInt32(reader.GetOrdinal("recipeId"))
                        };
                    }
                    reader.Close();
                    return favorite;
                }
            }
        }
    }
}
