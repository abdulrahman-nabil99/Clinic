using Clinic_system.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Clinic_system.Services
{
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
    {
        protected readonly ClinicdbContext _db;
        protected readonly DbSet<TEntity> _dbSet;
        public GenericService(ClinicdbContext _context)
        {
            _db = _context;
            _dbSet = _context.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TEntity>> Search (Func<TEntity,bool> func)
        {
            var data = await _dbSet.AsNoTracking().ToListAsync();
            data = data.Where(func).ToList();
            return data;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

    }
}
