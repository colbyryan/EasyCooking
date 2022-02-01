using EasyCooking.Models;
using System.Collections.Generic;

namespace EasyCooking.Repositories
{
    public interface IIngredientRepository
    {
        void Add(Ingredient ingredient);
        List<Ingredient> GetAll();
        List<Ingredient> GetAllByRecipeId(int Id);
        Ingredient GetById(int Id);
        void Remove(int id);
        void UpdateIngredient(Ingredient ingredient);
    }
}