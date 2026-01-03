using AutoMapper;
using BookingHotelAPI.Application.DTOs.Auth;
using BookingHotelAPI.Domain.Entities;

namespace BookingHotelAPI.Application.MappingProfiles;


public class HotelMappingProfile : Profile
{
    public HotelMappingProfile()
    {
        CreateMap<Hotel, GetHotelDto>()
              .ForMember(d => d.CountryName, cfg => cfg.MapFrom<CountryNameResolver>());
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
public sealed class BookingMappingProfile : Profile
{
    public BookingMappingProfile()
    {
        CreateMap<Booking, GetBookingDto>()
            .ForMember(d => d.HotelName, o => o.MapFrom(s => s.Hotel!.Name))//можно было бы использовать CountryNameResolver, это второй вариант
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<CreateBookingDto, Booking>()
            .ForMember(d => d.Id, o => o.Ignore())// Ignore используется для повышения производительности, чтобы EF не просматривал все поля
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.TotalPrice, o => o.Ignore())
            .ForMember(d => d.Status, o => o.Ignore())
            .ForMember(d => d.CreatedAtUtc, o => o.Ignore())
            .ForMember(d => d.UpdatedAtUtc, o => o.Ignore())
            .ForMember(d => d.Hotel, o => o.Ignore());

        CreateMap<UpdateBookingDto, Booking>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.TotalPrice, o => o.Ignore())
            .ForMember(d => d.Status, o => o.Ignore())
            .ForMember(d => d.CreatedAtUtc, o => o.Ignore())
            .ForMember(d => d.UpdatedAtUtc, o => o.Ignore())
            .ForMember(d => d.Hotel, o => o.Ignore());
    }
}
