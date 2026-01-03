using System.ComponentModel.DataAnnotations;

namespace BookingHotelAPI.Application.DTOs.Auth;

public class UpdateCountryDto : CreateCountryDto
{
    [Required]
    public int CountryId { get; set; }
}
