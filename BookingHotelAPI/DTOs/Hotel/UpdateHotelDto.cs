using System.ComponentModel.DataAnnotations;

namespace BookingHotelAPI.DTOs.Hotel;

public class UpdateHotelDto : CreateHotelDto
{
    [Required]
    public int Id { get; set; }
}
