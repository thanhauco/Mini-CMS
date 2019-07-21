using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniCMS.Domain.Entities;
using MiniCMS.Infrastructure.Data;

namespace MiniCMS.Infrastructure.Repositories
{
    public class AuditLogRepository : IRepository<AuditLog>
    {
        private readonly MiniCmsDbContext _context;

        public AuditLogRepository(MiniCmsDbContext context)
        {
            _context = context;
        }

        public async Task<AuditLog> GetByIdAsync(Guid id)
        {
            return await _context.Set<AuditLog>().FindAsync(id);
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _context.Set<AuditLog>()
                .OrderByDescending(l => l.CreatedAt)
                .Take(100)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByUserAsync(string userId)
        {
            return await _context.Set<AuditLog>()
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<AuditLog> AddAsync(AuditLog entity)
        {
            await _context.Set<AuditLog>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task UpdateAsync(AuditLog entity)
        {
            throw new NotSupportedException("Audit logs cannot be updated");
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotSupportedException("Audit logs cannot be deleted");
        }
    }
}
