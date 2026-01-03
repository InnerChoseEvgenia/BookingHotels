using BookingHotelAPI.Common.Constants;
using BookingHotelAPI.Contracts;
using BookingHotelAPI.Data;
using BookingHotelAPI.Handlers;
using BookingHotelAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the IoC container.
var connectionString = builder.Configuration
    .GetConnectionString("HotelBookingDbConnectionString");
builder.Services.AddDbContextPool<HotelBookingDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
       .AddRoles<IdentityRole>()
       .AddEntityFrameworkStores<HotelBookingDbContext>();
builder.Services.AddHttpContextAccessor();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
     .AddJwtBearer(options =>
     {
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
             ValidAudience = builder.Configuration["JwtSettings:Audience"],
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
             ClockSkew = TimeSpan.Zero // Default is 5 minutes
         };
     })
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(AuthenticationDefaults.ApiKeyScheme, _ => { })
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(AuthenticationDefaults.BasicScheme, _ => { });
builder.Services.AddAuthorization();

builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IHotelsService, HotelsService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IApiKeyValidatorService, ApiKeyValidatorService>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());


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
