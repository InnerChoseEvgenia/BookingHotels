namespace BookingHotelAPI.Common.Constants;

public static class CacheConstants
{
    // Output Cache Policy Names
    public const string AuthenticatedUserCachingPolicy = "AuthenticatedUserCachingPolicy";

    public const string AuthenticatedUserCachingPolicyTag = "authpolicy-";

    public const string CountrySingleCacheName = "country_";
    public const string CountryListCacheName = "countries_list_";

    // Cache Durations (in seconds)
    public const int ShortDuration = 60; // 1 minute
    public const int MediumDuration = 300; // 5 minutes
    public const int LongDuration = 900; // 15 minutes
}
