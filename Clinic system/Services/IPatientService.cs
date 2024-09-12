using Clinic_system.Models;

namespace Clinic_system.Services
{
    public interface IPatientService: IGenericService<Patient>
    {
        Task<Patient> GetPatientByPhoneAsync(string phone);
    }
}
