using NotesApp.Domain.Entities;

namespace NotesApp.Domain.Interfaces
{
    public interface INoteRepository : IRepository<Note>, IRepositoryAsync<Note>
    {

    }
}
