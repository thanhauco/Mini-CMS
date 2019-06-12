using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniCMS.Domain.Entities;
using MiniCMS.Infrastructure.Data;

namespace MiniCMS.Infrastructure.Repositories
{
    public class SchemaRepository : IRepository<Schema>
    {
        private readonly MiniCmsDbContext _context;

        public SchemaRepository(MiniCmsDbContext context)
        {
            _context = context;
        }

        public async Task<Schema> GetByIdAsync(Guid id)
        {
            return await _context.Schemas.FindAsync(id);
        }

        public async Task<Schema> GetByNameAsync(Guid appId, string name)
        {
            return await _context.Schemas
                .FirstOrDefaultAsync(s => s.AppId == appId && s.Name == name);
        }

        public async Task<IEnumerable<Schema>> GetAllAsync()
        {
            return await _context.Schemas.ToListAsync();
        }

        public async Task<IEnumerable<Schema>> GetByAppAsync(Guid appId)
        {
            return await _context.Schemas
                .Where(s => s.AppId == appId)
                .ToListAsync();
        }

        public async Task<Schema> AddAsync(Schema entity)
        {
            await _context.Schemas.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Schema entity)
        {
            _context.Schemas.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var schema = await _context.Schemas.FindAsync(id);
            if (schema != null)
            {
                _context.Schemas.Remove(schema);
                await _context.SaveChangesAsync();
            }
        }
    }
}
