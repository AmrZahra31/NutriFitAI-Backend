using FitnessApp.DTOs;
using FitnessApp.Models;
using Microsoft.AspNetCore.Identity;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Interfaces;

namespace FitnessApp.Bl
{
    

    public class LoginService :ILoginService
    {
        SignInManager<ApplicationUsers> _signInManager;
        UserManager<ApplicationUsers> _userManager;
        ILogger<LoginService> _logger;
        IConfiguration _configuration;
        IUserService _userService;
        public LoginService(SignInManager<ApplicationUsers> signInManager, UserManager<ApplicationUsers> userManager,
                ILogger<LoginService>logger,IConfiguration configuration, IUserService userService)
       {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
            _userService = userService;
       }
        
        public async Task<ApiResponse<bool>> AuthorizedAsync(UserLoginModel user)
       {
            if (user.Email != null)
            {
                var user1 = await _userManager.FindByEmailAsync(user.Email);
                if (user1 != null && await _userService.GetCurentStateAsync())
                {
                    try
                    {
                        if (user.PasswordHash != null)
                        {
                            var res = await _signInManager.PasswordSignInAsync(user1, user.PasswordHash, true, true);
                            if (res.Succeeded)
                                return ApiResponse<bool>.Ok(true,await TokensService.GenerateTokenAsync(user1, _userManager, _configuration));
                            return ApiResponse<bool>.Fail("the email or password is wrong");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex.Message);
                        return ApiResponse<bool>.Fail("Unexpected error, please try later"); ;
                    }

                }
                return ApiResponse<bool>.Fail("Unauthorized");
            }

            return ApiResponse<bool>.Fail("please, enter your email");
        }
    }
}
