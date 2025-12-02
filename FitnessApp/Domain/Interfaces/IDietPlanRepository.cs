using FitnessApp.Domain.Entities;

namespace FitnessApp.Domain.Interfaces
{
    public interface IDietPlanRepository
    {
        TbDietPlan GetId(string userId);
        public bool Save(TbDietPlan tbUserSetting);
        bool IsCurrentState(string userId);
        Task<bool> Delete(TbDietPlan tbUserSetting);
        List<TbMeal> GetMealsByDate(DateOnly dateOnly, string userId);
        public float GetTotalCalories(DateOnly date, string userId);
        public float GetPrice(DateOnly date, string userId);
        public List<TbDietPlan> GetCreatedAtId(DateTime today, ApplicationUsers user);
    }
}
