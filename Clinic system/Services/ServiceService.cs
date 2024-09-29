using Clinic_system.Data;
using Clinic_system.Helpers;
using Clinic_system.Models;
using Microsoft.EntityFrameworkCore;

namespace Clinic_system.Services
{
    public class ServiceService : GenericService<Service>, IServiceService
    {
        public ServiceService(ClinicdbContext _context) : base(_context)
        {
        }

        public async Task<IEnumerable<Service>> GetAllActiveServicesAsync()
        {
            var services = await _dbSet.Where(s=>s.IsActive).ToListAsync();
            return services;
        }
        public async Task UpdateImage(Service service, IFormFile file)
        {
            if (!service.ServiceImage.Contains("default"))
                DeleteImage(service);
            var image = await ImageUploadHelper
                .UploadImage(file, "services", $"service-{service.ServiceId}-img");
            service.ServiceImage = image;
            await UpdateAsync(service);
        }

        public void DeleteImage(Service service)
        {
            ImageUploadHelper.DeleteImage("services", service.ServiceImage);
        }
    }
}
