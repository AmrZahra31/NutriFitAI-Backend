using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Domain.Entities
{
    public partial class TbDietPlan
    {
        [Key]
        public int PlanId { get; set; }

        [Required]
        public string UserId { get; set; }  // FK

        [ForeignKey("UserId")]
        public virtual ApplicationUsers TbUser { get; set; }

        public string? PlanName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool CurrentState { get; set; } = false;
        public float TotalCalories { get; set; } = 1500;
        public float Budget { get; set; } = 1500;

       
        public virtual ICollection<TbDietPlanMeal> TbDietPlanMeals { get; set; } = new List<TbDietPlanMeal>();
    }
}
