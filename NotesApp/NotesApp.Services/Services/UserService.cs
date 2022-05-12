using NotesApp.Services.Interfaces;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
using NotesApp.Services.Dto;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using NotesApp.Services.Authorization;
using System.Security.Cryptography;
using NotesApp.Services.Exceptions;

namespace NotesApp.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly AuthenticationSettings _authenticationSettings;

        public UserService(
            IUserRepository usersRepository, 
            IPasswordHasher<User> passwordHasher, 
            IMapper mapper,
            IEmailService emailService,
            AuthenticationSettings authenticationSettings)
        {
            _userRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _emailService = emailService;
            _authenticationSettings = authenticationSettings;
        }

        public async Task<int> AddUser(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = hashedPassword;

            await _userRepository.AddAsync(user);
            return user.Id;
        }

        public async Task<int> AddUser(RegisterUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            user.RoleId = 2;

            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = hashedPassword;

            await _userRepository.AddAsync(user);
            return user.Id;
        }

        public async Task<string> GenerateJwt(LoginDto dto)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(u => u.Login == dto.Login, "Role");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(_authenticationSettings.JwtExpireMinutes);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public async Task ForgotPassword(ForgotPasswordRequestDto dto)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(u => u.Email == dto.Email);
            
            if(user == null)
                throw new NotFoundException($"User with email: {dto.Email} doesn't exists");

            var token = GenerateResetToken();
            var tokenHash = ComputeHash(token);

            user.ResetToken = tokenHash;
            user.ResetTokenExpires = DateTimeOffset.Now.AddHours(2);

            await _userRepository.UpdateAsync(user);

            var passwordResetLink = "https://localhost:7164/notes-api/accounts/reset-password/";
            await _emailService.SendEmailAsync(new EmailMessage() { 
                To = dto.Email, 
                Subject = "Notes reset password", 
                Content = $"Click here to reset you password: {passwordResetLink}{token}\nThis link is only valid till: {user.ResetTokenExpires.Value}"
            });
        }

        public async Task ResetPassword(ResetPasswordDto dto, string token)
        {
            var tokenHash = ComputeHash(token);
            var user = await _userRepository.GetFirstOrDefaultAsync(u => u.ResetToken == tokenHash);

            if (user == null || user.ResetTokenExpires < DateTimeOffset.Now)
                throw new BadRequestException("Invalid or expired link");

            user.ResetToken = null;
            user.ResetTokenExpires = null;
            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = hashedPassword;
            await _userRepository.UpdateAsync(user);
        }

        private string GenerateResetToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToHexString(bytes);
            return token;
        }

        private string ComputeHash(string data)
        {
            using var sha256 = SHA256.Create();
            var tokenHash = string.Join("", sha256.ComputeHash(Encoding.UTF8.GetBytes(data)).Select(x => x.ToString("x2")));
            return tokenHash;
        }
    }
}
