using FitnessApp.Controllers;
using FitnessApp.DAL.Repositories;
using FitnessApp.DTOs;
using FitnessApp.Models;
using Microsoft.AspNetCore.Identity;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Interfaces;
namespace FitnessApp.Bl
{
    

    public class PersonalInfoService :IPersonalInfoService
    {
        ILogger<PersonalInfoService> _logger;
        IUserSettingeRepository _userSetting;
        UserManager<ApplicationUsers> _userManager;
        public PersonalInfoService(ILogger<PersonalInfoService>logger,IUserSettingeRepository userSetting, UserManager<ApplicationUsers> userManager)
        {
            _logger = logger;
            _userSetting = userSetting;
            _userManager = userManager;
        }

        public ApiResponse<object> GetALlPersonalInfo(ApplicationUsers user)
        {
            try
            {
                var setting = _userSetting.GetId(user.Id);
                float weight = 50;              
                
                return ApiResponse<object>.Ok(new
                {
                    UserName = user.UserName,
                    ImagePath = user.Image,
                    Height = user.Height,
                    Weight = user.Weight,
                    Age = user.Age,
                    Goal = user.Goal,
                    ActivityIntensity = user.ActivityIntensity,
                });
            }

            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<bool>> UpdateUserProfileAsync(ApplicationUsers user, profileDto model)
        {
            try
            {
                int count = 0;
                if (model.ImagePath != null)
                {
                    if (model.ImagePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                        || model.ImagePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                        || model.ImagePath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                    {
                        user.Image = model.ImagePath;
                        count++;
                    }
                    else
                        return ApiResponse<bool>.Fail("please, enter a valid image path");
                }
                if (model.UserHeight != null)
                {
                    if (model.UserHeight < 120) 
                        return ApiResponse<bool>.Fail("the height is very small must not less than 120cm");
                    user.Height = model.UserHeight ?? 0;
                    count++;
                }
                if (model.UserWeight != null)
                {
                    var setting = _userSetting.GetId(user.Id);

                    var weight = model.UserWeight;
                    if (setting.PreferredUnits == "lbs")
                    {
                        if (weight < 12.5)
                            return ApiResponse<bool>.Fail("the weight is very small must not less than 25kg or 12lbs");
                    }
                    else
                    {
                        if (weight < 25)
                            return ApiResponse<bool>.Fail("the weight is very small must not less than 25kg or 12lbs");
                    }
                    user.Weight = model.UserWeight??user.Weight;
                    count++;
                }
                if (model.UserGender != null)
                {
                    var gender = model.UserGender.ToLower();
                    if (gender != "male" && gender != "female")
                        return ApiResponse<bool>.Fail("Enter a valid Gender.");
                    user.Gender =gender;
                    count++;
                }
                if (model.UserAge != null)
                {
                    count++;
                    user.Age = model.UserAge ?? 0;
                }

                if (count == 0)
                    return ApiResponse<bool>.Fail("please, enter any attribute for update");
                await _userManager.UpdateAsync(user);
                return ApiResponse<bool>.Ok(true, "the profile is updated successfully");
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }
    }
}
