namespace Clinic_system.Services
{
    public interface IGenericService<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task SaveChangesAsync();
        Task<IEnumerable<TEntity>> Search(Func<TEntity, bool> func);

    }
}
