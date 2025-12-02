using FitnessApp.DAL.context;
using FitnessApp.Domain.Entities;
using FitnessApp.DTOs;
using FitnessApp.Models;
using FitnessApp.Domain.Interfaces;

namespace FitnessApp.Bl
{


    public class DietService : IDietService
    {
        private readonly IDietApiClient _api;
        private readonly FitnessAppContext _context;
        private readonly IGenDietRepository _genDiet;
        private readonly IUserService _userService;
        private readonly ILogger<DietService> _logger;
        private readonly IUserSettingeRepository _settingService;
        private readonly IDietPlanRepository _dietplan;

        public DietService(IDietApiClient api, FitnessAppContext db, IGenDietRepository genDiet,
            IUserService userService, ILogger<DietService> logger, IUserSettingeRepository settingService,
            IDietPlanRepository dietPlan)
        {
            _api = api;
            _context = db;
            _genDiet = genDiet;
            _userService = userService;
            _logger = logger;
            _settingService = settingService;
            _dietplan = dietPlan;
        }

        public async Task<ApiResponse<object>> GenerateMealsAsync(DietRequestModel model, CancellationToken ct = default)
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user == null)
                return ApiResponse<object>.Fail("Unauthorized");

            if (model.Daily_Budget < 5)
                return ApiResponse<object>.Fail("Budget must be at least 5.");

            var userWeight = _settingService.GetId(user.Id).PreferredUnits == "lbs"? (int)(model.Weight * 2.20462262f):model.Weight;
            

            // Call external API with cancellation token
            HttpResponseMessage response;
            try
            {
                response = await _api.GenerateDietAsync(model, ct);
            }
            catch (TaskCanceledException tce)
            {
                _logger.LogWarning(tce, "Diet API timed out or request canceled for user {UserId}", user.Id);
                return ApiResponse<object>.Fail("External service timeout.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Diet API for user {UserId}", user.Id);
                return ApiResponse<object>.Fail("Cannot reach external service.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync(ct);
                _logger.LogWarning("Diet API returned {Status} for user {UserId}: {Error}", (int)response.StatusCode, user.Id, err);
                return ApiResponse<object>.Fail("External API error.", new { status = (int)response.StatusCode, body = err });
            }

            var result = await response.Content.ReadFromJsonAsync<MealResponseModel>(cancellationToken: ct);
            if (result == null)
            {
                _logger.LogError("Diet API returned null body for user {UserId}", user.Id);
                return ApiResponse<object>.Fail("Invalid response from external service.");
            }

            // Transaction: delete old, add new
            using var transaction = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                var deleted = await _genDiet.DeleteOldMealsAsync(user);
                if (!deleted)
                {
                    await transaction.RollbackAsync(ct);
                    _logger.LogError("Failed deleting old meals for user {UserId}", user.Id);
                    return ApiResponse<object>.Fail("Failed to delete old meals.");
                }

                var dietPlan = new TbDietPlan
                {
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    Budget = model.Daily_Budget,
                    TotalCalories = result.Daily_Calories,
                    CurrentState = true
                };

                var meals = await _genDiet.AddNewMealsAsync(user, dietPlan, result);
                if (meals == null)
                {
                    await transaction.RollbackAsync(ct);
                    _logger.LogError("Failed adding new meals for user {UserId}", user.Id);
                    return ApiResponse<object>.Fail("Failed to create meals.");
                }

                await transaction.CommitAsync(ct);
                return ApiResponse<object>.Ok(meals, "Meals generated successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                _logger.LogError(ex, "Transaction error generating meals for user {UserId}", user.Id);
                return ApiResponse<object>.Fail("Server error while saving meals.");
            }
        }

        public string IsCurrentState(string userId)
        {
            try
            {
                var isexist = _dietplan.IsCurrentState(userId);
                return isexist ? "Plan is exists" : "Plan is not exists";
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public List<Meal2> GetListOfMeals(DateOnly? date,string userId)
        {
            var meals = _dietplan.GetMealsByDate(date ?? DateOnly.FromDateTime(DateTime.Now), userId);
            if (meals == null)
                return null;
            try
            {
                List<Meal2> meals2 = new List<Meal2>
                {
                       new Meal2
                       {
                           Name=meals[0].Name,
                           MealType=meals[0].MealType,
                           Calories=meals[0].Calories,
                           EstimatedPriceEGP=meals[0].Price,
                           RecipeIngredientParts=meals[0].RecipeIngredientParts,
                           RecipeIngredientQuantities=meals[0].RecipeIngredientQuantities,
                           combinedIngredients=meals[0].RecipeIngredientQuantities
                           .Zip(meals[0].RecipeIngredientParts, (quantity, part) => $"{quantity} {part}")
                            .ToList()
                       },
                       new Meal2
                       {
                           Name=meals[1].Name,
                           MealType=meals[1].MealType,
                           Calories=meals[1].Calories,
                           EstimatedPriceEGP=meals[1].Price,
                           RecipeIngredientParts=meals[1].RecipeIngredientParts,
                           RecipeIngredientQuantities=meals[1].RecipeIngredientQuantities,
                           combinedIngredients=meals[1].RecipeIngredientQuantities
                           .Zip(meals[1].RecipeIngredientParts, (quantity, part) => $"{quantity} {part}")
                            .ToList()
                       },
                       new Meal2
                       {
                           Name=meals[2].Name,
                           MealType=meals[2].MealType,
                           Calories=meals[2].Calories,
                           EstimatedPriceEGP=meals[2].Price,
                           RecipeIngredientParts=meals[2].RecipeIngredientParts,
                           RecipeIngredientQuantities=meals[2].RecipeIngredientQuantities,
                           combinedIngredients=meals[2].RecipeIngredientQuantities
                           .Zip(meals[2].RecipeIngredientParts, (quantity, part) => $"{quantity} {part}")
                            .ToList()
                       },
                       new Meal2
                       {
                           Name=meals[3].Name,
                           MealType=meals[3].MealType,
                           Calories=meals[3].Calories,
                           EstimatedPriceEGP=meals[3].Price,
                           RecipeIngredientParts=meals[3].RecipeIngredientParts,
                           RecipeIngredientQuantities=meals[3].RecipeIngredientQuantities,
                           combinedIngredients=meals[3].RecipeIngredientQuantities
                           .Zip(meals[3].RecipeIngredientParts, (quantity, part) => $"{quantity} {part}")
                            .ToList()
                       },
                };
                return meals2;
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }

        }
        
        public List<TbMeal> GetMealsByDate(DateOnly? date,string userId)
        {
            try
            {
                var meals = _dietplan.GetMealsByDate(date ?? DateOnly.FromDateTime(DateTime.Now), userId);
                if (meals == null)
                    return null;
                return meals;
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public int GetMealsCount(DateOnly? date, string userId)
        {
            try
            {
                var meals = _dietplan.GetMealsByDate(date ?? DateOnly.FromDateTime(DateTime.Now), userId);
                if (meals == null)
                    return 0;
                return meals?.Count ?? 0;
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public float GetMealsTotalPrice(DateOnly? date, string userId)
        {
            try
            {
                return _dietplan.GetPrice(date ?? DateOnly.FromDateTime(DateTime.Now), userId);
                
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public float GetMealsTotalCalories(DateOnly? date, string userId)
        {
            try
            {
                return _dietplan.GetTotalCalories(date ?? DateOnly.FromDateTime(DateTime.Now), userId);

            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

    }

}
