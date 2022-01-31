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
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Favorites WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
