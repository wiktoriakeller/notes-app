using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using NotesApp.Services.Interfaces;

namespace NotesApp.Services.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;
        public int? GetUserId =>
            User is null ? null : int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
