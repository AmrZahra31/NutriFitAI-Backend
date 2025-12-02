using FitnessApp.Domain.Entities;
using FitnessApp.DTOs;
using FitnessApp.Models;

namespace FitnessApp.Domain.Interfaces
{
    public interface IPersonalInfoService
    {
        public ApiResponse<object> GetALlPersonalInfo(ApplicationUsers user);
        public Task<ApiResponse<bool>> UpdateUserProfileAsync(ApplicationUsers user, profileDto model);
    }
}
