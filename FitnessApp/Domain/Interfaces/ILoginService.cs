using FitnessApp.DTOs;
using FitnessApp.Models;

namespace FitnessApp.Domain.Interfaces
{
    public interface ILoginService
    {
        public Task<ApiResponse<bool>> AuthorizedAsync(UserLoginModel user);
    }
}
