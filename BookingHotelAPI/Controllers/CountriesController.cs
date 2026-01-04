using BookingHotelAPI.Application.Contracts;
using BookingHotelAPI.Application.DTOs.Auth;
using BookingHotelAPI.Common.Constants;
using BookingHotelAPI.Common.Models.Filtering;
using BookingHotelAPI.Common.Models.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace BookingHotelAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class CountriesController(ICountriesService countriesService) : BaseApiController
{
    // GET: api/Countries
    [HttpGet]
    [OutputCache(PolicyName = CacheConstants.AuthenticatedUserCachingPolicy)]
    public async Task<ActionResult<IEnumerable<GetCountriesDto>>> GetCountries([FromQuery] CountryFilterParameters filters)
    {
        var result = await countriesService.GetCountriesAsync(filters);
        return ToActionResult(result);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
    {
        var result = await countriesService.GetCountryAsync(id);
        return ToActionResult(result);
    }

    // GET: api/Countries/{id}/hotels
    [HttpGet("{countryId:int}/hotels")]
    public async Task<ActionResult<GetCountryHotelsDto>> GetCountryHotels(
        [FromRoute] int countryId,
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] CountryFilterParameters filters)
    {
        var result = await countriesService.GetCountryHotelsAsync(countryId, paginationParameters, filters);
        return ToActionResult(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = RoleNames.Administrator)]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateDto)
    {
        var result = await countriesService.UpdateCountryAsync(id, updateDto);
        return ToActionResult(result);
    }

    // PATCH: api/Countries/5
    [HttpPatch("{id}")]
    [Authorize(Roles = RoleNames.Administrator)]
    public async Task<IActionResult> PatchCountry(int id, [FromBody] JsonPatchDocument<UpdateCountryDto> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest("Patch document is required.");
        }

        var result = await countriesService.PatchCountryAsync(id, patchDoc);
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
