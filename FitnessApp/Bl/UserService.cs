using FitnessApp.DAL.Repositories;
using FitnessApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using FitnessApp.Domain.Entities;
using FitnessApp.Domain.Interfaces;
namespace FitnessApp.Bl
{
   

    public class UserService : IUserService
    {
        public UserService(IHttpContextAccessor contextAccessor, UserManager<ApplicationUsers> userManager,
                ILogger<UserService> logger,IUserRepository user)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _logger = logger;
            _user = user;
        }
        IHttpContextAccessor _contextAccessor;
        UserManager<ApplicationUsers> _userManager;
        ILogger<UserService> _logger;
        IUserRepository _user;

        public string? GetEmail()
        {
            var user = _contextAccessor.HttpContext?.User;
            return user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        }
        public async Task<ApplicationUsers?> GetCurrentUserAsync()
        {
            var email = GetEmail();
            if (email == null)
                return null;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.CurrentState)
                return null;
            return user;
        }

        public bool UpdateCurentState(string userId, bool CurrentState)
        {
            try
            {
                return _user.UpdateCurentState(userId, CurrentState);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public async Task<bool> GetCurentStateAsync()
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                    return false;
                return user.CurrentState;
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public bool UpdateUser(ApplicationUsers user)
        {
            try
            {
                if (user == null)
                    return false;
                _user.updateUser(user);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public ApplicationUsers GetUserById(string userId)
        {
            try
            {
                var user = _user.GetUserById(userId);              
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }

        public async Task<ApplicationUsers> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
               
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }
    }
}
