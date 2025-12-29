using BookingHotelAPI.Contracts;
using BookingHotelAPI.Data;
using BookingHotelAPI.DTOs.Hotel;
using Microsoft.EntityFrameworkCore;

namespace BookingHotelAPI.Services;

public class HotelsService(HotelBookingDbContext context) : IHotelsService
{
    public async Task<IEnumerable<GetHotelDto>> GetHotelsAsync()
    {
        return await context.Hotels
            .Include(q => q.Country)
            .Select(h => new GetHotelDto(h.Id, h.Name, h.Address, h.Rating, h.CountryId, h.Country!.Name))
            .ToListAsync();
    }

    public async Task<GetHotelDto> GetHotelAsync(int id)
    {
        var hotel = await context.Hotels
            //.Include(q=>q.Country)
            .Where(h => h.Id == id)
            .Select(h => new GetHotelDto(
                h.Id,
                h.Name,
                h.Address,
                h.Rating,
                h.CountryId,
                h.Country!.Name))
            .FirstOrDefaultAsync();

        return hotel ?? null;
    }

    public async Task UpdateHotelAsync(int id, UpdateHotelDto hotelDto)
    {
        var hotel = await context.Hotels.FindAsync(id);

        hotel.Name = hotelDto.Name;
        hotel.Address = hotelDto.Address;
        hotel.Rating = hotelDto.Rating;
        hotel.CountryId = hotelDto.CountryId;
        context.Hotels.Update(hotel);
        await context.SaveChangesAsync();
    }

    public async Task<GetHotelDto> CreateHotelAsync(CreateHotelDto hotelDto)
    {
        var hotel = new Hotel
        {
            Name = hotelDto.Name,
            Address = hotelDto.Address,
            Rating = hotelDto.Rating,
            CountryId = hotelDto.CountryId
        };

        context.Hotels.Add(hotel);
        await context.SaveChangesAsync();

        return new GetHotelDto(
            hotel.Id,
            hotel.Name,
            hotel.Address, 
            hotel.Rating,
            hotel.CountryId,
            string.Empty
           );
    }

    public async Task DeleteHotelAsync(int id)
    {
        var hotel = await context.Hotels.FindAsync(id);
        context.Hotels.Remove(hotel);
        await context.SaveChangesAsync();

        //var hotel = await context.Hotels
        //    .Where(q=>q.Id== id)
        //    .ExecuteDeleteAsync(); второй вариант удаления
    }
    public async Task<bool> HotelExistsAsync(int id)
    {
        return await context.Hotels.AnyAsync(e => e.Id == id);
    }
}
