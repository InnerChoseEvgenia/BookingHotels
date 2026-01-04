using BookingHotelAPI.Application.DTOs.Auth;
using BookingHotelAPI.Common.Constants;
using BookingHotelAPI.Common.Models.Filtering;
using BookingHotelAPI.Common.Models.Paging;

namespace BookingHotelAPI.Application.Contracts;
public interface ICountriesService
{
    Task<bool> CountryExistsAsync(int id);
    Task<bool> CountryExistsAsync(string name);
    Task<Result<GetCountryDto>> CreateCountryAsync(CreateCountryDto createDto);
    Task<Result> DeleteCountryAsync(int id);
    Task<Result<IEnumerable<GetCountriesDto>>> GetCountriesAsync(CountryFilterParameters? filters);
    Task<Result<GetCountryDto?>> GetCountryAsync(int id);
    Task<Result> UpdateCountryAsync(int id, UpdateCountryDto updateDto);
    Task<Result<GetCountryHotelsDto>> GetCountryHotelsAsync(int countryId, PaginationParameters paginationParameters, CountryFilterParameters filters);
}