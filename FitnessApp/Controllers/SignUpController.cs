using FitnessApp.Bl;
using FitnessApp.DAL;
using FitnessApp.Domain.Interfaces;
using FitnessApp.DTOs;
using FitnessApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FitnessApp.apiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly ISignUpService _signUpService;

        public SignUpController(ISignUpService signUpService)
        {
            _signUpService = signUpService;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] UserModel user,CancellationToken ct=default)
        {
           
            try
            {
                var response = await _signUpService.SignUpAsync(user, ct);
                if (response.Data == null)
                    return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error occurred. Please try again later."));
            }
        }
       
    }
}
