using BookingHotelAPI.Application.Contracts;
using BookingHotelAPI.Application.DTOs.Auth;
using BookingHotelAPI.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotelAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CountriesController(ICountriesService countriesService) : BaseApiController
{
    // GET: api/Countries
    [HttpGet]
   
    public async Task<ActionResult<IEnumerable<GetCountriesDto>>> GetCountries()
    {
        var result = await countriesService.GetCountriesAsync();
        return ToActionResult(result);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
    {
        var result = await countriesService.GetCountryAsync(id);
        return ToActionResult(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = RoleNames.Administrator)]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateDto)
    {
        var result = await countriesService.UpdateCountryAsync(id, updateDto);
        return ToActionResult(result);
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Administrator)]
    public async Task<ActionResult<GetCountryDto>> PostCountry(CreateCountryDto createDto)
    {
        var result = await countriesService.CreateCountryAsync(createDto);
        if (!result.IsSuccess) return MapErrorsToResponse(result.Errors);
        return CreatedAtAction(nameof(GetCountry), new { id = result.Value!.Id }, result.Value);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    [Authorize(Roles = RoleNames.Administrator)]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var result = await countriesService.DeleteCountryAsync(id);
        return ToActionResult(result);
    }
}
