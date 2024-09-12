using Microsoft.AspNetCore.Identity;

namespace Clinic_system.Helpers
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<object>();
            return passwordHasher.HashPassword(null, password);
        }

        public static bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            var passwordHasher = new PasswordHasher<object>();
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, inputPassword);

            return result == PasswordVerificationResult.Success;
        }
    }
}
