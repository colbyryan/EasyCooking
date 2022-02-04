using EasyCooking.Models;

namespace EasyCooking.Repositories
{
    public interface IFavoriteRepository
    {
        void Add(Favorites favorites);
        void Delete(int recipeId, int userId);
        Favorites GetById(int id);
        bool IsSubscribed(int userId, int recipeId);
    }
}