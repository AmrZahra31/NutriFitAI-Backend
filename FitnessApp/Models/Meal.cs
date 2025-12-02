namespace FitnessApp.Models
{
    public class Meal
    {
        public string Name { get; set; }
        public string MealType { get; set; }
        public float Calories { get; set; }
        public float EstimatedPriceEGP { get; set; }
        public string RecipeIngredientParts { get; set; }
        public string RecipeIngredientQuantities { get; set; }
    }
}
