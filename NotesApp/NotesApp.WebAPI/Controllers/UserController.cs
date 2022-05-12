using Microsoft.AspNetCore.Mvc;
using NotesApp.Services.Interfaces;
using NotesApp.Services.Dto;
using Microsoft.AspNetCore.Authorization;

namespace NotesApp.WebAPI.Controllers
{
    [Route("notes-api/accounts")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _usersService;

        public UserController(IUserService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("admin/create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            await _usersService.AddUser(dto);
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            await _usersService.AddUser(dto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            string token = await _usersService.GenerateJwt(dto);
            return Ok(token);
        }
    }
}
