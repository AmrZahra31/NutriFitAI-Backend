using FitnessApp.DAL.Repositories;
using FitnessApp.DTOs;
using FitnessApp.Models;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Interfaces;

namespace FitnessApp.Bl
{
    

    public class SettingService :ISettingService
    {
        IUserSettingeRepository _userSetting;
        IUserService _userService;
        ILogger<SettingService> _logger;
        public SettingService(IUserSettingeRepository userSetting,ILogger<SettingService>logger,IUserService userService)
        {
            _userSetting = userSetting;
            _logger = logger;
            _userService = userService;
        }
        public string GetLanguage(string userId)
        {
            try
            {
                return _userSetting.GetId(userId)?.Language;
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public string ChangeLanguage(string userId,string Language)
        {
            try
            {
                var tbUserSetting = _userSetting.GetId(userId);

                tbUserSetting.Language = Language;

                if (Language.ToLower() == "arabic" || Language.ToLower() == "english")

                {
                    _userSetting.Save(tbUserSetting);
                    return "Language updated successfully";
                }
                return "Enter a valid format for language (arabic or english)";
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public NotificationSettingsDto GetUserSetting(string userId)
        {
            try
            {
                var settings = _userSetting.GetId(userId);

                if (settings == null)
                    return null;

                var result = new NotificationSettingsDto
                {
                    Sound = settings.Sound,
                    Theme = settings.Theme,
                    NextWorkoutTime = settings.NextWorkoutTime,
                    PreferredUnits = settings.PreferredUnits,
                    NotificationsEnabled = settings.NotificationsEnabled,
                    GeneralNotification = settings.GeneralNotification,
                    ReminderToDrinkWater = settings.ReminderToDrnkWater,
                    NextMealTime = settings.NextMealTime
                };
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<bool>> UpdateSettingAsync(string userId, NotificationSettingsDto model)
        {
            try
            {
                var settings = _userSetting.GetId(userId);
                var user = await _userService.GetCurrentUserAsync();
                if (settings == null && user == null)
                    return ApiResponse<bool>.Fail("Not settings for this user");

                settings.Sound = model.Sound ?? settings.Sound;
                settings.Theme = (model.Theme?.ToLower()) ?? settings.Theme;
                if (settings.Theme != "light" && settings.Theme != "dark")
                    return ApiResponse<bool>.Fail("you must enter light or dark only");
                settings.NextWorkoutTime = model.NextWorkoutTime ?? settings.NextWorkoutTime;
                settings.PreferredUnits = (model.PreferredUnits?.ToLower()) ?? settings.PreferredUnits;
                if (settings.PreferredUnits != "kg" && settings.PreferredUnits != "lbs")
                    return ApiResponse<bool>.Fail("you must enter kg or lbs only");

                if (settings.PreferredUnits == "lbs")
                {
                    user.Weight = (int)(user.Weight * 0.45359237);
                }
                if (!_userService.UpdateUser(user))
                    return ApiResponse<bool>.Fail("Can not update this user");
                settings.NotificationsEnabled = model.NotificationsEnabled ?? settings.NotificationsEnabled;
                settings.GeneralNotification = model.GeneralNotification ?? settings.GeneralNotification;
                settings.ReminderToDrnkWater = model.ReminderToDrinkWater ?? settings.ReminderToDrnkWater;
                settings.NextMealTime = model.NextMealTime ?? settings.NextMealTime;

                if (_userSetting.Save(settings))
                    return ApiResponse<bool>.Ok(true, "Notification settings updated successfully");
                return ApiResponse<bool>.Fail("Can not save the setting for this user");
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

    }
}
