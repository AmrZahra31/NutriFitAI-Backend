using System.Text.Json.Serialization;

namespace FitnessApp.DTOs
{
    public class DietRequestModel
    {
        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("weight")]
        public int Weight { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("activity_level")]
        public string ActivityLevel { get; set; }

        [JsonPropertyName("goal")]
        public string Goal { get; set; }

        [JsonPropertyName("daily_budget")]
        public int Daily_Budget { get; set; }

        [JsonPropertyName("dietary_restrictions")]
        public List<string> Dietary_Restrictions { get; set; }
    }
}
