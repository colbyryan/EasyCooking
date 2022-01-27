using EasyCooking.Models;
using System.Collections.Generic;

namespace EasyCooking.Repositories
{
    public interface IRecipeRepository
    {
        void Add(Recipe recipe);
        List<Recipe> GetAll();
        Recipe GetById(int Id);
    }
}