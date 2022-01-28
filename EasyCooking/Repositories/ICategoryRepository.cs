using EasyCooking.Models;
using System.Collections.Generic;

namespace EasyCooking.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
    }
}