using FitnessApp.DTOs;
using FitnessApp.Models;

namespace FitnessApp.Domain.Interfaces
{
    public interface IPasswordService
    {
        public Task<ApiResponse<bool>> ChangePasswordAsync(PasswordModel passwords);
        public Task<ApiResponse<bool>> VerifyOtpAsync(VerifyOtpRequest model);
        public Task<ApiResponse<bool>> ResendCodeAsync(string email);
        public Task<ApiResponse<bool>> ResetPasswordAsync(PasswordModel resetPassword);
    }
}
