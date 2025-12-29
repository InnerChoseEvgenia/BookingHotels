using BookingHotelAPI.Contracts;
using BookingHotelAPI.DTOs.Hotel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingHotelAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController(IHotelsService hotelsService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetHotelDto>>> GetHotels()
    {
        var hotels = await hotelsService.GetHotelsAsync();
        return Ok(hotels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetHotelDto>> GetHotel(int id)
    {
        var hotel = await hotelsService.GetHotelAsync(id);
        if (hotel == null)
        {
            return NotFound();
        }
        return hotel;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutHotel(int id, UpdateHotelDto hotelDto)
    {
        if (id != hotelDto.Id)
        {
            return BadRequest();
        }
        try
        {
            await hotelsService.UpdateHotelAsync(id, hotelDto);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await hotelsService.HotelExistsAsync(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        
      return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<GetHotelDto>> PostHotel(CreateHotelDto hotelDto)
    {
        var hotel = await hotelsService.CreateHotelAsync(hotelDto);

        return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
       await hotelsService.DeleteHotelAsync(id);
       return NoContent();
    }
}
