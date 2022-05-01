using NotesApp.Services.Dto;

namespace NotesApp.Services.Interfaces
{
    public interface IUsersService
    {
        Task<int> AddUser(CreateUserDto dto);
    }
}