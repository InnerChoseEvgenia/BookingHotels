using BookingHotelAPI.Application.Contracts;
using BookingHotelAPI.Application.MappingProfiles;
using BookingHotelAPI.Application.Services;
using BookingHotelAPI.CachePolicies;
using BookingHotelAPI.Common.Constants;
using BookingHotelAPI.Common.Models.Config;
using BookingHotelAPI.Domain.Data;
using BookingHotelAPI.Domain.Entities;
using BookingHotelAPI.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the IoC container.
var connectionString = builder.Configuration
    .GetConnectionString("HotelBookingDbConnectionString");
builder.Services.AddDbContextPool<HotelBookingDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.CommandTimeout(30);
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null
        );
    });
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }

    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); // ? Optional: Global no-tracking (only if most operations are read-only)
}, poolSize: 128);

builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
       .AddRoles<IdentityRole>()
       .AddEntityFrameworkStores<HotelBookingDbContext>();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();
if (string.IsNullOrWhiteSpace(jwtSettings.Key))
{
    throw new InvalidOperationException("JwtSettings:Key is not configured.");
}


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
             ValidIssuer = jwtSettings.Issuer,
             ValidAudience = jwtSettings.Audience,
             IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
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

builder.Services.AddAutoMapper(cfg => { }, typeof(HotelMappingProfile).Assembly);


builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//builder.Services.AddMemoryCache();
builder.Services.AddOutputCache(options =>
{
    options.AddPolicy(CacheConstants.AuthenticatedUserCachingPolicy, builder =>
    {
        builder.AddPolicy<AuthenticatedUserCachingPolicy>()
        .SetCacheKeyPrefix(CacheConstants.AuthenticatedUserCachingPolicyTag);
    }, true);
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(RateLimitingConstants.FixedPolicy, opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 50;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 5;
    });

    options.AddPolicy(RateLimitingConstants.PerUserPolicy, context =>
    {
        var username = context.User?.Identity?.Name ?? "anonymous";

        return RateLimitPartition.GetSlidingWindowLimiter(username, _ => new SlidingWindowRateLimiterOptions
        {
            Window = TimeSpan.FromMinutes(1),
            PermitLimit = 50,
            SegmentsPerWindow = 6,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 3
        });
    });

    // Global rate limit by IP
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ipAddress, _ => new FixedWindowRateLimiterOptions
        {
            Window = TimeSpan.FromMinutes(1),
            PermitLimit = 200,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 10
        });
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, cancellationToken) =>
    {
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            context.HttpContext.Response.Headers.RetryAfter = retryAfter.TotalSeconds.ToString();
        }

        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";

        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            error = "Too many requests",
            message = "Rate limit exceeded. Please try again later.",
            retryAfter = retryAfter.TotalSeconds
        }, cancellationToken: cancellationToken);
    };
});


var app = builder.Build();

app.MapGroup("api/defaultauth").MapIdentityApi<ApplicationUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRateLimiter();

app.UseAuthorization();

app.UseOutputCache();

app.MapControllers();

app.Run();
