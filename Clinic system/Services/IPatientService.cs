using Clinic_system.Models;
using Clinic_system.ViewModels;

namespace Clinic_system.Services
{
    public interface IPatientService: IGenericService<Patient>
    {
        Task<Patient> GetPatientByPhoneAsync(string phone);
        Task<Patient> GetOrCreatePatientAsync(BookViewModel model);
    }
}
