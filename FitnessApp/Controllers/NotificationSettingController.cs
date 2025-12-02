using FitnessApp.Bl;
using FitnessApp.DAL;
using FitnessApp.Domain.Interfaces;
using FitnessApp.DTOs;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationSettingController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ISettingService _settingService;

    public NotificationSettingController(IUserService user, ISettingService settingService)
    {
        _userService = user;
        _settingService = settingService;
    }

    

    [HttpGet("GetSettings")]
    public async Task<IActionResult> GetSettings()
    {
        try
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user == null)
                return Unauthorized(ApiResponse<object>.Fail("Unauthorized"));

            var settings = _settingService.GetUserSetting(user.Id);

            if (settings == null)
                return BadRequest(ApiResponse<object>.Fail("Not settings for this user"));
            

            return Ok(ApiResponse<object>.Ok(settings)); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
        }
    }

    [HttpPost("UpdateSettings")]
    public async Task<IActionResult> UpdateSettings([FromBody] NotificationSettingsDto model)
    {
       
        try
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user == null)
                return Unauthorized(ApiResponse<object>.Fail("Unauthorized"));

            var settingsResponse = await _settingService.UpdateSettingAsync(user.Id,model);

            if (!settingsResponse.Data)
                return BadRequest(settingsResponse);

           
                return Ok(ApiResponse<object>.Ok(null, "Notification settings updated successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.Fail("unexpected server error"));
        }
    }
}
