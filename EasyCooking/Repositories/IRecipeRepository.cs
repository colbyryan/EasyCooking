using EasyCooking.Models;
using System.Collections.Generic;

namespace EasyCooking.Repositories
{
    public interface IRecipeRepository
    {
        void Add(Recipe recipe);
        List<Recipe> GetAll();
        List<Recipe> GetByFavorited(int Id);
        Recipe GetById(int Id);
        void Remove(int id);
        void UpdateRecipe(Recipe recipe);
    }
}