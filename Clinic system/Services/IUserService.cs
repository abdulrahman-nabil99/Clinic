using Clinic_system.Models;

namespace Clinic_system.Services
{
    public interface IUserService : IGenericService<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<ICollection<User>> GetDoctorsAsync();
        Task<ICollection<User>> GetReceptionistsAsync();

    }
}
