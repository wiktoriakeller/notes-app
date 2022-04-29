using System;
using NotesApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace NotesApp.Models
{
    public class NotesDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Login)
                .HasMaxLength(20);

            modelBuilder.Entity<Note>()
                .Property(p => p.NoteName)
                .IsRequired()
                .HasMaxLength(30);

            modelBuilder.Entity<Note>()
                .Property(n => n.Content)
                .IsRequired();

            modelBuilder.Entity<Tag>()
                .Property(t => t.TagName)
                .IsRequired()
                .HasMaxLength(10);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = entityEntry.Entity as BaseEntity;
                if(entity is not null)
                {
                    entity.UpdatedDate = DateTimeOffset.Now;

                    if (entityEntry.State == EntityState.Added)
                    {
                        entity.CreatedDate = DateTimeOffset.Now;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
