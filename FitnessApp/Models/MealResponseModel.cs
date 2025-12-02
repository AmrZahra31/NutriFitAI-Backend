namespace FitnessApp.Models
{
    public class MealResponseModel
    {
        public float Daily_Calories { get; set; }
        public float Per_Meal_Target { get; set; }
        public SuggestedRecipes Suggested_Recipes { get; set; }
    }
}
