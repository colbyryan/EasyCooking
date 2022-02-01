using EasyCooking.Models;
using System.Collections.Generic;

namespace EasyCooking.Repositories
{
    public interface ICategoryRepository
    {
        void CreateCategory(Category category);
        void Delete(int id);
        List<Category> GetAll();
        Category GetCategoryById(int id);
        void Update(Category category);
    }
}