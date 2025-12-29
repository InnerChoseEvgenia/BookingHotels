using BookingHotelAPI.DTOs.Countriy;

namespace BookingHotelAPI.Contracts;

public interface ICountriesService
{
    Task<IEnumerable<GetCountriesDto>> GetCountriesAsync();
    Task<GetCountryDto?> GetCountryAsync(int id);
    Task<GetCountryDto> CreateCountryAsync(CreateCountryDto createDto);
    Task UpdateCountryAsync(int id, UpdateCountryDto updateDto);
    Task DeleteCountryAsync(int id);
    Task<bool> CountryExistsAsync(int id);
    Task<bool> CountryExistsAsync(string name);
}