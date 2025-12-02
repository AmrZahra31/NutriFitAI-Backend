namespace FitnessApp.DTOs
{
    public class VerifyOtpRequest
    {
        public string Email { get; set; }
        public int otpCode { get; set; }
    }
}
