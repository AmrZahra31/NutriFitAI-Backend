using System.Text.Json;
namespace FitnessApp.Bl
{


    public static class StringParsingHelper
    {
        public static List<string> ParseStringList(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<string>();


            string jsonList = input.Replace("'", "\"");

            try
            {
                return JsonSerializer.Deserialize<List<string>>(jsonList) ?? new List<string>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }

}
