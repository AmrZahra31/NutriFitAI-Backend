using FitnessApp.Domain.Entities;

namespace FitnessApp.Domain.Interfaces
{
    public interface IWorkoutRepository
    {
        List<TbWorkout> GetByBiginner();
        List<TbWorkout> GetByIntermediate();
        List<TbWorkout> GetByAdvanced();
        TbWorkout GetById(int id);
    }
}
