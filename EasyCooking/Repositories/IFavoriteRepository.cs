using EasyCooking.Models;

namespace EasyCooking.Repositories
{
    public interface IFavoriteRepository
    {
        void Add(Favorites favorites);
    }
}