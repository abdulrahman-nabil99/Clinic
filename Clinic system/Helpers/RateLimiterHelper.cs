using Microsoft.Extensions.Caching.Memory;

namespace Clinic_system.Helpers
{
    public class RateLimiterHelper
    {
        private readonly IMemoryCache _cache;

        public RateLimiterHelper(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string? GetClientIpAddress(HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress?.ToString();
        }

        public bool HasExceededRequestLimit(string action, HttpContext httpContext)
        {
            string? ipAddress = GetClientIpAddress(httpContext);
            string cacheKey = $"IpRequests_{ipAddress}_{action}";
            if (!_cache.TryGetValue(cacheKey, out int requestCount))
            {
                requestCount = 0;
            }

            if (requestCount >= 2) // Limit per 24 hours
            {
                return true;
            }

            _cache.Set(cacheKey, ++requestCount, TimeSpan.FromHours(24)); // 24 hours expiration
            return false;
        }
    }
}
