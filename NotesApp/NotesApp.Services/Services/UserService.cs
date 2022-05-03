using NotesApp.Services.Interfaces;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
using NotesApp.Services.Dto;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using System.Security.Claims;

namespace NotesApp.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(IUserRepository usersRepository, IPasswordHasher<User> passwordHasher, IMapper mapper)
        {
            _userRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<int> AddUser(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = hashedPassword;

            await _userRepository.AddAsync(user);
            return user.Id;
        }

        public async Task<string> GenerateJwt(LoginDto dto)
        {
            var users = await _userRepository.GetAllAsync(u => u.Login == dto.Login);
            var user = users.First();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };
        }
    }
}
