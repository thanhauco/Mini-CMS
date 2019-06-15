using Microsoft.EntityFrameworkCore;
using MiniCMS.Domain.Entities;

namespace MiniCMS.Infrastructure.Data
{
    public class MiniCmsDbContext : DbContext
    {
        public DbSet<App> Apps { get; set; }
        public DbSet<Schema> Schemas { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Webhook> Webhooks { get; set; }

        public MiniCmsDbContext(DbContextOptions<MiniCmsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // App configuration
            modelBuilder.Entity<App>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.DisplayName).HasMaxLength(100);
                entity.OwnsMany(e => e.Contributors, c =>
                {
                    c.WithOwner();
                    c.Property(x => x.UserId).HasMaxLength(100);
                });
                entity.OwnsMany(e => e.ApiKeys, k =>
                {
                    k.WithOwner();
                    k.HasKey(x => x.Id);
                    k.Property(x => x.Key).HasMaxLength(100);
                    k.Property(x => x.Secret).HasMaxLength(100);
                });
            });

            // Schema configuration
            modelBuilder.Entity<Schema>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.AppId, e.Name }).IsUnique();
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.DisplayName).HasMaxLength(100);
                // Store fields as JSON
                entity.Ignore(e => e.Fields);
            });

            // Content configuration
            modelBuilder.Entity<Content>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.AppId);
                entity.HasIndex(e => e.SchemaId);
                entity.Property(e => e.Data).HasColumnType("jsonb");
            });

            // Asset configuration
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.AppId);
                entity.Property(e => e.FileName).HasMaxLength(255).IsRequired();
                entity.Property(e => e.MimeType).HasMaxLength(100);
                entity.Property(e => e.StoragePath).HasMaxLength(500);
                entity.Property(e => e.Metadata).HasColumnType("jsonb");
            });
        }
    }
}
