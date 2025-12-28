using BookingHotelAPI.DTOs.Hotel;

namespace BookingHotelAPI.DTOs.Countriy;

//public class GetCountryDto
//{
//    public int Id { get; set; }
//    public string Name { get; set; } = string.Empty;
//    public string ShortName { get; set; } = string.Empty;
//    //public List<GetHotelSlimDto> Hotels { get; set; } = new();
//}

public class GetCountryHotelsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    //public PagedResult<GetHotelSlimDto> Hotels { get; set; } = new();
}

//public class GetCountriesDto
//{
//    public int CountryId { get; set; }
//    public string Name { get; set; } = string.Empty;
//    public string ShortName { get; set; } = string.Empty;
//}

public record GetCountriesDto(
    int CountryId,
    string Name,
    string ShortName
    );

public record GetCountryDto( 
    int Id,
    string Name,
    string ShortName,
    List<GetHotelSlimDto> Hotels
    );

