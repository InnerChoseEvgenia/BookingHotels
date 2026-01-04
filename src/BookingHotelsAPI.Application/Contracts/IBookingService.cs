using BookingHotelAPI.Application.DTOs.Auth;
using BookingHotelAPI.Common.Constants;
using BookingHotelAPI.Common.Models.Filtering;
using BookingHotelAPI.Common.Models.Paging;

namespace BookingHotelAPI.Application.Contracts;


public interface IBookingService
{
    Task<Result> AdminCancelBookingAsync(int hotelId, int bookingId);
    Task<Result> CancelBookingAsync(int hotelId, int bookingId);
    Task<Result> AdminConfirmBookingAsync(int hotelId, int bookingId);
    Task<Result<GetBookingDto>> CreateBookingAsync(CreateBookingDto dto);
    Task<Result<PagedResult<GetBookingDto>>> GetBookingsForHotelAsync(int hotelId, PaginationParameters paginationParameters, BookingFilterParameters filters);
    Task<Result<GetBookingDto>> UpdateBookingAsync(int hotelId, int bookingId, UpdateBookingDto updateBookingDto);
    Task<Result<PagedResult<GetBookingDto>>> GetUserBookingsForHotelAsync(int hotelId, PaginationParameters paginationParameters, BookingFilterParameters filters);
}
