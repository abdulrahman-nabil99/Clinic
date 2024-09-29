using Clinic_system.Data;
using Clinic_system.Models;
using Clinic_system.ViewModels;
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
        public async Task<Patient> GetOrCreatePatientAsync(BookViewModel model)
        {
            var patient = await GetPatientByPhoneAsync(model.Phone);
            if (patient is not { })
            {
                patient = new Patient
                {
                    FullName = model.Name,
                    PhoneNumber = model.Phone,
                    Email = model.Email
                };
                await AddAsync(patient);
            }
            else
            {
                patient.Email = model.Email;
                await UpdateAsync(patient);
            }
            return patient;
        }


    }
}
