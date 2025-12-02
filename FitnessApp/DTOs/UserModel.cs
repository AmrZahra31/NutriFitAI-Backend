using System.ComponentModel.DataAnnotations;

namespace FitnessApp.DTOs
{
    public class UserModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        public int? UserotpCode { get; set; }

        public DateTime? UserotpCodeExpiry { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Gender { get; set; } = null!;
        [Required]
        public float? Weight { get; set; } 
        [Required]
        public float? Height { get; set; } 
        [Required]
        public string Goal { get; set; }
        [Required]
        public string ActivityIntensity { get; set; } = null!;

        public string PreferredUnits { get; set; }
    }
}
