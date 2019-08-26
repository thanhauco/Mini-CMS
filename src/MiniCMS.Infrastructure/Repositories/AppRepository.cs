using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniCMS.Domain.Entities;
using MiniCMS.Infrastructure.Data;

namespace MiniCMS.Infrastructure.Repositories
{
    public class AppRepository : IRepository<App>
    {
        private readonly MiniCmsDbContext _context;

        public AppRepository(MiniCmsDbContext context)
        {
            _context = context;
        }

        public async Task<App> GetByIdAsync(Guid id)
        {
            return await _context.Apps
                .Include(a => a.Contributors)
                .Include(a => a.ApiKeys)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }

        public async Task<App> GetByNameAsync(string name)
        {
            return await _context.Apps
                .Include(a => a.Contributors)
                .Include(a => a.ApiKeys)
                .FirstOrDefaultAsync(a => a.Name == name && !a.IsDeleted);
        }

        public async Task<IEnumerable<App>> GetAllAsync()
        {
            return await _context.Apps
                .Include(a => a.Contributors)
                .Where(a => !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<App> AddAsync(App entity)
        {
            await _context.Apps.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(App entity)
        {
            _context.Apps.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var app = await _context.Apps.FindAsync(id);
            if (app != null)
            {
                app.IsDeleted = true;
                app.DeletedAt = DateTime.UtcNow;
                _context.Apps.Update(app);
                await _context.SaveChangesAsync();
            }
        }
    }
}
