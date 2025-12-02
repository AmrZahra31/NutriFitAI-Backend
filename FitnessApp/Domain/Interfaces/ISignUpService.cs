using FitnessApp.DTOs;
using FitnessApp.Models;

namespace FitnessApp.Domain.Interfaces
{
    public interface ISignUpService
    {
        public Task<ApiResponse<object>> SignUpAsync(UserModel user, CancellationToken ct = default);
    }
}
