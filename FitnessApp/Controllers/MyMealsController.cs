using FitnessApp.Bl;
using FitnessApp.DAL.Repositories;
using FitnessApp.Domain.Entities;
using FitnessApp.DTOs;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Interfaces;



namespace FitnessApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MyMealsController : ControllerBase
    {
        private readonly IDietPlanRepository _dietPlanService;
        IUserService _userService;
        IDietService _dietService;

        public MyMealsController(IUserService user, IDietPlanRepository dietPlanService, IDietService dietService)
        {
            _userService = user;
            _dietPlanService = dietPlanService;
            _dietService = dietService;
        }

        
        [HttpGet("isexist")]
        public async Task<ActionResult<bool>> IsExist()
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user == null)       
                    return Unauthorized(ApiResponse<object>.Fail("Unauthorized"));

                var isExist = _dietService.IsCurrentState(user.Id);
                
                    return Ok(ApiResponse<bool>.Ok(!isExist.Contains("not"),isExist));
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ApiResponse<bool>.Fail("An unexpected error"));
            }
        }

        

        [HttpGet("meals")]
        public async Task<ActionResult<List<TbMeal>>> GetMealsByDate([FromQuery] DateOnly? date = null)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user == null)
                {
                    return Unauthorized(ApiResponse<object>.Fail("Unauthorized"));
                }

                
                var meals = _dietService.GetListOfMeals(date ?? DateOnly.FromDateTime(DateTime.Now),user.Id);

                if (meals == null)
                {

                    return NotFound(ApiResponse<object>.Fail("the meals is not found"));

                }

                return Ok(ApiResponse<object>.Ok(meals));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
        }

        [HttpGet("mealcount")]
        public async Task<ActionResult<int>> GetMealCount([FromQuery] DateOnly? date = null)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user == null)
                    return Unauthorized(ApiResponse<object>.Fail("Unauthorized"));

                var count = _dietService.GetMealsCount(date ?? DateOnly.FromDateTime(DateTime.Now), user.Id);
                return Ok(ApiResponse<object>.Ok(count));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
        }


        [HttpGet("price")]
        public async Task<ActionResult<float>> GetPrice([FromQuery] DateOnly? date = null)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user == null)
                    return Unauthorized(ApiResponse<object>.Fail("Unauthorized"));

                var price = _dietService.GetMealsTotalPrice(date ?? DateOnly.FromDateTime(DateTime.Now), user.Id);
                return Ok(ApiResponse<object>.Ok(price));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ApiResponse<object>.Fail("expected meals for this day"));
            }
        }

        [HttpGet("totalcalories")]
        public async Task<ActionResult<float>> GetTotalCalories([FromQuery] DateOnly? date = null)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user == null)
                    return Unauthorized(ApiResponse<object>.Fail("Unauthorized"));

                var totalCalories = _dietService.GetMealsTotalCalories(date ?? DateOnly.FromDateTime(DateTime.Now),user.Id);
                return Ok(ApiResponse<object>.Ok(totalCalories));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ApiResponse<object>.Fail("expected meals for this day"));
            }
        }

        [HttpPost("generateDiet")]
        public async Task<IActionResult> GenerateMeals([FromBody] DietRequestModel model, CancellationToken ct)
        {
            var result = await _dietService.GenerateMealsAsync(model, ct);

            if (!result.Success)
            {
                // Map errors to proper status codes as needed
                if (result.Message == "Unauthorized") return Unauthorized(result);
                if (result.Message?.Contains("External") == true) return StatusCode(503, result);
                return StatusCode(500, result);
            }

            return Ok(result);
        }


    }

}

