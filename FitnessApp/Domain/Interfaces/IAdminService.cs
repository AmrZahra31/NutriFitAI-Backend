using FitnessApp.Domain.Entities;
using FitnessApp.Models;

namespace FitnessApp.Domain.Interfaces
{
    public interface IAdminService
    {
        public Task<ApiResponse<bool>> AddAdminAsync(ApplicationUsers user);
        public Task<ApiResponse<bool>> RemoveAdminAsync(ApplicationUsers user);
        public ApiResponse<bool> UpdateCurrentStateUser(ApplicationUsers user, bool state);
    }
}
