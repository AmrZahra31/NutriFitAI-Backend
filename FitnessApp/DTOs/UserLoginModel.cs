using System.ComponentModel.DataAnnotations;

namespace FitnessApp.DTOs
{
    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
    }
}
