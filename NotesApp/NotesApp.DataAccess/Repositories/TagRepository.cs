using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;

namespace NotesApp.DataAccess.Repositories
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(NotesDbContext dbContext) : base(dbContext)
        {

        }
    }
}
