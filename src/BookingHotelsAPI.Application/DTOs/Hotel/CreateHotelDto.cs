using System.ComponentModel.DataAnnotations;

namespace BookingHotelAPI.Application.DTOs.Auth;

public class CreateHotelDto
{
    [Required]
    public required string Name { get; set; }

    [MaxLength(150)]
    public required string Address { get; set; }

    [Range(1, 5)]
    public double Rating { get; set; }

    [Required]
    public int CountryId { get; set; }
}
