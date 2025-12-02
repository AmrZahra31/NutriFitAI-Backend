using FitnessApp.Controllers;
using FitnessApp.Domain.Interfaces;
using FitnessApp.DTOs;

namespace FitnessApp.DAL.Repositories
{
    public class DietApiClient :IDietApiClient
    {
        public DietApiClient(HttpClient httpClient,ILogger<DietApiClient>logger)
        {
            _client = httpClient;
            _logger = logger;
        }
        HttpClient _client;
        ILogger<DietApiClient> _logger;

        public async Task<HttpResponseMessage> GenerateDietAsync(DietRequestModel model,CancellationToken ct=default)
        {
            return await _client.PostAsJsonAsync("personalized_recommend", model, ct);
        }
    }
}
