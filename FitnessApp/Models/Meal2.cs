namespace FitnessApp.Models
{
    public class Meal2
    {
        public string Name { get; set; }
        public string MealType { get; set; }
        public float Calories { get; set; }
        public float EstimatedPriceEGP { get; set; }
        public List<string> RecipeIngredientParts { get; set; }
        public List<string> RecipeIngredientQuantities { get; set; }
        public List<string> combinedIngredients { get; set; }
    }
}
