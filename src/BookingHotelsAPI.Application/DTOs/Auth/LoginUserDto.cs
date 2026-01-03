using System.ComponentModel.DataAnnotations;

namespace BookingHotelAPI.Application.DTOs.Auth;

public class LoginUserDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
