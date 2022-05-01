using Microsoft.AspNetCore.Mvc;
using NotesApp.Services.Interfaces;
using NotesApp.Services.Dto;

namespace NotesApp.WebAPI.Controllers
{
    [Route("notes-api/account")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto dto)
        {
            await _usersService.AddUser(dto);
            return Ok();
        }
    }
}
