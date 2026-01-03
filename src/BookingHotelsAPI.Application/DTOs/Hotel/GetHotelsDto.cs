namespace BookingHotelAPI.Application.DTOs.Auth;

public record GetHotelsDto(
    int Id,
    string Name,
    string Address,
    double Rating,
    int CountryId
    //string CountryName
    );



