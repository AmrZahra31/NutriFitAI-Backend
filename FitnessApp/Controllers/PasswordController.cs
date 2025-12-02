using FitnessApp.Bl;
using FitnessApp.DAL;
using FitnessApp.Domain.Interfaces;
using FitnessApp.DTOs;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FitnessApp.apiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IPasswordService _passwordService;

        public PasswordController(IUserService user, IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordModel passwords)
        {
            try
            {

               var passwordResponse= await _passwordService.ChangePasswordAsync(passwords);
                if (!passwordResponse.Data)
                    return BadRequest(passwordResponse);
                return Ok(passwordResponse);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
            
        }


        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest model)
        {
            try
            {
                var verifiedResponse = await _passwordService.VerifyOtpAsync(model);
                if (!verifiedResponse.Data)
                    return BadRequest(verifiedResponse);
                return Ok(verifiedResponse);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordModel resetPassword)
        {
            try
            {

                var passwordResponse = await _passwordService.ResetPasswordAsync(resetPassword);
                if (passwordResponse.Data == null) 
                    return BadRequest(passwordResponse);
                return Ok(passwordResponse);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
        }

        [HttpPost("ResendCode")]
        public async Task<IActionResult> ResendCode([FromBody] string email)
        {
            try
            {
                var respnse = await _passwordService.ResendCodeAsync(email);
                if (!respnse.Data)
                    return BadRequest(respnse);
                return Ok(respnse);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ApiResponse<object>.Fail("An unexpected error, please try later"));
            }
        }
    


       
    }
}
