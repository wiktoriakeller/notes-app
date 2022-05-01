using NotesApp.Services.Interfaces;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
using NotesApp.Services.Dto;
using Microsoft.AspNetCore.Identity;

namespace NotesApp.Services.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UsersService(IUsersRepository usersRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = usersRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<int> AddUser(CreateUserDto dto)
        {
            var user = new User()
            {
                Login = dto.Login,
                Email = dto.Email,
                PasswordHash = dto.Password,
                RoleId = dto.RoleId
            };

            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = hashedPassword;

            await _userRepository.AddAsync(user);
            return user.Id;
        }
    }
}
