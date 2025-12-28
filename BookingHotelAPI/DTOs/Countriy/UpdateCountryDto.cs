using System.ComponentModel.DataAnnotations;

namespace BookingHotelAPI.DTOs.Countriy;

public class UpdateCountryDto : CreateCountryDto
{
    [Required]
    public int CountryId { get; set; }
}
