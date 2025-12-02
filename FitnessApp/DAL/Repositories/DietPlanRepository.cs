using FitnessApp.DAL.context;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.DAL.Repositories
{
    public class DietPlanRepository : IDietPlanRepository
    {
        public DietPlanRepository(FitnessAppContext ctx)
        {
            context = ctx;
        }
        FitnessAppContext context;

        public TbDietPlan GetId(string userId)
        {
            try
            {
                var plan= context.TbDietPlans.Where(i => i.UserId.Equals(userId)).FirstOrDefault();

                return plan;
            }
            catch(Exception ex)
            {
                Console.WriteLine("No Item with GetId in DietPlan ",ex.Message);
                throw;
            }
        }
        public List<TbDietPlan> GetCreatedAtId(DateTime today, ApplicationUsers user)
        {
            try
            {
                return context.TbDietPlans.Where(i => i.CreatedAt == today && i.UserId == user.Id).ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine("No Item with GetCreatedAtId in DietPlan ", ex.Message);
                throw;
            }
        }

        public bool Save(TbDietPlan tbDietPlans)
        {
            try
            {
                if (tbDietPlans.PlanId == 0)
                {

                    context.TbDietPlans.Add(tbDietPlans);
                    context.SaveChanges();
                    return true;
                }
                else
                {

                    //context.Entry(tbUserSetting).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    context.Entry(tbDietPlans).State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("No Item with Save in DietPlan ", ex.Message);
                throw;
            }
            
        }

        public bool IsCurrentState(string userId)
        {
            try
            {
                var tbDietPlan = GetId(userId);
                if(tbDietPlan!=null)
                    return tbDietPlan.CurrentState;
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine("No Item with IsCurrentState in DietPlan ", ex.Message);
                throw;
            }


        }

        public async Task<bool> Delete(TbDietPlan tbDietPlans)
        {
            try
            {
                //context.Entry(tbUserSetting).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                var dietPlan = await context.TbDietPlans.FindAsync(tbDietPlans.PlanId);
                if (dietPlan == null)
                    return false;
                else
                {
                    context.TbDietPlans.Remove(tbDietPlans);
                    context.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Delete DietPlain Error: " + ex.Message);
                throw;
            }

           


        }

        public List<TbMeal> GetMealsByDate(DateOnly dateOnly, string userId)
        {
            try
            {
                var plan = context.TbDietPlans
        .Include(p => p.TbDietPlanMeals)
            .ThenInclude(pm => pm.TbMeal)
        .Include(p => p.TbDietPlanMeals)
            .ThenInclude(pm => pm.TbCategory)
        .FirstOrDefault(p => DateOnly.FromDateTime(p.CreatedAt) == dateOnly && p.UserId == userId);

                if (plan == null)
                    return null;

                return plan.TbDietPlanMeals.Select(pm => pm.TbMeal).ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine("No Item with GetMealsByDate in DietPlan ", ex.Message);
                throw;
            }
            
        }

        public float GetPrice( DateOnly date, string userId)
        {
            try
            {
                var plan = context.TbDietPlans.FirstOrDefault(i => i.UserId == userId && DateOnly.FromDateTime(i.CreatedAt) == date);
                if (plan == null)
                    return 0;
                else
                    return plan.Budget;
            }
            catch(Exception ex)
            {
                Console.WriteLine("No Item with GetPrice in DietPlan ", ex.Message);
                throw;
            }
           
        }

        public float GetTotalCalories(DateOnly date, string userId)
        {
            try
            {
                var plan = context.TbDietPlans.FirstOrDefault(i => i.UserId == userId && DateOnly.FromDateTime(i.CreatedAt) == date);
                if (plan == null)
                    return 0;
                else
                    return plan.TotalCalories;
            }
            catch(Exception ex)
            {
                Console.WriteLine("No Item with GetId in DietPlan ", ex.Message);
                throw;
            }
            
        }
    }
}
