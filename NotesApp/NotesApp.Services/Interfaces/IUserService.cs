using NotesApp.Services.Dto;

namespace NotesApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> AddUser(CreateUserDto dto);
        Task<int> AddUser(RegisterUserDto dto);
        Task<string> GenerateJwt(LoginDto dto);
        Task ForgotPassword(ForgotPasswordRequestDto dto);
        Task ResetPassword(ResetPasswordDto dto, string token);
    }
}