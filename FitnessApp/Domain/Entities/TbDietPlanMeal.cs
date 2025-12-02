using FitnessApp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Domain.Entities
{
    public partial class TbDietPlanMeal
    {
        [Key]
        public int PlanMealId { get; set; }

        public int PlanId { get; set; } // FK
        [ForeignKey("PlanId")]
        public virtual TbDietPlan TbDietPlan { get; set; }

        public int MealId { get; set; } // FK
        [ForeignKey("MealId")]
        public virtual TbMeal TbMeal { get; set; }

        public int CategoryId { get; set; } 
        [ForeignKey("CategoryId")]
        public virtual TbMealCategory TbCategory { get; set; }
    }
}
