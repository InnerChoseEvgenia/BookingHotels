using BookingHotelAPI.DTOs.Hotel;

namespace BookingHotelAPI.Contracts
{
    public interface IHotelsService
    {
        Task<GetHotelDto> CreateHotelAsync(CreateHotelDto hotelDto);
        Task DeleteHotelAsync(int id);
        Task<GetHotelDto> GetHotelAsync(int id);
        Task<IEnumerable<GetHotelDto>> GetHotelsAsync();
        Task<bool> HotelExistsAsync(int id);
        Task UpdateHotelAsync(int id, UpdateHotelDto hotelDto);
    }
}