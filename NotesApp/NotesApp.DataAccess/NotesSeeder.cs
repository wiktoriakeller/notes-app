using NotesApp.Domain.Entities;

namespace NotesApp.DataAccess
{
    public class NotesSeeder
    {
        private readonly NotesDbContext _dbContext;

        public NotesSeeder(NotesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if(_dbContext.Database.CanConnect())
            {
                if(!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    await _dbContext.Roles.AddRangeAsync(roles);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role() { RoleName = "User" },
                new Role() { RoleName = "Admin"}
            };

            return roles;
        }
    }
}
