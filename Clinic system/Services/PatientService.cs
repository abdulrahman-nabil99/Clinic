using Clinic_system.Data;
using Clinic_system.Models;
using Microsoft.EntityFrameworkCore;

namespace Clinic_system.Services
{
    public class PatientService : GenericService<Patient>, IPatientService
    {
        public PatientService(ClinicdbContext _context) : base(_context)
        {
        }

        public async Task<Patient> GetPatientByPhoneAsync(string phone)
        {
            var patient = await _dbSet.FirstOrDefaultAsync(p => p.PhoneNumber == phone);
            return patient;
        }
    }
}
