using System.ComponentModel.DataAnnotations;

namespace BookingHotelAPI.Application.DTOs.Auth;

public class UpdateHotelDto : CreateHotelDto
{
    [Required]
    public int Id { get; set; }
}
