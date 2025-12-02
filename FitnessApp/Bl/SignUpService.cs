using FitnessApp.DAL.context;
using FitnessApp.DAL.Repositories;
using FitnessApp.DTOs;
using FitnessApp.Models;
using Microsoft.AspNetCore.Identity;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Interfaces;
namespace FitnessApp.Bl
{
    

    public class SignUpService :ISignUpService
    {
        UserManager<ApplicationUsers> _userManager;
        FitnessAppContext _context;
        IConfiguration _configuration;
        IUserSettingeRepository _userSetting;
        SignInManager<ApplicationUsers> _signInManager;
        EmailSender _emailSender;
        ILogger<SignUpService> _logger;

        public SignUpService(UserManager<ApplicationUsers>userManager,FitnessAppContext ctx,
                IConfiguration configuration,IUserSettingeRepository userSetting, SignInManager<ApplicationUsers> signInManager,
                    EmailSender emailSender,ILogger<SignUpService>logger)
        {
            _userManager = userManager;
            _context = ctx;
            _configuration = configuration;
            _userSetting = userSetting;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<ApiResponse<object>> SignUpAsync(UserModel user, CancellationToken ct = default)
        {
            if (user.Password != user.ConfirmPassword)
                return ApiResponse<object>.Fail("ConfirmPassword must match the password.");

            if (!IsValidGoal(user.Goal))
                return ApiResponse<object>.Fail("Enter a valid goal (health_maintenance, muscle_gain, weight_loss).");


            if (!IsValidActivityIntensity(user.ActivityIntensity))
                return ApiResponse<object>.Fail("Enter a valid Activity Intensity (sedentary, lightly_active, moderately_active, very_active, extra_active).");

            using var transaction = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                if (user.Gender.ToLower() != "male" || user.Gender.ToLower() != "female")
                    return ApiResponse<object>.Fail("please, enter a valid Gender");
                var applicationUser = new ApplicationUsers
                {
                    Name = user.Name,
                    Age = user.Age,
                    Email = user.Email,
                    UserName = user.Name,
                    Gender = user.Gender,
                    Goal = user.Goal,
                    ActivityIntensity = user.ActivityIntensity,
                    Height = user.Height??0,
                    Weight = user.Weight??0
                };


                if (user.PreferredUnits != "kg" && user.PreferredUnits != "lbs")
                    return ApiResponse<object>.Fail("Preferred units must be 'kg' or 'lbs'.");
                if (user.Password == null)
                    return ApiResponse<object>.Fail("please, enter a valid password");
                var result = await _userManager.CreateAsync(applicationUser, user.Password);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync(ct);
                    return ApiResponse<object>.Fail("Failed to create user.", result.Errors);
                }
                var res = await _userManager.AddToRoleAsync(applicationUser, "Customer");
                if (!res.Succeeded)
                {
                    await transaction.RollbackAsync(ct);
                    return ApiResponse<object>.Fail("Failed to add role to this user.", result.Errors);
                }
                _userSetting.Save(new TbUserSetting
                {
                    PreferredUnits = user.PreferredUnits?.ToLower() ?? "kg",
                    UserId = applicationUser.Id
                });

                var signInResult = await _signInManager.PasswordSignInAsync(applicationUser, user.Password, isPersistent: true, lockoutOnFailure: true);
                if (!signInResult.Succeeded)
                {
                    await transaction.RollbackAsync(ct);
                    return ApiResponse<object>.Fail("Invalid login attempt after registration.");
                }

                var token = await TokensService.GenerateTokenAsync(applicationUser, _userManager, _configuration);

                if (!string.IsNullOrEmpty(user.Email))
                {
                    await _emailSender.SendEmailAsync(user.Email, "Welcome to Limitless Fitness App", EmailSender.GetMesasge(applicationUser));
                }
                await transaction.CommitAsync(ct);
                return ApiResponse<object>.Ok(token, "you are register successfully!");
                
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning("Macros Api is canceled or timed out");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("unexpected server error");
                throw;
            }
        }
        private bool IsValidGoal(string goal)
        {
            return goal == "health_maintenance" || goal == "muscle_gain" || goal == "weight_loss";
        }

        private bool IsValidActivityIntensity(string intensity)
        {
            return intensity == "sedentary" || intensity == "lightly_active" ||
                   intensity == "moderately_active" || intensity == "very_active" ||
                   intensity == "extra_active";
        }
    }
}

