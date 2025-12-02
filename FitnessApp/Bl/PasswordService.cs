using FitnessApp.DAL.Repositories;
using FitnessApp.DTOs;
using FitnessApp.Models;
using Microsoft.AspNetCore.Identity;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Interfaces;


namespace FitnessApp.Bl
{
    


    public class PasswordService :IPasswordService
    {
        ILogger<PasswordService> _logger;
        IUserService _userService;
        UserManager<ApplicationUsers> _userManager;
        EmailSender _emailSender;

        public PasswordService(ILogger<PasswordService>logger,IUserService userService,
                UserManager<ApplicationUsers> userManager,EmailSender emailSender)
        {
            _logger = logger;
            _userService = userService;
            _userManager = userManager;
            _emailSender = emailSender;
        }


        public async Task<ApiResponse<bool>> ChangePasswordAsync(PasswordModel passwords)
        {
            try
            {
                //change password if user want to change his password for mor security for his self and he logged in
                var user = await _userManager.FindByEmailAsync(passwords.Email);
                if (user == null || !user.CurrentState)
                    return ApiResponse<bool>.Fail("Unauthorized");

                if (passwords.Email == null)
                {
                    if (passwords.CurrentPassword == null)
                        return ApiResponse<bool>.Fail("please, enter your Current Password");

                    if (passwords.CurrentPassword == passwords.NewPassword)
                        return ApiResponse<bool>.Fail("New password cannot be the same as the current password");

                    var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, passwords.CurrentPassword);
                    if (!isPasswordCorrect)
                        return ApiResponse<bool>.Fail("Current password is incorrect");
                }

                if (passwords.NewPassword != passwords.ConfirmPassword)
                    return ApiResponse<bool>.Fail("New password and confirmation do not match");

                if (string.IsNullOrEmpty(passwords.Email))
                {
                    passwords.Email = _userService.GetEmail();
                    if(passwords.Email==null)
                        return ApiResponse<bool>.Fail("please, enter your email");
                    var result = await _userManager.ChangePasswordAsync(user, passwords.CurrentPassword, passwords.NewPassword);
                    if (result.Succeeded)
                        return ApiResponse<bool>.Ok(true, "Password changed successfully");
                }

                // change password if user is forget his password and he want to login


                var otpCode = new Random().Next(10000, 99999);
                user.UserotpCode = otpCode;
                user.UserotpCodeExpiry = DateTime.UtcNow.AddMinutes(10);
                user.VerifyOtp = false;
                await _userManager.UpdateAsync(user);

                await _emailSender.SendEmailAsync(passwords.Email, "Reset your Password", $"Your password reset code is: <b>{otpCode}</b>");

                return ApiResponse<bool>.Ok(true, "check your email for reset code");


            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<bool>> VerifyOtpAsync(VerifyOtpRequest model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !user.CurrentState)
                    return ApiResponse<bool>.Fail("Unauthorized");

                if (user.UserotpCode == null && user.UserotpCodeExpiry == null)
                    return ApiResponse<bool>.Fail("No active OTP code. Please request a new code.");

                if (user.UserotpCode != model.otpCode && user.UserotpCodeExpiry < DateTime.UtcNow)
                    return ApiResponse<bool>.Fail("Invalid OTP code");

                user.UserotpCode = null;
                user.UserotpCodeExpiry = null;
                user.VerifyOtp = true;
                await _userManager.UpdateAsync(user);

                return ApiResponse<bool>.Ok(true, "OTP code has verified successfully");
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<bool>> ResendCodeAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null || !user.CurrentState)
                    return ApiResponse<bool>.Fail("Email not found");

                var otpCode = new Random().Next(10000, 99999);
                user.UserotpCode = otpCode;
                user.UserotpCodeExpiry = DateTime.UtcNow.AddMinutes(10);
                user.VerifyOtp = false;
                await _userManager.UpdateAsync(user);

                await _emailSender.SendEmailAsync(email, "Reset your Password", $"Your password reset code is: <b>{otpCode}</b>");

                return ApiResponse<bool>.Ok(true, "Reset code resent to your email.");
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }


        }


        public async Task<ApiResponse<bool>> ResetPasswordAsync(PasswordModel resetPassword)
        {
            try
            {
                if (resetPassword.NewPassword != resetPassword.ConfirmPassword)
                    return ApiResponse<bool>.Fail( "New password and confirmation do not match" );

                if (resetPassword.Email == null)
                    return ApiResponse<bool>.Fail("please, enter youe email");

                var user = await _userManager.FindByEmailAsync(resetPassword.Email);
                if (user == null || !user.CurrentState)
                    return ApiResponse<bool>.Fail("Unauthorized");

                if (!user.VerifyOtp)
                    return ApiResponse<bool>.Fail( "the OtpCode is not verification");
                user.VerifyOtp = false;
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            
                var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPassword.NewPassword);
                if (result.Succeeded)
                    return ApiResponse<bool>.Ok(true, "Password reset successfully, please login");

                return ApiResponse<bool>.Fail( result.Errors.FirstOrDefault()?.Description ?? "Password reset failed" );
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

    }
}
