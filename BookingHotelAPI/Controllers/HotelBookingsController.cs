using BookingHotelAPI.Application.Contracts;
using BookingHotelAPI.Application.DTOs.Auth;
using BookingHotelAPI.AuthorizationFilters;
using BookingHotelAPI.Common.Models.Filtering;
using BookingHotelAPI.Common.Models.Paging;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotelAPI.Controllers;

[Route("api/hotels/{hotelId:int}/bookings")]
[ApiController]
public class HotelBookingsController(IBookingService bookingService) : BaseApiController
{
    [HttpGet("admin")]
    [HotelOrSystemAdminAttribute]
    public async Task<ActionResult<PagedResult<GetBookingDto>>> GetBookingsAdmin(
        [FromRoute] int hotelId,
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] BookingFilterParameters filters)
    {
        var result = await bookingService.GetUserBookingsForHotelAsync(hotelId, paginationParameters,filters);
        return ToActionResult(result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<GetBookingDto>>> GetBookings(
        [FromRoute] int hotelId,
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] BookingFilterParameters filters)
    {
        var result = await bookingService.GetUserBookingsForHotelAsync(hotelId, paginationParameters, filters);
        return ToActionResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<GetBookingDto>> CreateBooking([FromRoute] int hotelId, [FromBody] CreateBookingDto createBookingDto)
    {
        var result = await bookingService.CreateBookingAsync(createBookingDto);
        return ToActionResult(result);
    }

    [HttpPut("{bookingId:int}")]
    public async Task<ActionResult<GetBookingDto>> UpdateBooking(
   [FromRoute] int hotelId,
   [FromRoute] int bookingId,
   [FromBody] UpdateBookingDto updateBookingDto)
    {
        var result = await bookingService.UpdateBookingAsync(hotelId, bookingId, updateBookingDto);
        return ToActionResult(result);
    }

    [HttpPut("{bookingId:int}/cancel")]
    public async Task<IActionResult> CancelBooking([FromRoute] int hotelId, [FromRoute] int bookingId)
    {
        var result = await bookingService.CancelBookingAsync(hotelId, bookingId);
        return ToActionResult(result);
    }

    [HttpPut("{bookingId:int}/admin/cancel")]
    [HotelOrSystemAdminAttribute]
    public async Task<IActionResult> AdminCancelBooking([FromRoute] int hotelId, [FromRoute] int bookingId)
    {
        var result = await bookingService.AdminCancelBookingAsync(hotelId, bookingId);
        return ToActionResult(result);
    }

    [HttpPut("{bookingId:int}/admin/confirm")]
    [HotelOrSystemAdminAttribute]
    public async Task<IActionResult> AdminConfirmBooking([FromRoute] int hotelId, [FromRoute] int bookingId)
    {
        var result = await bookingService.AdminConfirmBookingAsync(hotelId, bookingId);
        return ToActionResult(result);
    }
}
