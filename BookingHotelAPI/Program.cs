using BookingHotelAPI.Constants;
using BookingHotelAPI.Contracts;
using BookingHotelAPI.Data;
using BookingHotelAPI.Handlers;
using BookingHotelAPI.MappingProfiles;
using BookingHotelAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the IoC container.
var connectionString = builder.Configuration
    .GetConnectionString("HotelBookingDbConnectionString");
builder.Services.AddDbContextPool<HotelBookingDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
       //.AddRoles<IdentityRole>()
       .AddEntityFrameworkStores<HotelBookingDbContext>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = AuthenticationDefaults.BasicScheme;
    options.DefaultChallengeScheme = AuthenticationDefaults.BasicScheme;
})
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(AuthenticationDefaults.BasicScheme, _ =>
    { });
builder.Services.AddAuthorization();

builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IHotelsService, HotelsService>();
builder.Services.AddScoped<IUsersService, UsersService>();


builder.Services.AddAutoMapper(cfg =>{
    cfg.AddProfile<HotelMappingProfile>();
    cfg.AddProfile<CountryMappingProfile>();
});


builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapGroup("api/defaultauth").MapIdentityApi<ApplicationUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
