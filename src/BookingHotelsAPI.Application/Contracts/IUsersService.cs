using BookingHotelAPI.Application.DTOs.Auth;
using BookingHotelAPI.Common.Constants;

namespace BookingHotelAPI.Application.Contracts;

public interface IUsersService
{
    string UserId { get; }
    Task<Result<string>> LoginAsync(LoginUserDto dto);
    Task<Result<RegisteredUserDto>> RegisterAsync(RegisterUserDto registerUserDto);
}
