using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FitnessApp.Domain.Entities
{
    public class ApplicationUsers : IdentityUser
    {
        public string? Name { get; set; }
        public int? UserotpCode { get; set; }
        public DateTime? UserotpCodeExpiry { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        public float Weight { get; set; }
        public float Height { get; set; }
        public string Goal { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Image { get; set; } = "Default.jpg";
        public int Budget { get; set; } = 1000;
        public string ActivityIntensity { get; set; } = null!;
        public bool CurrentState { get; set; } = true;
        public bool VerifyOtp { get; set; }=false;
        
        public virtual ICollection<TbDietPlan> TbDietPlans { get; set; } = new List<TbDietPlan>();
    }
}
