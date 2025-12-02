using FitnessApp.Models;

namespace FitnessApp.Domain.Interfaces
{
    public interface IWorkoutService
    {
        public ApiResponse<object> GetByBiginner();
        public ApiResponse<object> GetByIntermediate();
        public ApiResponse<object> GetByAdvanced();
        public ApiResponse<object> GetWorkoutsById(int id);
    }
}
