using Clinic_system.Models;

namespace Clinic_system.Services
{
    public interface IServiceService : IGenericService<Service>
    {
        Task<IEnumerable<Service>> GetAllActiveServicesAsync();
        Task UpdateImage(Service article, IFormFile file);
        void DeleteImage(Service article);
    }
}
