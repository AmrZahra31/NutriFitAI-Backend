using FitnessApp.Domain.Interfaces;
using FitnessApp.Models;
using Microsoft.IdentityModel.Tokens;


namespace FitnessApp.DAL.Repositories
{

    public class MacrosApiClient : IMacrosApiClient
    {
        private readonly HttpClient _client;
        private readonly string? _apiKey;
        private readonly ILogger<MacrosApiClient> _logger;

        public MacrosApiClient(ILogger<MacrosApiClient> logger,IConfiguration config,HttpClient client)
        {
            _client = client;
            _apiKey = config["MacrosApi:ApiKey"];
            _logger = logger;
        }
        public async Task<ApiResponse<object>> GetMacrosAsync(string query, CancellationToken ct = default)
        {
           
                string request =_client.BaseAddress +Uri.EscapeDataString(query);
            _client.DefaultRequestHeaders.Clear();
            if (!_apiKey.IsNullOrEmpty())
                    _client.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);

                HttpResponseMessage response;
            try
            {
                response = await _client.GetAsync(request, ct);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync(ct);
                    
                    return ApiResponse<object>.Ok(result, "Macros is sended successfully");
                }

                return ApiResponse<object>.Fail($"status code is{response.StatusCode}", await response.Content.ReadAsStringAsync());


            }
            catch(TaskCanceledException ex)
            {
                _logger.LogWarning("Macros Api is canceled or timed out");
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogWarning("unexpected server error");
                throw;
            }

            
        }
    }
}
