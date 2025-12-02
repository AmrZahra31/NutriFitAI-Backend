using FitnessApp.Domain.Entities;
using FitnessApp.Models;

namespace FitnessApp.Domain.Interfaces
{
    public interface IGenDietRepository
    {
        public Task<bool> DeleteOldMealsAsync(ApplicationUsers user);
        public Task<List<Meal>> AddNewMealsAsync(ApplicationUsers user, TbDietPlan tbDietPlan, MealResponseModel result);
    }
}
