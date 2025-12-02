using FitnessApp.Bl;
using FitnessApp.Domain.Interfaces;
using FitnessApp.DTOs;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitnessApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;

        public AdminController(IAdminService adminService,IUserService userService)
        {
            _adminService = adminService;
            _userService = userService;
        }
        
        [HttpGet()]
        public async Task<IActionResult> AddAdmin([FromBody]string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if(user==null)
                    return Unauthorized(ApiResponse<object>.Fail("Unauthorized"));

                var response = await _adminService.AddAdminAsync(user);
                if (!response.Data)
                    return BadRequest(response);
                return Ok(response);
            }
            catch
            {
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
        }

        
        [HttpDelete()]
        public async Task<IActionResult> RemoveAdmin([FromBody] string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                    return Unauthorized(ApiResponse<object>.Fail("Unauthorized"));

                var response = await _adminService.RemoveAdminAsync(user);
                if (!response.Data)
                    return BadRequest(response);
                return Ok(response);
            }
            catch
            {
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateCurrentStateUser([FromBody] UpdateStateDto stateDto)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(stateDto.Email);
                if (user == null)
                    return Unauthorized(ApiResponse<object>.Fail("Unauthorized"));

                var response = _adminService.UpdateCurrentStateUser(user, stateDto.State??false);
                if (!response.Data)
                    return BadRequest(response);
                return Ok(response);
            }
            catch
            {
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
        }

    }
}


