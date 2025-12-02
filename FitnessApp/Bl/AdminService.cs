using FitnessApp.DAL.Repositories;
using FitnessApp.Models;
using Microsoft.AspNetCore.Identity;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Interfaces;

namespace FitnessApp.Bl
{


    public class AdminService :IAdminService
    {
        ILogger<ApplicationUsers> _logger;
        UserManager<ApplicationUsers> _userManager;
        IUserRepository _userRepo;

        public AdminService(ILogger<ApplicationUsers> logger,UserManager<ApplicationUsers> userManager, IUserRepository userRepo)
        {
            _logger = logger;
            _userManager = userManager;
            _userRepo = userRepo;
        }

        public async Task<ApiResponse<bool>> AddAdminAsync(ApplicationUsers user)
        {
            try
            {
                if (user == null)
                    return ApiResponse<bool>.Fail("Unauthorized");

                if (await _userManager.IsInRoleAsync(user, "Customer"))
                    await _userManager.RemoveFromRoleAsync(user, "Customer");

                if (!await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    return ApiResponse<bool>.Ok(true, $"the {user.Name} become admin now.");
                }
                return ApiResponse<bool>.Fail($"the {user.Name} is already admin now.");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<bool>> RemoveAdminAsync(ApplicationUsers user)
        {
            try
            {
                if (user == null)
                    return ApiResponse<bool>.Fail("Unauthorized");
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "Admin");
                }
                else
                    return ApiResponse<bool>.Fail($"the {user.Name} was not an admin before.");


                await _userManager.AddToRoleAsync(user, "Customer");
                return ApiResponse<bool>.Ok(true, $"the {user.Name} become Customer now.");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public ApiResponse<bool> UpdateCurrentStateUser(ApplicationUsers user,bool state)
        {
            try
            {
                if (user == null)
                    return ApiResponse<bool>.Fail("Unauthorized");
                if (user.CurrentState == state) 
                    return ApiResponse<bool>.Fail("Current state is same the state you want to changed");

                if (_userRepo.UpdateCurentState(user.Id, state))
                    return ApiResponse<bool>.Ok(true, "Current state is updated successfully");
                return ApiResponse<bool>.Fail("Can not update for current state");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }
    }
}
