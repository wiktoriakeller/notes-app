using NotesApp.Domain.Entities;

namespace NotesApp.Domain.Interfaces
{
    public interface ITagRepository : IRepository<Tag>, IRepositoryAsync<Tag>
    {

    }
}
