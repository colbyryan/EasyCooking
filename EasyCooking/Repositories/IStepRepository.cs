using EasyCooking.Models;
using System.Collections.Generic;

namespace EasyCooking.Repositories
{
    public interface IStepRepository
    {
        void Add(Step step);
        List<Step> GetAll();
        List<Step> GetAllByRecipeId(int Id);
        Step GetById(int Id);
        void Remove(int id);
        void UpdateStep(Step step);
    }
}