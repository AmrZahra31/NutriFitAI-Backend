using FitnessApp.Bl;
using FitnessApp.DAL;
using FitnessApp.Domain.Interfaces;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitnessApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LanguageController : ControllerBase
    {

        public LanguageController(ISettingService userSetting,IUserService user)
        {
            _userSetting = userSetting;
            _user = user;
        }

        
        ISettingService _userSetting;
        IUserService _user;
        
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var user=await _user.GetCurrentUserAsync();
                if (user == null)
                   return Unauthorized(ApiResponse<object>.Fail("Unauthorized"));
                
                return Ok(ApiResponse<object>.Ok(_userSetting.GetLanguage(user.Id)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later")); 
            }
            
        }

        // POST api/<ChangeLanguageController>
        [HttpPost("ChangeLanguage")]
        public async Task<IActionResult> ChangeLanguage([FromBody] string Language)
        { 
            try
            {
                var user = await _user.GetCurrentUserAsync();
                if (user == null)
                    return Unauthorized(ApiResponse<object>.Fail("Unauthorized"));
                var thatChanged = _userSetting.ChangeLanguage(user.Id, Language);
                    return Ok(ApiResponse<bool>.Ok(thatChanged.Contains("successfully"), thatChanged));
                
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
        }

    }
}
