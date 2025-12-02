using FitnessApp.DTOs;
using FitnessApp.Models;

namespace FitnessApp.Domain.Interfaces
{
    public interface ISettingService
    {
        public string GetLanguage(string userId);
        public string ChangeLanguage(string userId, string Language);
        public NotificationSettingsDto GetUserSetting(string userId);
        public Task<ApiResponse<bool>> UpdateSettingAsync(string userId, NotificationSettingsDto model);
    }
}
