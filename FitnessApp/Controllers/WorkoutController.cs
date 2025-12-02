using FitnessApp.Bl;
using FitnessApp.DAL;
using FitnessApp.Domain.Interfaces;
using FitnessApp.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitnessApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        IWorkoutService _workout;
        public WorkoutController(IWorkoutService workout) 
        {
            _workout = workout;
        }
        
        [HttpGet("GetBiginner")]
        public IActionResult GetBiginner()
        {
            try
            {
                var Workouts = _workout.GetByBiginner();
                if (Workouts == null)
                    return NotFound(Workouts);
                return Ok(Workouts);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
        }

        [HttpGet("Getintermediate")]
        public IActionResult Getintermediate()
        {
            try
            {
                var Workouts = _workout.GetByIntermediate();
                if (Workouts == null)
                    return NotFound(Workouts);
                return Ok(Workouts);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
        }

        [HttpGet("GetAdvanced")]
        public IActionResult GetAdvanced()
        {
            try
            {
                var Workouts = _workout.GetByAdvanced();
                if (Workouts == null)
                    return NotFound(Workouts);
                return Ok(Workouts);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
        }

        // Get one workout by ID
        [HttpGet("GetWorkoutById/{id}")]
        public IActionResult GetWorkoutByName(int id)
        {
            try
            {

                var singleWorkout = _workout.GetWorkoutsById(id);
                if (singleWorkout == null)
                    return NotFound(singleWorkout);

                return Ok(singleWorkout);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
        }

    }
}
