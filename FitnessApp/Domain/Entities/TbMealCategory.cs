using FitnessApp.Models;
using System;
using System.Collections.Generic;

namespace FitnessApp.Domain.Entities;

public partial class TbMealCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<TbDietPlanMeal> TbDietPlanMeals { get; set; } = new List<TbDietPlanMeal>();

   

}
