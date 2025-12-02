using FitnessApp.Models;
using System;
using System.Collections.Generic;

namespace FitnessApp.Domain.Entities
{
    public partial class TbMeal
    {
        public int MealId { get; set; }
        public string Name { get; set; } = null!;
        public float Calories { get; set; }
        public decimal? Protein { get; set; }
        public decimal? Carbs { get; set; }
        public decimal? Fats { get; set; }
        public float Price { get; set; }
        public string MealType { get; set; }
        public List<string> RecipeIngredientParts { get; set; } 
        public List<string> RecipeIngredientQuantities { get; set; }=new List<string>();
        
        public virtual ICollection<TbDietPlanMeal> TbDietPlanMeals { get; set; } = new List<TbDietPlanMeal>();
    }
}
