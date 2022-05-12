using NotesApp.Domain.Entities;

namespace NotesApp.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
