using FitnessApp.Bl;
using FitnessApp.DAL.context;
using FitnessApp.Models;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Interfaces;

namespace FitnessApp.DAL.Repositories
{

    public class GenDietRepository : IGenDietRepository
    {
        public GenDietRepository(FitnessAppContext ctx,IDietPlanRepository dietPlan)
        {
            context = ctx;
            _dietPlanService = dietPlan;
        }
        FitnessAppContext context;
        IDietPlanRepository _dietPlanService;

        public async Task<bool> DeleteOldMealsAsync(ApplicationUsers user)
        {
            
                var today = DateTime.Today;

                var oldplans = _dietPlanService.GetCreatedAtId(today, user);

                if (oldplans.Any())
                {

                    foreach (var oldPlan in oldplans)
                    {
                        var oldPlanMeals = context.TbDietPlanMeals.Where(pm => pm.PlanId == oldPlan.PlanId).ToList();

                        foreach (var oldPlanMeal in oldPlanMeals)
                        {
                            var meal = context.TbMeals.FirstOrDefault(m => m.MealId == oldPlanMeal.MealId);
                            if (meal != null)
                            {
                                context.TbMeals.Remove(meal);
                            }

                            context.TbDietPlanMeals.Remove(oldPlanMeal);
                        }

                        context.TbDietPlans.Remove(oldPlan);
                    }

                    await context.SaveChangesAsync();
                }
                return true;
            }
            
        

        public async Task<List<Meal>> AddNewMealsAsync(ApplicationUsers user, TbDietPlan tbDietPlan, MealResponseModel result)
        {
           
                

                context.TbDietPlans.Add(tbDietPlan);
                await context.SaveChangesAsync();

                var allMeals = new List<Meal>();
                allMeals.AddRange(result.Suggested_Recipes.Breakfast);
                allMeals.AddRange(result.Suggested_Recipes.Snack);
                allMeals.AddRange(result.Suggested_Recipes.Lunch);
                allMeals.AddRange(result.Suggested_Recipes.Dinner);

                foreach (var meal in allMeals)
                {


                    var integrationParts = StringParsingHelper.ParseStringList(meal.RecipeIngredientParts);
                    var integrationQuantities = StringParsingHelper.ParseStringList(meal.RecipeIngredientQuantities);
                    var tbMeal = new TbMeal
                    {
                        Name = meal.Name,
                        MealType = meal.MealType,
                        Calories = meal.Calories,
                        Price = meal.EstimatedPriceEGP,
                        RecipeIngredientParts = integrationParts,
                        RecipeIngredientQuantities = integrationQuantities,
                    };

                    context.TbMeals.Add(tbMeal);
                    await context.SaveChangesAsync();
                    int CatId = 1;
                    if (meal.MealType == "snack")
                        CatId = 2;
                    else if (meal.MealType == "lunch")
                        CatId = 3;
                    else if (meal.MealType == "dinner")
                        CatId = 4;

                    var tbDietPlanMeal = new TbDietPlanMeal()
                    {
                        PlanId = tbDietPlan.PlanId,
                        MealId = tbMeal.MealId,
                        CategoryId = CatId,
                    };

                    context.TbDietPlanMeals.Add(tbDietPlanMeal);

                }
                
                await context.SaveChangesAsync();
                
                return allMeals;
           
        }

        

        
    }

}
