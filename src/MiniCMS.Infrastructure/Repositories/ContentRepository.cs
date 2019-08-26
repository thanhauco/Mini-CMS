using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniCMS.Domain.Entities;
using MiniCMS.Infrastructure.Data;

namespace MiniCMS.Infrastructure.Repositories
{
    public class ContentRepository : IRepository<Content>
    {
        private readonly MiniCmsDbContext _context;

        public ContentRepository(MiniCmsDbContext context)
        {
            _context = context;
        }

        public async Task<Content> GetByIdAsync(Guid id)
        {
            return await _context.Contents
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<IEnumerable<Content>> GetAllAsync()
        {
            return await _context.Contents
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Content>> GetByAppAndSchemaAsync(Guid appId, Guid schemaId)
        {
            return await _context.Contents
                .Where(c => c.AppId == appId && c.SchemaId == schemaId && !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Content>> GetPublishedAsync(Guid appId, Guid schemaId)
        {
            return await _context.Contents
                .Where(c => c.AppId == appId && 
                            c.SchemaId == schemaId && 
                            c.Status == ContentStatus.Published &&
                            !c.IsDeleted)
                .OrderByDescending(c => c.PublishedAt)
                .ToListAsync();
        }

        public async Task<Content> AddAsync(Content entity)
        {
            await _context.Contents.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Content entity)
        {
            _context.Contents.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var content = await _context.Contents.FindAsync(id);
            if (content != null)
            {
                content.IsDeleted = true;
                content.DeletedAt = DateTime.UtcNow;
                _context.Contents.Update(content);
                await _context.SaveChangesAsync();
            }
        }
    }
}
