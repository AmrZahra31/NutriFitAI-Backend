using FitnessApp.Models;

namespace FitnessApp.Domain.Interfaces
{
    public interface IMacrosApiClient
    {
        Task<ApiResponse<object>> GetMacrosAsync(string query, CancellationToken ct = default);
    }
}
