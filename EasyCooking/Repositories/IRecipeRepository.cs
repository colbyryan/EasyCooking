using EasyCooking.Models;

namespace EasyCooking.Repositories
{
    public interface IRecipeRepository
    {
        void Add(Recipe recipe);
        Recipe GetAll();
        Recipe GetById(int Id);
    }
}