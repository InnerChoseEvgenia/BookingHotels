using System.ComponentModel.DataAnnotations;

namespace BookingHotelAPI.DTOs.Booking;

public record UpdateBookingDto(
    DateOnly CheckIn,
    DateOnly CheckOut,
    [Required][Range(minimum: 1, maximum: 10)] 
    int Guests
);
