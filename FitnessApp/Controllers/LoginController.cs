using FitnessApp.Bl;
using FitnessApp.Domain.Interfaces;
using FitnessApp.DTOs;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitnessApp.apiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task< IActionResult> Login([FromBody] UserLoginModel user)
        {
            try
            {
                var result = await _loginService.AuthorizedAsync(user);
                return Ok(result);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
            
        }
       

    }
}

