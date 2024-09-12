using Clinic_system.Data;
using Clinic_system.Models;
using Microsoft.EntityFrameworkCore;

namespace Clinic_system.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        public UserService(ClinicdbContext _context) : base(_context)
        {
        }

        public async Task<ICollection<User>> GetDoctorsAsync()
        {
            var doctors = await _dbSet.Where(u=>u.RoleId==2).ToListAsync();
            return doctors;
        }

        public async Task<ICollection<User>> GetReceptionistsAsync()
        {
            var receptionists = await _dbSet.Where(u => u.RoleId == 3).ToListAsync();
            return receptionists;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _dbSet.SingleOrDefaultAsync(u=> u.Email == email);
            return user;
        }
    }
}
