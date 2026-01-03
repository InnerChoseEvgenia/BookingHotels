using System.ComponentModel.DataAnnotations;

namespace BookingHotelAPI.DTOs.Booking;

public record CreateBookingDto(
    [Required] int HotelId,
    DateOnly CheckIn,
    DateOnly CheckOut,
    [Required][Range(minimum: 1, maximum: 10)] int Guests); 
