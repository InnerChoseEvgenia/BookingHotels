using BookingHotelAPI.Application.DTOs.Auth;
using BookingHotelAPI.Common.Constants;
using BookingHotelAPI.Common.Models.Filtering;
using BookingHotelAPI.Common.Models.Paging;

namespace BookingHotelAPI.Application.Contracts;

public interface IHotelsService
{
    Task<Result<GetHotelDto>> CreateHotelAsync(CreateHotelDto hotelDto);
    Task<Result> DeleteHotelAsync(int id);
    Task<Result<GetHotelDto>> GetHotelAsync(int id);
    Task<Result<PagedResult<GetHotelDto>>> GetHotelsAsync(PaginationParameters paginationParameters, HotelFilterParameters filters);
    Task<bool> HotelExistsAsync(int id);
    Task<bool> HotelExistsAsync(string name, int countryId);
    Task<Result> UpdateHotelAsync(int id, UpdateHotelDto updateDto);
}
