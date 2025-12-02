using FitnessApp.Domain.Entities;

namespace FitnessApp.Domain.Interfaces
{
    public interface IUserService
    {
        public string? GetEmail();
        public Task<ApplicationUsers?> GetCurrentUserAsync();
        public bool UpdateCurentState(string userId, bool _CurrentState);
        public Task<bool> GetCurentStateAsync();
        public bool UpdateUser(ApplicationUsers user);
        public ApplicationUsers GetUserById(string userId);
        public Task<ApplicationUsers> GetUserByEmailAsync(string email);
    }
}
