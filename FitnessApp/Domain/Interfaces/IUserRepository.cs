using FitnessApp.Domain.Entities;

namespace FitnessApp.Domain.Interfaces
{
    public interface IUserRepository
    {
        public bool UpdateCurentState(string userId, bool _CurrentState);
        public bool updateUser(ApplicationUsers user);
        public ApplicationUsers GetUserById(string userId);
    }
}
