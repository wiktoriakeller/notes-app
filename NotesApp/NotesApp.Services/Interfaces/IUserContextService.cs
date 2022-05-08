using System.Security.Claims;

namespace NotesApp.Services.Interfaces
{
    public interface IUserContextService
    {
        int? GetUserId { get; }
        ClaimsPrincipal User { get; }
    }
}