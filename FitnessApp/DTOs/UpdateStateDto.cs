using System.ComponentModel.DataAnnotations;

namespace FitnessApp.DTOs
{
    public class UpdateStateDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public bool? State { get; set; }
    }

}
