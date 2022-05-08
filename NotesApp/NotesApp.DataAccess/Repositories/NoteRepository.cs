using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
 
namespace NotesApp.DataAccess.Repositories
{
    public class NoteRepository : BaseRepository<Note>, INoteRepository
    {
        public NoteRepository(NotesDbContext dbContext) : base(dbContext)
        {
        }
    }
}
