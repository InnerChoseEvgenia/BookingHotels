using AutoMapper;
using BookingHotelAPI.Data;
using BookingHotelAPI.DTOs.Countriy;
using BookingHotelAPI.DTOs.Hotel;

namespace BookingHotelAPI.MappingProfiles;


public class HotelMappingProfile : Profile
{
    public HotelMappingProfile()
    {
        CreateMap<Hotel, GetHotelDto>()
              .ForMember(d => d.Country, cfg => cfg.MapFrom<CountryNameResolver>());
        //.ForMember(d => d.Country, cfg => cfg.MapFrom(s=>s.Country.Name ?? string.Empty));// чтобы был мапинг из источника (s)
        // в пункт назначения (d), где типы этих полей
        // отличаются (string,и объкт типа Country)
        CreateMap<Hotel, GetHotelSlimDto>();// Added for Country->GetCountryDto nested projection
        CreateMap<CreateHotelDto, Hotel>();
    }
}

public class CountryMappingProfile : Profile
{
    public CountryMappingProfile()
    {
         CreateMap<Country, GetCountryDto>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.CountryId));
        CreateMap<Country, GetCountriesDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.CountryId));
        CreateMap<CreateCountryDto, Country>();
    }
}

public class CountryNameResolver : IValueResolver<Hotel, GetHotelDto, string>
{
    //для разрешения конфликта между полями моделей
    public string Resolve(Hotel source, GetHotelDto destination, string destMember, ResolutionContext context)
    {
        return source.Country?.Name ?? string.Empty;
    }
}
