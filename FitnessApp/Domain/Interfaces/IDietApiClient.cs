using FitnessApp.DTOs;

namespace FitnessApp.Domain.Interfaces
{
    public interface IDietApiClient
    {
        public Task<HttpResponseMessage> GenerateDietAsync(DietRequestModel model, CancellationToken ct);
    }
}
