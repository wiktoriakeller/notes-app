using NotesApp.Services.Dto;

namespace NotesApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> AddUser(CreateUserDto dto);
        Task<string> GenerateJwt(LoginDto dto);
    }
}