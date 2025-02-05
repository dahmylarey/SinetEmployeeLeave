using Microsoft.EntityFrameworkCore;
using SinetEmployeeLeave.Data;
using SinetEmployeeLeave.Repository;

namespace SinetEmployeeLeave.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LeaveManagementDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(LeaveManagementDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        //All Employee on leave
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        //Get by ID
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        //Add
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        //Update
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }



    }
}
