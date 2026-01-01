using BookingHotelAPI.DTOs.Auth;
using BookingHotelAPI.Results;

namespace BookingHotelAPI.Contracts;

public interface IUsersService
{
    Task<Result<string>> LoginAsync(LoginUserDto dto);
    Task<Result<RegisteredUserDto>> RegisterAsync(RegisterUserDto registerUserDto);
}
