using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System;

namespace Clinic_system.Helpers
{
    public class OtpHelper
    {
        private readonly IMemoryCache _cache;

        public OtpHelper(IMemoryCache cache)
        {
            _cache = cache;
        }
        public string GenerateOtp(string phoneNumber)
        {
            Random random = new Random();
            string otp = random.Next(1, 100000).ToString("D5");
            string cacheKey = $"otp_{phoneNumber}";
            _cache.Set(cacheKey, otp , TimeSpan.FromMinutes(30)); // 30 min expiration
            return otp;
        }
        public bool VerifyOtp(string phoneNumber, string otp)
        {
            string cacheKey = $"otp_{phoneNumber}";

            if (string.IsNullOrEmpty(otp) || !_cache.TryGetValue(cacheKey, out string realOtp))
            {
                return false;
            }

            return realOtp == otp;
        }
    }
}
