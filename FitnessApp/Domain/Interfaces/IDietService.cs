using FitnessApp.Domain.Entities;
using FitnessApp.DTOs;
using FitnessApp.Models;

namespace FitnessApp.Domain.Interfaces
{
    public interface IDietService
    {
        Task<ApiResponse<object>> GenerateMealsAsync(DietRequestModel model, CancellationToken ct = default);
        public string IsCurrentState(string userId);
        public List<Meal2> GetListOfMeals(DateOnly? date, string userId);
        public List<TbMeal> GetMealsByDate(DateOnly? date, string userId);
        public int GetMealsCount(DateOnly? date, string userId);
        public float GetMealsTotalPrice(DateOnly? date, string userId);
        public float GetMealsTotalCalories(DateOnly? date, string userId);
    }
}
