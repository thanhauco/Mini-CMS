using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniCMS.Domain.Entities;
using MiniCMS.Infrastructure.Data;

namespace MiniCMS.Infrastructure.Repositories
{
    public class WebhookRepository : IRepository<Webhook>
    {
        private readonly MiniCmsDbContext _context;

        public WebhookRepository(MiniCmsDbContext context)
        {
            _context = context;
        }

        public async Task<Webhook> GetByIdAsync(Guid id)
        {
            return await _context.Set<Webhook>().FindAsync(id);
        }

        public async Task<IEnumerable<Webhook>> GetAllAsync()
        {
            return await _context.Set<Webhook>().ToListAsync();
        }

        public async Task<IEnumerable<Webhook>> GetByAppAsync(Guid appId)
        {
            return await _context.Set<Webhook>()
                .Where(w => w.AppId == appId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Webhook>> GetByEventAsync(Guid appId, WebhookEvent @event)
        {
            return await _context.Set<Webhook>()
                .Where(w => w.AppId == appId && w.IsEnabled && w.Events.HasFlag(@event))
                .ToListAsync();
        }

        public async Task<Webhook> AddAsync(Webhook entity)
        {
            await _context.Set<Webhook>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Webhook entity)
        {
            _context.Set<Webhook>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var webhook = await _context.Set<Webhook>().FindAsync(id);
            if (webhook != null)
            {
                _context.Set<Webhook>().Remove(webhook);
                await _context.SaveChangesAsync();
            }
        }
    }
}
