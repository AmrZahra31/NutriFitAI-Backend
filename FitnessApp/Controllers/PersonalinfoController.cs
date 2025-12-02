using FitnessApp.Bl;
using FitnessApp.DAL;
using FitnessApp.DTOs;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Interfaces;


namespace FitnessApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonalinfoController : ControllerBase
    {
        private readonly IPersonalInfoService _personalService;
        private readonly IUserService _userService;

        public PersonalinfoController(IUserService user, IPersonalInfoService personalService)
        {
            _userService = user;
            _personalService = personalService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user == null) return Unauthorized(ApiResponse<object>.Fail("Unauthorized"));

                var response = _personalService.GetALlPersonalInfo(user);
                if (response == null)
                    return BadRequest(response);
                return Ok(response);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error occurred."));
            }
        }



        [HttpPost("Updateprofile")]
        public async Task<IActionResult> Updateprofile([FromBody] profileDto model)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user == null) return Unauthorized(ApiResponse<object>.Fail("Unauthorized"));

                var response = await _personalService.UpdateUserProfileAsync(user, model);
                if (!response.Data)
                    return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error occurred."));
            }
        }

    }


}
