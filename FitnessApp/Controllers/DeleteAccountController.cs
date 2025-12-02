using FitnessApp.Bl;
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
    public class DeleteAccountController : ControllerBase
    {
        public DeleteAccountController(IUserService user)
        {
            _userService = user;
        }
        
        IUserService _userService;


        
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                var user=await _userService.GetCurrentUserAsync();
                if (user == null)
                    return Unauthorized(ApiResponse<object>.Fail("Unauthorized"));
                _userService.UpdateCurentState(user.Id, false);
                return Ok(ApiResponse<bool>.Ok(true, $"{user.Name} is deleted") );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
        }

        
    }
}
