using FitnessApp.DAL.Repositories;
using FitnessApp.Domain.Interfaces;
using FitnessApp.Models;

namespace FitnessApp.Bl
{

    public class WorkoutService :IWorkoutService
    {
        IWorkoutRepository _workout;
        ILogger<IWorkoutService> _logger;

        public WorkoutService(IWorkoutRepository workout, ILogger<IWorkoutService> logger)
        {
            _workout = workout;
            _logger = logger;
        }

        public ApiResponse<object> GetByBiginner()
        {
            try
            {
                var Workouts = _workout.GetByBiginner();
                if (Workouts == null)
                    return ApiResponse<object>.Fail("not found workouts for Biginners");
                return ApiResponse<object>.Ok(Workouts);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public ApiResponse<object> GetByIntermediate()
        {
            try
            {
                var Workouts = _workout.GetByIntermediate();
                if (Workouts == null)
                    return ApiResponse<object>.Fail("not found workouts for Intermediate");
                return ApiResponse<object>.Ok(Workouts);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public ApiResponse<object> GetByAdvanced()
        {
            try
            {
                var Workouts = _workout.GetByAdvanced();
                if (Workouts == null)
                    return ApiResponse<object>.Fail("not found workouts for Intermediate");
                return ApiResponse<object>.Ok(Workouts);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public ApiResponse<object> GetWorkoutsById(int id)
        {
            try
            {
                var singleWorkout = _workout.GetById(id);
                if (singleWorkout == null)
                    return ApiResponse<object>.Fail($"Workout with ID: {id} is not found.");

                return ApiResponse<object>.Ok(singleWorkout);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }
    }
}
