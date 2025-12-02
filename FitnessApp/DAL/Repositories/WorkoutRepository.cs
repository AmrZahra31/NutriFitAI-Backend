using FitnessApp.DAL.context;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Interfaces;

namespace FitnessApp.DAL.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        public WorkoutRepository(FitnessAppContext context)
        {
            _context = context;
        }
        FitnessAppContext _context;

        public List<TbWorkout> GetByAdvanced()
        {
            try
            {
                return _context.TbWorkouts.Where(i => i.Level == "Advanced").ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine("No Item with GetByAdvanced in Workout ", ex.Message);
                throw;
            }
        }

        public List<TbWorkout> GetByBiginner()
        {
            try
            {
                return _context.TbWorkouts.Where(i => i.Level == "Biginner").ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine("No Item with GetByBiginner in Workout ", ex.Message);
                throw;
            }
        }

        public List<TbWorkout> GetByIntermediate()
        {
            try
            {
                return _context.TbWorkouts.Where(i => i.Level == "intermediate").ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine("No Item with GetByIntermediate in Workout ", ex.Message);
                throw;
            }
        }
        public TbWorkout GetById(int id)
        {
            try
            {
                return _context.TbWorkouts.FirstOrDefault(i => i.WorkoutId==id);
            }
            catch(Exception ex)
            {
                Console.WriteLine("No Item with GetById in Workout ", ex.Message);
                throw;
            }
        }
    }
}
