namespace FitnessApp.DTOs
{
    public class PasswordModel
    {
        public string? CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string? Email { get; set; }
    }
}
