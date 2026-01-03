namespace BookingHotelAPI.Application.Contracts;

public interface IApiKeyValidatorService
{
    Task<bool> IsValidAsync(string apiKey, CancellationToken ct = default);
}
