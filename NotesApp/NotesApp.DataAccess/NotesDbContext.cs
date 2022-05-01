using NotesApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace NotesApp.DataAccess
{
    public class NotesDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
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
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Login)
                .HasMaxLength(20);

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(r => r.RoleName)
                .IsRequired();

            modelBuilder.Entity<Note>()
                .HasIndex(n => new { n.NoteName, n.UserId })
                .IsUnique();

            modelBuilder.Entity<Note>()
                .Property(p => p.NoteName)
                .HasMaxLength(40);

            modelBuilder.Entity<Note>()
                .Property(n => n.Content)
                .IsRequired();

            modelBuilder.Entity<Tag>()
                .HasIndex(t => new { t.TagName, t.NoteId });

            modelBuilder.Entity<Tag>()
                .Property(t => t.TagName)
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
                if (entity is not null)
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
